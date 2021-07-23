using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using System;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Methods to create core options with fallback values</summary>
    public static class CoreOptionsFactory
    {
        private const string hostingPrefix = "ASPNETCORE_";
        private const string applicationNameKey = "APPLICATION";
        private const string environmentNameKey = "ENVIRONMENT";
        private const string hostNameKey = "HOST";
        private const string keyVaultVaultKey = "KEYVAULT_VAULT";
        private const string appConfigEndpointKey = "APPCONFIG_ENDPOINT";
        private const string appConfigConnectionStringKey = "APPCONFIG_CONNECTIONSTRING";

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

            hostOptions.Application.ApplicationName = hostOptions.Application.ApplicationName ?? configuration[$"{hostingPrefix}{applicationNameKey}"] ?? configuration[applicationNameKey];
            hostOptions.Application.EnvironmentName = hostOptions.Application.EnvironmentName ?? configuration[$"{hostingPrefix}{environmentNameKey}"] ?? configuration[environmentNameKey];
            hostOptions.Application.HostName = hostOptions.Application.HostName ?? configuration[$"{hostingPrefix}{hostNameKey}"] ?? configuration[hostNameKey];

            hostOptions.KeyVault.Vault = hostOptions.KeyVault.Vault ?? configuration[$"{hostingPrefix}{keyVaultVaultKey}"] ?? configuration[keyVaultVaultKey];

            hostOptions.AppConfig.Endpoint = hostOptions.AppConfig.Endpoint ?? configuration[$"{hostingPrefix}{appConfigEndpointKey}"] ?? configuration[appConfigEndpointKey];
            hostOptions.AppConfig.ConnectionString = hostOptions.AppConfig.ConnectionString ?? configuration[$"{hostingPrefix}{appConfigConnectionStringKey}"] ?? configuration[appConfigConnectionStringKey];
        }

        /// <summary>Configuration key prefix for hosting configuration</summary>
        public static string HostingPrefix => hostingPrefix;

        /// <summary>Gets current host from environment</summary>
        /// <remarks>This method only gets host from environment variables. It shall only be used in application initialization.</remarks>
        public static string GetCurrentHostFromEnvironment()
        {
            return Environment.GetEnvironmentVariable($"{hostingPrefix}{hostNameKey}") ?? Environment.GetEnvironmentVariable(hostNameKey) ?? ApplicationHostEnvironment.LocalHostName;
        }
    }
}
