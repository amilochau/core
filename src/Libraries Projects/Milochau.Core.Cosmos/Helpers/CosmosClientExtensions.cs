using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Milochau.Core.Abstractions.Exceptions;
using Milochau.Finance.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Cosmos.Helpers
{
    /// <summary>Extensions for <see cref="CosmosClient"/></summary>
    public static class CosmosClientExtensions
    {
        /// <summary>Exception message when the entity is not found</summary>
        public const string EntityNotFoundExceptionMessage = "Entity is not found.";

        /// <summary>Create a new entity</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public async static Task CreateItemAsync<TItem>(this CosmosClient cosmosClient, string databaseName, string containerName, TItem item, string partitionKey, ILogger logger, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer(databaseName, containerName);
            var response = await container.CreateItemAsync(item, new PartitionKey(partitionKey), null, cancellationToken);

            logger.LogResponse(response, "create");
        }

        /// <summary>Read an entity as a point item</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        /// <exception cref="NotFoundException">Entity has not been found</exception>
        public async static Task<TItem> ReadPointItemAsync<TItem>(this CosmosClient cosmosClient, string databaseName, string containerName, string id, string partitionKey, ILogger logger, CancellationToken cancellationToken)
            where TItem : IEntity<string>
        {
            try
            {
                var container = cosmosClient.GetContainer(databaseName, containerName);
                var response = await container.ReadItemAsync<TItem>(id, new PartitionKey(partitionKey), null, cancellationToken);

                logger.LogResponse(response, "read point");

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotFoundException(EntityNotFoundExceptionMessage);
            }
        }

        /// <summary>Query entities</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public static IOrderedQueryable<TItem> QueryItems<TItem>(this CosmosClient cosmosClient, string databaseName, string containerName, string? partitionKey)
            where TItem : IEntity<string>
        {
            var container = cosmosClient.GetContainer(databaseName, containerName);
            return container.GetItemLinqQueryable<TItem>(false, null, new QueryRequestOptions
            {
                PartitionKey = partitionKey != null ? new PartitionKey(partitionKey) : null,
            });
        }

        /// <summary>Get an entity from a query, or returns null</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public async static Task<TItem?> GetItemOrDefaultAsync<TItem>(this IQueryable<TItem> query, ILogger logger, CancellationToken cancellationToken)
        {
            using var feedIterator = query.ToFeedIterator();

            // Only read one item, so we don't need to loop with the 'feedIterator.HasMoreResults'
            var response = await feedIterator.ReadNextAsync(cancellationToken);

            logger.LogResponse(response, "get or default");

            return response.FirstOrDefault();
        }

        /// <summary>Get a single entity from a query</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        /// <exception cref="NotFoundException">Entity has not been found or is not unique</exception>
        public async static Task<TItem> GetSingleItemAsync<TItem>(this IQueryable<TItem> query, ILogger logger, CancellationToken cancellationToken)
        {
            using var feedIterator = query.ToFeedIterator();

            // Only read one item, so we don't need to loop with the 'feedIterator.HasMoreResults'
            var response = await feedIterator.ReadNextAsync(cancellationToken);

            logger.LogResponse(response, "get single");

            if (response.Count != 1)
            {
                throw new NotFoundException(EntityNotFoundExceptionMessage);
            }

            return response.First();
        }

        /// <summary>List entities from a query</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public async static IAsyncEnumerable<TItem> ListItemsAsync<TItem>(this IQueryable<TItem> query, ILogger logger, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var feedIterator = query.ToFeedIterator();

            // Iterate query result pages
            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);
                logger.LogResponse(response, "list");

                foreach (var item in response)
                {
                    yield return item;
                }
            }
        }

        /// <summary>Patch an entity</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public async static Task PatchItemAsync<TItem>(this CosmosClient cosmosClient, string databaseName, string containerName, string id, string partitionKey, IReadOnlyList<PatchOperation> patchOperations, ILogger logger, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer(databaseName, containerName);
            var response = await container.PatchItemAsync<TItem>(id, new PartitionKey(partitionKey), patchOperations, null, cancellationToken);

            logger.LogResponse(response, "patch");
        }

        /// <summary>Remove an entity</summary>
        /// <typeparam name="TItem">Type of database entity</typeparam>
        public async static Task RemoveItemAsync<TItem>(this CosmosClient cosmosClient, string databaseName, string containerName, string id, string partitionKey, ILogger logger, CancellationToken cancellationToken)
        {
            var container = cosmosClient.GetContainer(databaseName, containerName);
            var response = await container.DeleteItemAsync<TItem>(id, new PartitionKey(partitionKey), null, cancellationToken);

            logger.LogResponse(response, "remove");
        }

        private static void LogResponse<TItem>(this ILogger logger, ItemResponse<TItem> response, string operationType)
        {
            logger.LogInformation($"{typeof(TItem)} - {operationType} - RequestCharge: {response.RequestCharge}");
            logger.LogInformation(response.Diagnostics.ToString());
        }
        private static void LogResponse<TItem>(this ILogger logger, FeedResponse<TItem> response, string operationType)
        {
            logger.LogInformation($"{typeof(TItem)} - {operationType} - RequestCharge: {response.RequestCharge}");
            logger.LogInformation(response.Diagnostics.ToString());
        }
    }
}
