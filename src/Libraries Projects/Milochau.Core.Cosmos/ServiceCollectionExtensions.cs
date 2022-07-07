using Azure.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.Cosmos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milochau.Core.Cosmos
{
    /// <summary>Services registration extensions</summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Add Cosmos DB services</summary>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, Action<CosmosDbSettings> settings)
        {
            var settingsValue = new CosmosDbSettings();
            settings.Invoke(settingsValue);

            services.AddSingleton<CosmosClient>(serviceProvider =>
            {
                var applicationHostEnvironment = serviceProvider.GetRequiredService<IApplicationHostEnvironment>();
                var credential = serviceProvider.GetRequiredService<TokenCredential>();

                var cosmosClientOptions = new CosmosClientOptions
                {
                    ApplicationName = applicationHostEnvironment.ApplicationName,
                    EnableContentResponseOnWrite = false,
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        IgnoreNullValues = true,
                        Indented = false,
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                };

                if (!string.IsNullOrEmpty(settingsValue.ConnectionString))
                {
                    return new CosmosClient(settingsValue.ConnectionString, cosmosClientOptions);
                }
                else
                {
                    return new CosmosClient(settingsValue.AccountEndpoint, credential, cosmosClientOptions);
                }
            });

            return services;
        }
    }
}
