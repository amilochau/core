using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Abstractions.Models;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        [Function("system-cache-local-count")]
        public async Task<HttpResponseData> GetLocalCountAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/cache/local/count")] HttpRequestData request)
        {
            var countResponse = new CountResponse
            {
                Count = applicationMemoryCache.Count
            };

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(countResponse);
            return response;
        }

        /// <summary>Test if local cache contains items</summary>
        [Function("system-cache-local-contains")]
        public async Task<HttpResponseData> GetLocalContainsAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/cache/local/contains")] HttpRequestData request)
        {
            var containsResponse = new ContainsResponse();

            var keys = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues(containsKeyQueryKey)?.Where(x => !string.IsNullOrEmpty(x));
            if (keys != null && keys.Any())
            {
                containsResponse.Keys = keys;
                containsResponse.Contains = keys.Any(key => applicationMemoryCache.Contains(key));
            }

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(containsResponse);
            return response;
        }

        /// <summary>Compact local cache</summary>
        [Function("system-cache-local-compact")]
        public async Task<HttpResponseData> CompactLocalAsync([HttpTrigger(AuthorizationLevel.Admin, "post", Route = "system/cache/local/compact")] HttpRequestData request)
        {
            var compactResponse = new CompactResponse();
            var value = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues(compactPercentageQueryKey)?.FirstOrDefault();
            if (value == null || !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var percentage))
            {
                percentage = compactPercentageDefaultValue;
            }

            applicationMemoryCache.Compact(percentage);
            compactResponse.Percentage = percentage;

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(compactResponse);
            return response;
        }

        /// <summary>Remove an item from the local cache</summary>
        [Function("system-cache-local-remove")]
        public async Task<HttpResponseData> RemoveLocalAsync([HttpTrigger(AuthorizationLevel.Admin, "post", Route = "system/cache/local/remove")] HttpRequestData request)
        {
            var removeResponse = new RemoveResponse();

            var keys = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues(removeKeyQueryKey)?.Where(x => !string.IsNullOrEmpty(x))?.ToList();
            if (keys != null && keys.Any())
            {
                removeResponse.Keys = keys;
                foreach (var key in keys)
                {
                    applicationMemoryCache.Remove(key);
                }
            }

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(removeResponse);
            return response;
        }
    }
}
