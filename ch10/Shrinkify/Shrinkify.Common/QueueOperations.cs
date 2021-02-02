using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public static class QueueOperations
    {
        public static async Task SendMessage<T>(string connectionString, string queueName, T message) where T: class
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(queueName), queueName);

            var m = SerializeMessage(message);
            var queueClient = new QueueClient(connectionString, queueName);
            await queueClient.SendMessageAsync(m);
        }


        public static string SerializeMessage<T>(T message) where T : class
        {
            var plainText = JsonConvert.SerializeObject(message);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static T DeserializeMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
