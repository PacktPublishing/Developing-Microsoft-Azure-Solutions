using System;
using Microsoft.Extensions.Configuration;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class AppSettings
    {

        private const string SHRINKIFYSTORAGEACCOUNT = "SHRINKIFYSTORAGEACCOUNT";
        private const string SHRINKIFYSERVICEBUSACCOUNT = "SHRINKIFYSERVICEBUSACCOUNT";

        public AppSettings(IConfiguration configuration)
        {
            CheckIsNotNull(nameof(configuration), configuration);

            StorageAccount = configuration[SHRINKIFYSTORAGEACCOUNT];
            CheckIsNotNull(nameof(SHRINKIFYSTORAGEACCOUNT), StorageAccount);

            ServiceBusAccount = configuration[SHRINKIFYSERVICEBUSACCOUNT];
            CheckIsNotNull(nameof(SHRINKIFYSERVICEBUSACCOUNT), ServiceBusAccount);
        }

        public string StorageAccount { get; set; }

        public string ServiceBusAccount { get; set; }
    }
}
