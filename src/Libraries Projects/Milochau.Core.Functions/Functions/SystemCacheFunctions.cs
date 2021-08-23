using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Cache;
using System.Globalization;
using System.Linq;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>System Functions to expose cache management</summary>
    public class SystemCacheFunctions
    {
        private readonly IApplicationMemoryCache applicationMemoryCache;

        private const string compactPercentageQueryKey = "percentage";
        private const double compactPercentageDefaultValue = 1;
        private const string removeKeyQueryKey = "key";
        private const string containsKeyQueryKey = "key";

        /// <summary>Constructor</summary>
        public SystemCacheFunctions(IApplicationMemoryCache applicationMemoryCache)
        {
            this.applicationMemoryCache = applicationMemoryCache;
        }

        /// <summary>Count local cache items</summary>
        [Function("System-Cache-LocalCount")]
        public IActionResult LocalCount([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/cache/local/count")] HttpRequest request)
        {
            var response = new CountResponse
            {
                Count = applicationMemoryCache.Count
            };
            return new OkObjectResult(response);
        }

        /// <summary>Test if local cache contains items</summary>
        [Function("System-Cache-LocalContains")]
        public IActionResult LocalContains([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/cache/local/contains")] HttpRequest request)
        {
            if (!request.Query.TryGetValue(containsKeyQueryKey, out var keys))
            {
                return new NotFoundObjectResult("Please provide a cache key to test.");
            }

            var filteredKeys = keys.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var response = new ContainsResponse
            {
                Keys = filteredKeys,
                Contains = filteredKeys.Any(key => applicationMemoryCache.Contains(key))
            };
            return new OkObjectResult(response);
        }

        /// <summary>Compact local cache</summary>
        [Function("System-Cache-LocalCompact")]
        public IActionResult LocalCompact([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "system/cache/local/compact")] HttpRequest request)
        {
            var response = new CompactResponse();
            if (!request.Query.TryGetValue(compactPercentageQueryKey, out var value)
                || !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var percentage))
            {
                percentage = compactPercentageDefaultValue;
            }

            applicationMemoryCache.Compact(percentage);
            response.Percentage = percentage;

            return new OkObjectResult(response);
        }

        /// <summary>Remove an item from the local cache</summary>
        [Function("System-Cache-LocalRemove")]
        public IActionResult LocalRemove([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "system/cache/local/remove")] HttpRequest request)
        {
            if (!request.Query.TryGetValue(removeKeyQueryKey, out var keys))
            {
                return new NotFoundObjectResult("Please provide a cache key to remove.");
            }

            var filteredKeys = keys.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var response = new RemoveResponse { Keys = filteredKeys };
            foreach (var key in filteredKeys)
            {
                applicationMemoryCache.Remove(key);
            }
            return new OkObjectResult(response);
        }
    }
}
