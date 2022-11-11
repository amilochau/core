using Milochau.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Milochau.Core.Infrastructure.Features.Cache
{
    /// <summary>Application memory cache</summary>
    public class ApplicationMemoryCache : MemoryCache, IApplicationMemoryCache
    {
        /// <summary>Constructor</summary>
        /// <param name="optionsAccessor">Options accessor</param>
        public ApplicationMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        /// <summary>Get an item from the memory cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns></returns>
        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory, TimeSpan duration)
            => GetOrCreate(key, factory, duration, CacheItemPriority.Normal);

        /// <summary>Get an item from the memory cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns></returns>
        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory, TimeSpan duration, CacheItemPriority priority)
        {
            return this.GetOrCreate(key, cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(duration);
                cacheEntry.SetPriority(priority);
                return factory();
            });
        }

        /// <summary>Get an item from the memory cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns>Item from cache or factory</returns>
        public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, TimeSpan duration)
            => GetOrCreateAsync(key, factory, duration, CacheItemPriority.Normal);

        /// <summary>Get an item from the memory cache, or create a new one from the defined factory</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Item factory</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns>Item from cache or factory</returns>
        public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, TimeSpan duration, CacheItemPriority priority)
        {
            return this.GetOrCreateAsync(key, async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(duration);
                cacheEntry.SetPriority(priority);
                return await factory();
            });
        }

        /// <summary>Get the existance state of an item</summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if the item exists in cache</returns>
        public bool Contains(string key)
        {
            return TryGetValue(key, out _);
        }

        /// <summary>Set an item into the cache</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="item">Item to add</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <returns>The item</returns>
        public TItem Set<TItem>(string key, TItem item, TimeSpan duration)
            => Set(key, item, duration, CacheItemPriority.Normal);

        /// <summary>Set an item into the cache</summary>
        /// <typeparam name="TItem">Type of item stored in cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="item">Item to add</param>
        /// <param name="duration">Cache duration (absolute expiration, relative from now)</param>
        /// <param name="priority">Cache item priority</param>
        /// <returns>The item</returns>
        public TItem Set<TItem>(string key, TItem item, TimeSpan duration, CacheItemPriority priority)
        {
            return this.Set(key, item, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration,
                Priority = priority
            });
        }

        /// <summary>Remove an item from the cache</summary>
        /// <param name="key">Cache key</param>
        public void Remove(string key)
        {
            base.Remove(key);
        }
    }
}
