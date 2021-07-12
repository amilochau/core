﻿using Microsoft.AspNetCore.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Cache;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    /// <summary>Middleware that exposes a cache management endpoints</summary>
    internal class SystemCacheMiddleware
    {
        private readonly IApplicationMemoryCache applicationMemoryCache;

        private const string compactPercentageQueryKey = "percentage";
        private const double compactPercentageDefaultValue = 1;
        private const string removeKeyQueryKey = "key";
        private const string containsKeyQueryKey = "key";

        /// <summary>Constructor</summary>
        public SystemCacheMiddleware(RequestDelegate _,
            IApplicationMemoryCache applicationMemoryCache)
        {
            this.applicationMemoryCache = applicationMemoryCache;
        }

        /// <summary>Processes a request</summary>
        /// <param name="httpContext">HTTP context</param>
        public Task InvokeAsync(HttpContext httpContext)
        {
            return httpContext.Request.Method switch
            {
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/local/count", StringComparison.OrdinalIgnoreCase) => LocalCountAsync(httpContext),
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/local/contains", StringComparison.OrdinalIgnoreCase) => LocalContainsAsync(httpContext),
                Keys.PostMethod when httpContext.Request.Path.Value.EndsWith("/local/compact", StringComparison.OrdinalIgnoreCase) => LocalCompactAsync(httpContext),
                Keys.PostMethod when httpContext.Request.Path.Value.EndsWith("/local/remove", StringComparison.OrdinalIgnoreCase) => LocalRemoveAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private Task LocalCountAsync(HttpContext httpContext)
        {
            var response = new CountResponse
            { 
                Count = applicationMemoryCache.Count
            };
            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }

        private Task LocalContainsAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Query.TryGetValue(containsKeyQueryKey, out var keys))
            {
                return BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, "Please provide a cache key to test.");
            }

            var filteredKeys = keys.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var response = new ContainsResponse
            {
                Keys = filteredKeys,
                Contains = filteredKeys.Any(key => applicationMemoryCache.Contains(key))
            };
            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }

        private Task LocalCompactAsync(HttpContext httpContext)
        {
            var response = new CompactResponse();
            if (!httpContext.Request.Query.TryGetValue(compactPercentageQueryKey, out var value)
                || !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var percentage))
            {
                percentage = compactPercentageDefaultValue;
            }

            applicationMemoryCache.Compact(percentage);
            response.Percentage = percentage;

            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }

        private Task LocalRemoveAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Query.TryGetValue(removeKeyQueryKey, out var keys))
            {
                return BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, "Please provide a cache key to remove.");
            }

            var filteredKeys = keys.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var response = new RemoveResponse { Keys = filteredKeys };
            foreach (var key in filteredKeys)
            {
                applicationMemoryCache.Remove(key);
            }

            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }
    }
}
