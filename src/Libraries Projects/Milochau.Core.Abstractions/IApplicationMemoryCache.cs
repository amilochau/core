using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Milochau.Core.Abstractions
{
    /// <summary>Application memory cache</summary>
    /// <remarks>
    /// A memory cache let you save execution time by spending memory. Please use it carefully.
    /// Memory cache may not be a good idea for Azure Functions applications, depending on hosting choices.
    /// </remarks>
    public interface IApplicationMemoryCache
    {
        /// <summary>Get an item from the cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns>Item from cache or factory</returns>
        TItem? GetOrCreate<TItem>(string key, Func<TItem> factory, TimeSpan duration);

        /// <summary>Get an item from the cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns>Item from cache or factory</returns>
        TItem? GetOrCreate<TItem>(string key, Func<TItem> factory, TimeSpan duration, CacheItemPriority priority);

        /// <summary>Get an item from the cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns>Item from cache or factory</returns>
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, TimeSpan duration);

        /// <summary>Get an item from the cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns>Item from cache or factory</returns>
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, TimeSpan duration, CacheItemPriority priority);

        /// <summary>Get the existance state of an item</summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if the item exists in cache</returns>
        bool Contains(string key);

        /// <summary>Set an item into the cache</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="item">Item to add</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns>The item</returns>
        TItem Set<TItem>(string key, TItem item, TimeSpan duration);

        /// <summary>Set an item into the cache</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="item">Item to add</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns>The item</returns>
        TItem Set<TItem>(string key, TItem item, TimeSpan duration, CacheItemPriority priority);

        /// <summary>Remove an item from the cache</summary>
        /// <param name="key">Cache key</param>
        void Remove(string key);

        /// <summary>Compact cache</summary>
        /// <param name="percentage">Size percentage to remove from cache</param>
        void Compact(double percentage);

        /// <summary>Get the count of the current entries for diagnostic purposes</summary>
        int Count { get; }
    }
}
