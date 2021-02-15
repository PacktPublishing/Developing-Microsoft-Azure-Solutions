using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static Shrinkify.ShrinkifyExtensions;
using static Shrinkify.QueueOperations;
using static Shrinkify.ServiceBusOperations;
using static Pineapple.Common.Preconditions;
using System.Diagnostics;

namespace Shrinkify
{
    public class FunctionsDependencies : Dependencies<Functions>
    {
        public FunctionsDependencies(ILogger<Functions> logger, AppSettings settings) : base(logger, settings)
        {
        }
    }

    public class Functions
    {
        private readonly IDependencies<Functions> _d;

        public Functions(IDependencies<Functions> dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);

            _d = dependencies;
        }

        [FunctionName("Shrink")]
        public async Task Run([QueueTrigger("shrink", Connection = "SHRINKIFYSTORAGEACCOUNT")]string queueMessage, ILogger log)
        {
            const int TIMELINESS = 5;

            log.LogInformation($"C# Queue trigger function processed: {queueMessage}");

            try
            {
                var shrinkMessage = DeserializeMessageFromAzureFunction<ShrinkMessage>(queueMessage);

                // IGNORE OLD MESSAGES
                if (shrinkMessage.When.AddMinutes(TIMELINESS) < DateTimeOffset.UtcNow)
                {
                    var shrunkImage = await ShrinkImageAsync(shrinkMessage.Image, _d.Settings);

                    Debug.WriteLine($"Sending message [{shrunkImage.ShrunkImageUrl}]");

                    await SendMessageAsync(_d.Settings.ServiceBusAccount, "notifications", shrunkImage, subscriptionFilter: shrunkImage.Folder);

                    Debug.WriteLine($"Sent message [{shrunkImage.ShrunkImageUrl}]");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}
