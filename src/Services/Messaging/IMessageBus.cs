namespace NC.Messaging;

/// <summary>
/// Future-proof interface for the Nexus Core Message Bus.
/// Supports Competing Consumers (Queue) and Pub/Sub (Topic).
/// </summary>
public interface IMessageBus : IDisposable
{
    // One-to-One (Competing Consumers)
    Task SendToQueueAsync<T>(string queueName, T message);
    Task ConsumeFromQueue<T>(string queueName, Func<T, Task<bool>> onMessage);

    // One-to-Many (Publish/Subscribe)
    Task PublishToTopicAsync<T>(string topicName, T message);
    Task SubscribeToTopic<T>(string topicName, string subscriberId, Func<T, Task<bool>> onMessage);
}
