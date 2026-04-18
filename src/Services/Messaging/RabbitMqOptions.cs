namespace NC.Messaging;


/// <summary> Configuration options for RabbitMQ messaging infrastructure. </summary>
public class RabbitMqOptions
{
    /// <summary> The configuration section name in appsettings.json. </summary>
    public const string SectionName = "RabbitMQ";

    /// <summary> The name of the Dead Letter Exchange. </summary>
    public string DeadLetterExchange { get; set; } = "nexus_core_dlx";

    /// <summary> The routing key used to send messages to the Dead Letter Queue. </summary>
    public string DeadLetterRoutingKey { get; set; } = "dead_letter";

    /// <summary> The name of the global queue that catches unhandled dead letters. </summary>
    public string GlobalDlq { get; set; } = "nexus_core_global_dlq";
}