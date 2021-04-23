using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Wicked.Favorites
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.AddEnvironmentVariables().Build();

                    var vaultUri = new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/");
                    var azureTenantId = builtConfig["AzureADDirectoryId"];
                    var azureClientId = builtConfig["AzureADApplicationId"];
                    var azureSecretId = builtConfig["AzureADApplicationSecret"];

                    var credential = new ClientSecretCredential(azureTenantId, azureClientId, azureSecretId);

                    config.AddAzureKeyVault(vaultUri, credential, new KeyVaultSecretManager());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
