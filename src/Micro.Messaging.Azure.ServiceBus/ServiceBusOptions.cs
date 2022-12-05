namespace Micro.Messaging.Azure.ServiceBus;

internal class ServiceBusOptions
{
    public const string SectionName = "AzureServiceBus";
    public string ConnectionString { get; set; } = string.Empty;
}