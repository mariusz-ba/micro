using Azure.Messaging.ServiceBus.Administration;
using Micro.Messaging.Abstractions;
using Micro.Messaging.Azure.ServiceBus.Conventions;
using Microsoft.Extensions.Hosting;

namespace Micro.Messaging.Azure.ServiceBus;

internal sealed class ServiceBusInitializer : IHostedService
{
    private readonly ServiceBusAdministrationClient _administrationClient;
    private readonly IMessageBrokerConventions _messageBrokerConventions;

    public ServiceBusInitializer(
        ServiceBusAdministrationClient administrationClient,
        IMessageBrokerConventions messageBrokerConventions)
    {
        _administrationClient = administrationClient;
        _messageBrokerConventions = messageBrokerConventions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var messageTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IMessage).IsAssignableFrom(t));

        foreach (var messageType in messageTypes)
        {
            var topic = _messageBrokerConventions.GetTopicName(messageType);

            await CreateTopicAsync(topic, cancellationToken);

            var subscription = _messageBrokerConventions.GetSubscriptionName(messageType);
            if (subscription is not null)
            {
                await CreateSubscriptionAsync(topic, subscription, cancellationToken);
            }
        }
    }

    private async Task CreateTopicAsync(string topic, CancellationToken cancellationToken)
    {
        var topicExists = await _administrationClient.TopicExistsAsync(topic, cancellationToken);
        if (topicExists.Value is false)
        {
            await _administrationClient.CreateTopicAsync(topic, cancellationToken);
        }
    }

    private async Task CreateSubscriptionAsync(string topic, string subscription, CancellationToken cancellationToken)
    {
        var subscriptionExists = await _administrationClient.SubscriptionExistsAsync(topic, subscription, cancellationToken);
        if (subscriptionExists.Value is false)
        {
            await _administrationClient.CreateSubscriptionAsync(topic, subscription, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}