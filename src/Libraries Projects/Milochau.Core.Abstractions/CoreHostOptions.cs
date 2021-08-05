using Azure.Identity;

namespace Milochau.Core.Abstractions
{
    /// <summary>Core host options</summary>
    public class CoreHostOptions
    {
        /// <summary>Default section in Configuration to retrieved</summary>
        public const string DefaultConfigurationSection = "Core:Host";

        /// <summary>Default App Configuration sentinel key</summary>
        public const string DefaultSentinelKey = "Sentinel:Key";

        /// <summary>Default App Configuration duration between refreshes</summary>
        public const int DefaultAppConfigRefreshExpirationInMinutes = 120;

        /// <summary>Default namespace for shared configuration in App Configuration</summary>
        public const string DefaultAppConfigKey = "Shared";

        /// <summary>Default separator in App Configuration keys to identity namespace</summary>
        public const string DefaultAppConfigNamespaceSeparator = "/";

        /// <summary>Application options</summary>
        public virtual ApplicationOptions Application { get; set; } = new ApplicationOptions();

        /// <summary>Azure App configuration options</summary>
        public virtual AppConfigurationOptions AppConfig { get; set; } = new AppConfigurationOptions();

        /// <summary>Azure Key Vault options</summary>
        public virtual KeyVaultOptions KeyVault { get; set; } = new KeyVaultOptions();

        /// <summary>Default Azure credential options</summary>
        public virtual DefaultAzureCredentialOptions Credential { get; set; } = new DefaultAzureCredentialOptions();
    }

    /// <summary>Application options</summary>
    public class ApplicationOptions
    {
        /// <summary>Organization name</summary>
        public string OrganizationName { get; set; }

        /// <summary>Application name</summary>
        public string ApplicationName { get; set; }

        /// <summary>Environment name</summary>
        public string EnvironmentName { get; set; }

        /// <summary>Host name</summary>
        public string HostName { get; set; }
    }

    /// <summary>Azure Key Vault options</summary>
    public class KeyVaultOptions
    {
        /// <summary>Vault URI</summary>
        public string Vault { get; set; }
    }

    /// <summary>Azure App configuration options</summary>
    public class AppConfigurationOptions
    {
        /// <summary>App Configuration Endpoint</summary>
        public string Endpoint { get; set; }

        /// <summary>Namespace separator; default is <see cref="CoreHostOptions.DefaultAppConfigNamespaceSeparator"/></summary>
        public string NamespaceSeparator { get; set; } = CoreHostOptions.DefaultAppConfigNamespaceSeparator;

        /// <summary>Sentinel key; default is <see cref="CoreHostOptions.DefaultSentinelKey"/></summary>
        /// <remarks>Azure App Configuration will prefix this <see cref="SentinelKey"/> with the <see cref="CoreHostOptions.DefaultAppConfigKey"/>; by example: `Shared/Sentinel:Key`</remarks>
        public string SentinelKey { get; set; } = CoreHostOptions.DefaultSentinelKey;

        /// <summary>Refresh expiration (minutes); default is <see cref="CoreHostOptions.DefaultAppConfigRefreshExpirationInMinutes"/></summary>
        /// <remarks>This refresh expiration duration is used for Azure App Configuration settings and feature flags</remarks>
        public int RefreshExpirationInMinutes { get; set; } = CoreHostOptions.DefaultAppConfigRefreshExpirationInMinutes;
    }
}
