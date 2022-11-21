using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.Functions.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
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

        /// <summary>Count local cache information</summary>
        [Function("system-cache-local")]
        public async Task<HttpResponseData> GetLocalCacheInformationAsync([HttpTrigger(AuthorizationLevel.Admin, "get, post", Route = "system/cache/local/{type}")] HttpRequestData request, string type, CancellationToken cancellationToken)
        {
            return request.Method switch
            {
                Keys.GetMethod when type.Equals("count", StringComparison.OrdinalIgnoreCase) => await GetLocalCountAsync(request, cancellationToken),
                Keys.GetMethod when type.Equals("contains", StringComparison.OrdinalIgnoreCase) => await GetLocalContainsAsync(request, cancellationToken),
                Keys.PostMethod when type.Equals("compact", StringComparison.OrdinalIgnoreCase) => await CompactLocalAsync(request, cancellationToken),
                Keys.PostMethod when type.Equals("remove", StringComparison.OrdinalIgnoreCase) => await RemoveLocalAsync(request, cancellationToken),
                _ => request.WriteEmptyResponse(HttpStatusCode.NotFound),
            };
        }

        /// <summary>Count local cache items</summary>
        internal async Task<HttpResponseData> GetLocalCountAsync(HttpRequestData request, CancellationToken cancellationToken)
        {
            var countResponse = new CountResponse
            {
                Count = applicationMemoryCache.Count
            };

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(countResponse, cancellationToken);
            return response;
        }

        /// <summary>Test if local cache contains items</summary>
        internal async Task<HttpResponseData> GetLocalContainsAsync(HttpRequestData request, CancellationToken cancellationToken)
        {
            var containsResponse = new ContainsResponse();

            var keys = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues(containsKeyQueryKey)?.Where(x => !string.IsNullOrEmpty(x));
            if (keys != null && keys.Any())
            {
                containsResponse.Keys = keys;
                containsResponse.Contains = keys.Any(key => applicationMemoryCache.Contains(key));
            }

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(containsResponse, cancellationToken);
            return response;
        }

        /// <summary>Compact local cache</summary>
        internal async Task<HttpResponseData> CompactLocalAsync(HttpRequestData request, CancellationToken cancellationToken)
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
            await response.WriteAsJsonAsync(compactResponse, cancellationToken);
            return response;
        }

        /// <summary>Remove an item from the local cache</summary>
        internal async Task<HttpResponseData> RemoveLocalAsync(HttpRequestData request, CancellationToken cancellationToken)
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
            await response.WriteAsJsonAsync(removeResponse, cancellationToken);
            return response;
        }
    }
}
