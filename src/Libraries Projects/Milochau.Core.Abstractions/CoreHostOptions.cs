﻿using Azure.Identity;

namespace Milochau.Core.Abstractions
{
    /// <summary>Core host options</summary>
    public class CoreHostOptions
    {
        /// <summary>Default section in Configuration to retrieved</summary>
        public const string DefaultConfigurationSection = "Core:Host";

        /// <summary>Application options</summary>
        public virtual ApplicationOptions Application { get; set; } = new ApplicationOptions();

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

        /// <summary>Region name</summary>
        public string RegionName { get; set; }
    }

    /// <summary>Azure Key Vault options</summary>
    public class KeyVaultOptions
    {
        /// <summary>Vault URI</summary>
        public string Vault { get; set; }
    }
}
