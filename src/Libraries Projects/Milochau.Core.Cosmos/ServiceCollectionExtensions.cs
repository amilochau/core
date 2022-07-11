using Azure.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Milochau.Core.Cosmos.Models;
using System;

namespace Milochau.Core.Cosmos
{
    /// <summary>Services registration extensions</summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Add Cosmos DB services</summary>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, Action<CosmosDbSettings>? settings = null)
        {
            if (settings != null)
            {
                services.Configure<CosmosDbSettings>(settings);
            }
            else
            {
                services.AddOptions<CosmosDbSettings>()
                    .Configure<IConfiguration>((options, configuration) => configuration.GetSection("Database").Bind(options))
                    .PostConfigure<IOptions<CoreHostOptions>>((options, hostOptions) =>
                    {
                        options.DatabaseName ??= hostOptions.Value.Application.GetInfrastructureConvention(InfrastructureConventionType.CosmosDbDatabaseName);
                        options.AccountEndpoint ??= hostOptions.Value.Application.GetInfrastructureConvention(InfrastructureConventionType.CosmosDbAccountEndpoint);
                    });
            }

            services.AddSingleton<CosmosClient>(serviceProvider =>
            {
                var applicationHostEnvironment = serviceProvider.GetRequiredService<IApplicationHostEnvironment>();
                var credential = serviceProvider.GetRequiredService<TokenCredential>();
                var options = serviceProvider.GetRequiredService<IOptions<CosmosDbSettings>>();

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

                if (!string.IsNullOrEmpty(options.Value.ConnectionString))
                {
                    return new CosmosClient(options.Value.ConnectionString, cosmosClientOptions);
                }
                else
                {
                    return new CosmosClient(options.Value.AccountEndpoint, credential, cosmosClientOptions);
                }
            });

            return services;
        }
    }
}
