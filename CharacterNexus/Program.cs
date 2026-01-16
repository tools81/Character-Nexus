using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

namespace CharacterNexus
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
                    var builtConfig = config.Build();
                    var vaultUri = builtConfig["KeyVault:Vault"];                    

                    if (!string.IsNullOrEmpty(vaultUri))
                    {
                        if (Debugger.IsAttached)
                        {
                            var clientSecretCredential = new ClientSecretCredential(
                                                                Environment.GetEnvironmentVariable("AZURE_TENANT_ID"),
                                                                Environment.GetEnvironmentVariable("AZURE_CLIENT_ID"),
                                                                Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET"));

                            config.AddAzureKeyVault(new Uri(vaultUri), clientSecretCredential);
                        }
                        else
                        {
                            config.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential());
                        }
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
