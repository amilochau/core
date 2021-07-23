using Milochau.Core.Abstractions;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Azure App Configuration registration</summary>
    public static class AppConfigurationRegistration
    {
        /// <summary>Configures Azure App Configuration</summary>
        /// <param name="appConfigOptions">Azure App Configuration options</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        public static void ConfigureAzureAppConfiguration(AzureAppConfigurationOptions appConfigOptions, CoreHostOptions hostOptions)
        {
            ConnectAzureAppConfiguration(appConfigOptions, hostOptions);
            ConfigureKeyLabels(appConfigOptions, hostOptions);
            ConfigureRefresh(appConfigOptions, hostOptions);
            ConfigureFeatureFlags(appConfigOptions, hostOptions);
        }

        /// <summary>Connect to Azure App Configuration</summary>
        public static void ConnectAzureAppConfiguration(AzureAppConfigurationOptions appConfigOptions, CoreHostOptions hostOptions)
        {
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.ConnectionString))
            {
                appConfigOptions.Connect(hostOptions.AppConfig.ConnectionString);
            }
            else if (!string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                var credential = new DefaultAzureCredential(hostOptions.Credential);
                appConfigOptions.Connect(new Uri(hostOptions.AppConfig.Endpoint), credential);
            }
        }

        /// <summary>Configure key labels</summary>
        public static void ConfigureKeyLabels(AzureAppConfigurationOptions appConfigOptions, CoreHostOptions hostOptions)
        {
            var applicationName = hostOptions.Application.ApplicationName;
            var environmentName = hostOptions.Application.EnvironmentName;
            var hostName = hostOptions.Application.HostName;
            var namespaceSeparator = hostOptions.AppConfig.NamespaceSeparator;

            appConfigOptions
                .Select($"{CoreHostOptions.DefaultAppConfigKey}*", LabelFilter.Null)
                .Select($"{applicationName}*", LabelFilter.Null);

            if (!string.IsNullOrEmpty(environmentName))
            {
                appConfigOptions
                    .Select($"{CoreHostOptions.DefaultAppConfigKey}*", environmentName)
                    .Select($"{applicationName}*", environmentName);
            }

            if (!string.IsNullOrEmpty(hostName))
            {
                appConfigOptions
                    .Select($"{CoreHostOptions.DefaultAppConfigKey}*", hostName)
                    .Select($"{applicationName}*", hostName);
            }

            appConfigOptions.TrimKeyPrefix($"{CoreHostOptions.DefaultAppConfigKey}{namespaceSeparator}");
            appConfigOptions.TrimKeyPrefix($"{applicationName}{namespaceSeparator}");
        }

        /// <summary>Configure refresh</summary>
        public static void ConfigureRefresh(AzureAppConfigurationOptions appConfigOptions, CoreHostOptions hostOptions)
        {
            appConfigOptions.ConfigureRefresh(refreshOptions =>
            {
                if (!string.IsNullOrEmpty(hostOptions.AppConfig.SentinelKey))
                {
                    refreshOptions.Register($"{CoreHostOptions.DefaultAppConfigKey}/{hostOptions.AppConfig.SentinelKey}", refreshAll: true);
                }
                if (hostOptions.AppConfig.RefreshExpirationInMinutes > 0)
                {
                    refreshOptions.SetCacheExpiration(TimeSpan.FromMinutes(hostOptions.AppConfig.RefreshExpirationInMinutes));
                }
            });
        }

        /// <summary>Configure feature flags</summary>
        public static void ConfigureFeatureFlags(AzureAppConfigurationOptions appConfigOptions, CoreHostOptions hostOptions)
        {
            appConfigOptions.UseFeatureFlags(featureFlagOptions =>
            {
                if (hostOptions.AppConfig.RefreshExpirationInMinutes > 0)
                {
                    featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(hostOptions.AppConfig.RefreshExpirationInMinutes);
                }
            });
        }
    }
}
