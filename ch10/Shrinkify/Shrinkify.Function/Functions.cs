using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static Shrinkify.ShrinkifyExtensions;
using static Shrinkify.QueueOperations;
using static Pineapple.Common.Preconditions;

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
            log.LogInformation($"C# Queue trigger function processed: {queueMessage}");

            try
            {
                var shrinkMessage = DeserializeMessage<ShrinkMessage>(queueMessage);

                await ShrinkImageAsync(shrinkMessage.Image, _d.Settings);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            finally
            {
                // TODO: Send to user who is waiting for the image (HMM:  SignalR???)
            }
        }
    }
}
