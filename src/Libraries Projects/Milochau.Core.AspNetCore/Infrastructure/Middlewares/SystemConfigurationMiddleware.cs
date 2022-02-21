using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions.Models;
using System;
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
            return httpContext.Request.Method switch
            {
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/providers", StringComparison.OrdinalIgnoreCase) => ProvidersAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private async Task ProvidersAsync(HttpContext httpContext)
        {
            var configurationRoot = configuration as ConfigurationRoot;
            var response = new ProvidersResponse
            {
                Providers = configurationRoot.Providers.Select(x => x.ToString())
            };

            await BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }
    }
}
