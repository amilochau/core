using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Configuration registration</summary>
    /// <remarks>
    /// These configuration providers will be used:
    /// <list type="bullet">
    ///    <item>Azure App Configuration</item>
    ///    <item>Azure Key Vault</item>
    ///    <item>JSON file appsettings.{host}.json</item>
    /// </list>
    /// </remarks>
    public static class ConfigurationRegistration
    {
        /// <summary>Add application configuration providers to the configuration builder <paramref name="configurationBuilder"/></summary>
        /// <param name="hostingContextConfiguration">Hosting context configuration</param>
        /// <param name="configurationBuilder">Configuration builder</param>
        public static void AddApplicationConfiguration(IConfiguration hostingContextConfiguration, IConfigurationBuilder configurationBuilder)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(hostingContextConfiguration);

            // Create specific configuration builder for new configuration sources: the first one will override the next ones
            var internalConfigurationBuilder = new ConfigurationBuilder();

            // Configure appsettings.{host}.json
            internalConfigurationBuilder.AddJsonFile($"appsettings.{hostOptions.Application.HostName}.json", optional: true, reloadOnChange: false);

            // Configure Azure Key Vault
            if (!string.IsNullOrEmpty(hostOptions.KeyVault.Vault))
            {
                var credential  = new DefaultAzureCredential(hostOptions.Credential);
                internalConfigurationBuilder.AddAzureKeyVault(new Uri(hostOptions.KeyVault.Vault), credential);
            }

            // Configure Azure App Configuration
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.ConnectionString) || !string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                internalConfigurationBuilder.AddAzureAppConfiguration(appConfigOptions =>
                {
                    AppConfigurationRegistration.ConfigureAzureAppConfiguration(appConfigOptions, hostOptions);
                });
            }

            // Add new configuration sources at the beginning of the application configuration builder
            foreach (var configurationSource in internalConfigurationBuilder.Sources)
            {
                configurationBuilder.Sources.Insert(0, configurationSource);
            }
        }
    }
}
