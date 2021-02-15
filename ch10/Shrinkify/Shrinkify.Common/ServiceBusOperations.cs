using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public static class ServiceBusOperations
    {
        private const string SUBSCRIPTION_FILTER = "SubscriptionFilter";

        private static string SerializeMessage<T>(T message) where T : class
        {
            return JsonConvert.SerializeObject(message);
        }

        private static T DeserializeMessage<T>(string message) where T : class
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

public static async Task SendMessageAsync<T>(string connectionString, string topic, T message, string correlationId = null, string subscriptionFilter = null, IDictionary<string, string> properties = null) where T : class
{
    CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
    CheckIsNotNullOrWhitespace(nameof(topic), topic);
            
    await using (var client = new ServiceBusClient(connectionString))
    {
        await using (var sender = client.CreateSender(topic))
        {
            var m = new ServiceBusMessage(SerializeMessage(message));
                    
            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                m.CorrelationId = correlationId;
            }
                    
            if (!string.IsNullOrWhiteSpace(subscriptionFilter))
            {
                Debug.WriteLine($"CHECK: {subscriptionFilter}");

                m.ApplicationProperties.Add(SUBSCRIPTION_FILTER, subscriptionFilter.ToLower());
            }

            if (properties != null)
            {
                foreach (var kv in properties)
                {
                    m.ApplicationProperties.Add(kv.Key, kv.Value);
                }
            }

            await sender.SendMessageAsync(m);
        }
    }
}

        public static async Task CreateSubscription(string connectionString, string topic, string subscription)
        {
            var client = new ServiceBusAdministrationClient(connectionString);

            await CreateSubscription(client, topic, subscription);
        }

        public static async Task CreateSubscription(ServiceBusAdministrationClient client, string topic, string subscription)
        {
            CheckIsNotGreaterThan(nameof(subscription), subscription.Length, 50);

            bool subscriptionExists = await client.SubscriptionExistsAsync(topic, subscription);

            if (!subscriptionExists)
            {
                var options = new CreateSubscriptionOptions(topic, subscription);

                _ = await client.CreateSubscriptionAsync(options);
            }
        }

public static async Task CreateSubscription(string connectionString,
                                            string topic,
                                            string subscription,
                                            string subscriptionFilter)
{
    CheckIsNotNullOrWhitespace(nameof(subscriptionFilter), subscriptionFilter);

    Debug.WriteLine($"CHECK: {subscriptionFilter}");

    string filterClause = $"{SUBSCRIPTION_FILTER} IN ('{subscriptionFilter.ToLower()}')";

    var client = new ServiceBusAdministrationClient(connectionString);

    await CreateSubscription(client, topic, subscription);

    await client.DeleteRuleAsync(topic, subscription, CreateRuleOptions.DefaultRuleName);

    var rule = new SqlRuleFilter(filterClause);

    await client.CreateRuleAsync(topic, subscription, new CreateRuleOptions(SUBSCRIPTION_FILTER, rule));
}

        public static async Task DeleteSubscription(string connectionString, string topic, string subscription)
        {
            var client = new ServiceBusAdministrationClient(connectionString);

            bool subscriptionExists = await client.SubscriptionExistsAsync(topic, subscription);

            if (subscriptionExists)
            {
                await client.DeleteSubscriptionAsync(topic, subscription);
            }
        }

        public static async Task EnsureTopicExists(string connectionString, string topic)
        {
            var sbClient = new ServiceBusAdministrationClient(connectionString);
            _ = await sbClient.CreateTopicAsync(topic);
        }

        public static async Task DeleteTopic(string connectionString, string topic)
        {
            var sbClient = new ServiceBusAdministrationClient(connectionString);
            _ = await sbClient.DeleteTopicAsync(topic);
        }
    }
}
