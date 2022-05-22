using Microsoft.Extensions.Configuration;
using Milochau.Core.Infrastructure.Hosting;
using System;

namespace Milochau.Core.AspNetCore.Infrastructure.Hosting
{
    /// <summary>Configuration registration</summary>
    /// <remarks>
    /// These configuration providers will be used:
    /// <list type="bullet">
    ///    <item>appsettings.{hostName}.json</item>
    ///    <item>Azure Key Vault</item>
    /// </list>
    /// </remarks>
    public static class ConfigurationRegistration
    {
        /// <summary>Add application configuration providers to the configuration builder <paramref name="configurationBuilder"/></summary>
        /// <param name="hostingContextConfiguration">Hosting context configuration</param>
        /// <param name="configurationBuilder">Configuration builder</param>
        public static void AddWebCoreConfiguration(IConfiguration hostingContextConfiguration, IConfigurationBuilder configurationBuilder)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(hostingContextConfiguration);

            // Create specific configuration builder for new configuration sources: the first one will override the next ones
            var internalConfigurationBuilder = new ConfigurationBuilder();

            // Configure appsettings.{hostName}.json
            var hostName = hostOptions.Application.HostName;
            internalConfigurationBuilder.AddJsonFile($"appsettings.{hostName}.json", optional: true, reloadOnChange: false);

            // Add new configuration sources at the beginning of the application configuration builder
            foreach (var configurationSource in internalConfigurationBuilder.Sources)
            {
                // Last configuration provider is environment variables
                // Ante-last configuration provider is secrets.json
                var position = Math.Max(0, configurationBuilder.Sources.Count - 2);
                configurationBuilder.Sources.Insert(position, configurationSource);
            }
        }
    }
}
