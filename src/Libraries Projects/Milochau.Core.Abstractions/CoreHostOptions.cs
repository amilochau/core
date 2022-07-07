using Azure.Identity;
using System;

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
        public string OrganizationName { get; set; } = null!;

        /// <summary>Application name</summary>
        public string ApplicationName { get; set; } = null!;

        /// <summary>Environment name</summary>
        public string EnvironmentName { get; set; } = null!;

        /// <summary>Host name</summary>
        public string HostName { get; set; } = null!;

        /// <summary>Region name</summary>
        public string RegionName { get; set; } = null!;

        /// <summary>Get the infrastructure convention for a defined type</summary>
        public string GetInfrastructureConvention(InfrastructureConventionType infrastructureConventionType, string suffix = "")
        {
            return infrastructureConventionType switch
            {
                InfrastructureConventionType.StorageAccountUri => $"https://{OrganizationName}{ApplicationName}{HostName}sto{suffix}.blob.core.windows.net/",
                InfrastructureConventionType.CosmosDbDatabaseName => $"{OrganizationName}-{ApplicationName}-{HostName}-cosmosdb",
                InfrastructureConventionType.CosmosDbAccountEndpoint => $"https://{OrganizationName}-{ApplicationName}-{HostName}-cosmos.documents.azure.com:443/",
                _ => throw new NotSupportedException("Infrastructure convention type not supported!"),
            };
        }
    }
    
    /// <summary>Azure Key Vault options</summary>
    public class KeyVaultOptions
    {
        /// <summary>Vault URI</summary>
        public string? Vault { get; set; }
    }

    /// <summary>Type of infrastructure convention</summary>
    public enum InfrastructureConventionType
    {
        /// <summary>Storage account URI</summary>
        StorageAccountUri = 1,

        /// <summary>Cosmos DB database name</summary>
        CosmosDbDatabaseName = 2,

        /// <summary>Cosmos DB account endpoint</summary>
        CosmosDbAccountEndpoint = 3
    }
}
