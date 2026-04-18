using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NC.Messaging;

public class RabbitMqInfrastructureInitializer(IConnection connection, IOptions<RabbitMqOptions> options) : BackgroundService
{
    private RabbitMqOptions Options => options?.Value ?? throw new ArgumentNullException(nameof(options));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        // 1. Declare the Dead Letter Exchange
        await channel.ExchangeDeclareAsync(Options.DeadLetterExchange, ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);

        // 2. Optional: Declare a global DLQ to catch all dead letters
        await channel.QueueDeclareAsync(Options.GlobalDlq, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(Options.GlobalDlq, Options.DeadLetterExchange, Options.DeadLetterRoutingKey, cancellationToken: stoppingToken);

    }
}
