using System;
using Microsoft.Extensions.Configuration;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class AppSettings
    {

        private const string SHRINKIFYSTORAGEACCOUNT = "SHRINKIFYSTORAGEACCOUNT";

        public AppSettings(IConfiguration configuration)
        {
            CheckIsNotNull(nameof(configuration), configuration);

            StorageAccount = configuration[SHRINKIFYSTORAGEACCOUNT];
            CheckIsNotNull(nameof(SHRINKIFYSTORAGEACCOUNT), StorageAccount);
        }

        public string StorageAccount { get; set; }
    }
}
