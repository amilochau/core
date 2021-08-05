using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using System;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Methods to create core options with fallback values</summary>
    public static class CoreOptionsFactory
    {
        /// <summary>Configuration prefix for generic host</summary>
        public const string GenericHostConfigurationPrefix = "DOTNET_";

        /// <summary>Configuration prefix for web host</summary>
        public const string WebHostConfigurationPrefix = "ASPNETCORE_";

        private const string applicationNameKey = "APPLICATION";
        private const string environmentNameKey = "ENVIRONMENT";
        private const string hostNameKey = "HOST";
        private const string keyVaultVaultKey = "KEYVAULT_VAULT";
        private const string appConfigEndpointKey = "APPCONFIG_ENDPOINT";

        /// <summary>Gets a new <see cref="CoreHostOptions"/> and setup fallback values</summary>
        /// <param name="configuration">Configuration</param>
        /// <returns>Core host options, see <see cref="CoreHostOptions"/></returns>
        public static CoreHostOptions GetCoreHostOptions(IConfiguration configuration)
        {
            var hostOptions = new CoreHostOptions();
            SetupCoreHostOptions(hostOptions, configuration);
            return hostOptions;
        }

        /// <summary>Setups <paramref name="hostOptions"/> with fallback values</summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        public static void SetupCoreHostOptions(CoreHostOptions hostOptions, IConfiguration configuration)
        {
            configuration.Bind(CoreHostOptions.DefaultConfigurationSection, hostOptions);

            hostOptions.Application.ApplicationName = hostOptions.Application.ApplicationName
                ?? GetValueFromConfiguration(configuration, applicationNameKey);
            hostOptions.Application.EnvironmentName = hostOptions.Application.EnvironmentName
                ?? GetValueFromConfiguration(configuration, environmentNameKey)
                ?? ApplicationHostEnvironment.DevelopmentEnvironmentName;
            hostOptions.Application.HostName = hostOptions.Application.HostName
                ?? GetValueFromConfiguration(configuration, hostNameKey)
                ?? ApplicationHostEnvironment.LocalHostName;

            hostOptions.KeyVault.Vault = hostOptions.KeyVault.Vault
                ?? GetValueFromConfiguration(configuration, keyVaultVaultKey);

            hostOptions.AppConfig.Endpoint = hostOptions.AppConfig.Endpoint
                ?? GetValueFromConfiguration(configuration, appConfigEndpointKey);
        }

        /// <summary>Gets current environment name from environment variables</summary>
        /// <remarks>This method only gets environment name from environment variables. It shall only be used in application initialization.</remarks>
        public static string GetCurrentEnvironmentFromEnvironmentVariables()
        {
            return GetValueFromEnvironment(environmentNameKey)
                ?? ApplicationHostEnvironment.DevelopmentEnvironmentName;
        }

        /// <summary>Gets current host name from environment</summary>
        /// <remarks>This method only gets host name from environment variables. It shall only be used in application initialization.</remarks>
        public static string GetCurrentHostFromEnvironmentVariables()
        {
            return GetValueFromEnvironment(hostNameKey)
                ?? ApplicationHostEnvironment.LocalHostName;
        }

        private static string GetValueFromConfiguration(IConfiguration configuration, string suffix)
        {
            return configuration[$"{GenericHostConfigurationPrefix}{suffix}"]
                ?? configuration[$"{WebHostConfigurationPrefix}{suffix}"]
                ?? configuration[suffix];
        }

        private static string GetValueFromEnvironment(string suffix)
        {
            return Environment.GetEnvironmentVariable($"{GenericHostConfigurationPrefix}{suffix}")
                ?? Environment.GetEnvironmentVariable($"{WebHostConfigurationPrefix}{suffix}")
                ?? Environment.GetEnvironmentVariable(suffix);
        }
    }
}
