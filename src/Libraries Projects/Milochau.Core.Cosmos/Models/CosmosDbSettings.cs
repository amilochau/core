using Azure.Core;

namespace Milochau.Core.Cosmos.Models
{
    /// <summary>Cosmos DB settings</summary>
    public class CosmosDbSettings
    {

        /// <summary>Connection string</summary>
        /// <remarks>If no connection string is set, we try to authenticate with <see cref="AccountEndpoint"/> and <see cref="TokenCredential"/></remarks>
        public string? ConnectionString { get; set; }

        /// <summary>Account endpoint</summary>
        public string AccountEndpoint { get; set; } = null!;

        /// <summary>Database name</summary>
        public string DatabaseName { get; set; } = null!;
    }
}
