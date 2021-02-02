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

            var m = JsonConvert.SerializeObject(message);

            byte[] buffer = Encoding.Unicode.GetBytes(m);
            string msg = Convert.ToBase64String(buffer);

            var queueClient = new QueueClient(connectionString, queueName);
            await queueClient.SendMessageAsync(msg);
        }

        public static T DeserializeMessage<T>(string message)
        {
            var buffer = Convert.FromBase64String(message);
            var s = Encoding.Unicode.GetString(buffer);
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
