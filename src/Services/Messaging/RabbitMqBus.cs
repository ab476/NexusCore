using NC.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace NC.Messaging;

/// <summary>
/// A thread-safe RabbitMQ implementation of the message bus.
/// </summary>
/// <param name="connection">The established RabbitMQ connection.</param>
/// <param name="serializer">The serialization service for message bodies.</param>
public sealed class RabbitMqBus(IConnection connection, ISerializer serializer) : IMessageBus, IDisposable
{
    private readonly IConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

    /// <summary> Channel used for publishing operations with concurrency control. </summary>
    /// <remarks>May be null until initialized. Access must be synchronized to ensure thread-safe use.</remarks>
    private IChannel? _publishChannel;
    private readonly SemaphoreSlim _channelLock = new(1, 1);

    /// <summary> Track consumer channels to cleanly dispose of them later </summary>
    private readonly ConcurrentBag<IChannel> _consumerChannels = [];

    private const string DeadLetterExchange = "nexus_core_dlx";
    private const string DeadLetterRoutingKey = "dead_letter";

    /// <summary>
    /// Safely initializes and returns a shared channel for publishing messages.
    /// </summary>
    private async Task<IChannel> GetPublishChannelAsync()
    {
        if (_publishChannel is not null)
            return _publishChannel;

        await _channelLock.WaitAsync();
        try
        {
            // Double-check locking pattern
            _publishChannel ??= await _connection.CreateChannelAsync();
            return _publishChannel;
        }
        finally
        {
            _channelLock.Release();
        }
    }

    /// <summary>
    /// Sends a persistent message to a specific point-to-point queue.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <param name="queueName">The destination queue name.</param>
    /// <param name="message">The message payload.</param>
    public async Task SendToQueueAsync<T>(string queueName, T message)
    {
        var channel = await GetPublishChannelAsync();

        // Note: In highly optimized production, you might move declarations out of the publish path
        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

        var body = _serializer.Serialize(message);
        var props = new BasicProperties { Persistent = true };

        // Even with a shared channel, BasicPublishAsync handles its own internal safety in v7,
        // but the channel creation MUST be thread-safe.
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body);
    }

    /// <summary>
    /// Publishes a persistent message to a fanout topic for multiple subscribers.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <param name="topicName">The destination topic (exchange) name.</param>
    /// <param name="message">The message payload.</param>
    public async Task PublishToTopicAsync<T>(string topicName, T message)
    {
        var channel = await GetPublishChannelAsync();
        await channel.ExchangeDeclareAsync(topicName, ExchangeType.Fanout, durable: true);

        var body = _serializer.Serialize(message);
        var props = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(
            exchange: topicName,
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: props,
            body: body);
    }

    /// <summary>
    /// Subscribes a handler to continuously consume messages from a specific queue.
    /// </summary>
    /// <typeparam name="T">The expected message type.</typeparam>
    /// <param name="queueName">The queue to consume from.</param>
    /// <param name="onMessage">The asynchronous handler function.</param>
    public async Task ConsumeFromQueue<T>(string queueName, Func<T, Task<bool>> onMessage)
    {
        // 2. Consumers get their own dedicated channel to avoid blocking publishers
        var channel = await _connection.CreateChannelAsync();
        _consumerChannels.Add(channel);

        var args = CreateDLXArguments();

        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
        await channel.BasicQosAsync(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        // Fixed Warning RCS1163: Using discard '_' for unused 'ch' parameter
        consumer.ReceivedAsync += async (_, ea) => await HandleMessage(channel, ea, onMessage);

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
    }

    /// <summary>
    /// Subscribes a handler to a topic using a unique subscriber queue.
    /// </summary>
    /// <typeparam name="T">The expected message type.</typeparam>
    /// <param name="topicName">The topic (exchange) to subscribe to.</param>
    /// <param name="subscriberId">A unique identifier for this subscriber instance.</param>
    /// <param name="onMessage">The asynchronous handler function.</param>
    public async Task SubscribeToTopic<T>(string topicName, string subscriberId, Func<T, Task<bool>> onMessage)
    {
        var channel = await _connection.CreateChannelAsync();
        _consumerChannels.Add(channel);

        string queueName = $"{topicName}.{subscriberId}";
        var args = CreateDLXArguments();

        await channel.ExchangeDeclareAsync(topicName, ExchangeType.Fanout, durable: true);
        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
        await channel.QueueBindAsync(queueName, topicName, string.Empty);

        await channel.BasicQosAsync(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        // Fixed Warning RCS1163
        consumer.ReceivedAsync += async (_, ea) => await HandleMessage(channel, ea, onMessage);

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
    }

    /// <summary>
    /// Internal pipeline for deserializing and processing incoming messages.
    /// Routes failed messages to the Dead Letter Queue.
    /// </summary>
    private async Task HandleMessage<T>(IChannel channel, BasicDeliverEventArgs ea, Func<T, Task<bool>> onMessage)
    {
        try
        {
            var message = _serializer.Deserialize<T>(ea.Body.ToArray());

            if (message == null)
            {
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                return;
            }

            bool success = await onMessage(message);

            if (success)
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            else
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
        }
        catch
        {
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
        }
    }

    /// <summary>
    /// Generates the standard arguments for binding queues to the global Dead Letter Exchange.
    /// </summary>
    private static Dictionary<string, object?> CreateDLXArguments() => new()
    {
        { "x-dead-letter-exchange", DeadLetterExchange },
        { "x-dead-letter-routing-key", DeadLetterRoutingKey }
    };

    /// <summary>
    /// Disposes of the active publishing channel and all tracked consumer channels.
    /// </summary>
    public void Dispose()
    {
        _publishChannel?.Dispose();
        _channelLock?.Dispose();

        foreach (var channel in _consumerChannels)
        {
            channel?.Dispose();
        }

        // Fixed Warning CA1816: Tells GC not to call the finalizer since we already cleaned up
        GC.SuppressFinalize(this);
    }
}