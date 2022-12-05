namespace Micro.Messaging.RabbitMQ;

internal class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";
    public string ConnectionString { get; set; } = string.Empty;
}