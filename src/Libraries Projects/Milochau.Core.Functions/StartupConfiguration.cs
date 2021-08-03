using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using Milochau.Core.Infrastructure.Hosting;
using Azure.Identity;

namespace Milochau.Core.Functions
{
    /// <summary>Startup configuration</summary>
    /// <remarks>
    /// These configuration providers will be used:
    /// <list type="bullet">
    ///    <item>Azure App Configuration</item>
    ///    <item>Azure Key Vault</item>
    ///    <item>JSON file appsettings.{host}.json</item>
    /// </list>
    /// </remarks>
    public static class StartupConfiguration
    {
        /// <summary>Configuration refresher</summary>
        public static IConfigurationRefresher ConfigurationRefresher { get; set; }

        /// <summary>Register configuration into <paramref name="configurationBuilder"/></summary>
        /// <param name="hostingContextConfiguration">Hosting context configuration</param>
        /// <param name="configurationBuilder">Configuration builder where to add new configuration providers</param>
        public static void RegisterConfiguration(IConfiguration hostingContextConfiguration, IConfigurationBuilder configurationBuilder)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(hostingContextConfiguration);
            
            // Create specific configuration builder for new configuration sources: the first one will override the next ones
            var internalConfigurationBuilder = new ConfigurationBuilder();

            // Configure appsettings.{host}.json
            internalConfigurationBuilder.AddJsonFile($"appsettings.{hostOptions.Application.HostName}.json", optional: true, reloadOnChange: false);

            // Configure Azure Key Vault
            if (!string.IsNullOrEmpty(hostOptions.KeyVault.Vault))
            {
                var credential = new DefaultAzureCredential(hostOptions.Credential);
                internalConfigurationBuilder.AddAzureKeyVault(new Uri(hostOptions.KeyVault.Vault), credential);
            }

            // Configure Azure App Configuration
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.ConnectionString) || !string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                internalConfigurationBuilder.AddAzureAppConfiguration(appConfigOptions =>
                {
                    AppConfigurationRegistration.ConfigureAzureAppConfiguration(appConfigOptions, hostOptions);
                    ConfigurationRefresher = appConfigOptions.GetRefresher();
                });
            }
            else
            {
                throw new NotSupportedException("You need to set up an Azure App Configuration to use IConfigurationRefresher");
            }

            // Add new configuration sources at the beginning of the application configuration builder
            foreach (var configurationSource in internalConfigurationBuilder.Sources)
            {
                configurationBuilder.Sources.Insert(0, configurationSource);
            }
        }
    }
}
