using Azure.Identity;

namespace Milochau.Core.Models
{
    /// <summary>Core host options</summary>
    public class CoreHostOptions
    {
        /// <summary>Default section in Configuration to retrieved <see cref="CoreHostOptions"/></summary>
        public const string DefaultConfigurationSection = "Core:Host";

        internal const string DefaultSentinelKey = "Sentinel:Key";
        internal const int DefaultAppConfigRefreshExpirationInMinutes = 120;
        internal const string DefaultAppConfigKey = "Shared";
        internal const string DefaultAppConfigNamespaceSeparator = "/";

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

        /// <summary>Tenant ID</summary>
        /// <remarks><see cref="ClientSecret"/> and <see cref="ClientId"/> must be set to authenticate with <see cref="TenantId"/></remarks>
        public string TenantId { get; set; }

        /// <summary>Client ID</summary>
        /// <remarks><see cref="TenantId"/> and <see cref="ClientSecret"/> must be set to authenticate with <see cref="ClientId"/></remarks>
        public string ClientId { get; set; }

        /// <summary>Client secret</summary>
        /// <remarks><see cref="TenantId"/> and <see cref="ClientId"/> must be set to authenticate with <see cref="ClientId"/></remarks>
        public string ClientSecret { get; set; }
    }

    /// <summary>Azure App configuration options</summary>
    public class AppConfigurationOptions
    {
        /// <summary>App Configuration Endpoint</summary>
        public string Endpoint { get; set; }

        /// <summary>App Configuration Connection string</summary>
        public string ConnectionString { get; set; }

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
