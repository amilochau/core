using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    /// <summary>Middleware that exposes a configuration endpoints</summary>
    internal class SystemConfigurationMiddleware
    {
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public SystemConfigurationMiddleware(RequestDelegate _,
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>Processes a request</summary>
        /// <param name="httpContext">HTTP context</param>
        public Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value ?? string.Empty;

            return httpContext.Request.Method switch
            {
                Keys.GetMethod when path.EndsWith("/providers", StringComparison.OrdinalIgnoreCase) => ProvidersAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private async Task ProvidersAsync(HttpContext httpContext)
        {
            var response = new ProvidersResponse();

            var configurationRoot = configuration as ConfigurationRoot;
            if (configurationRoot != null)
            {
                response.Providers = configurationRoot.Providers.Select(x => x.ToString()).Where(x => !string.IsNullOrWhiteSpace(x));
            }

            await BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }
    }
}
