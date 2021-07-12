using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Milochau.Core.Infrastructure.Features.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    /// <summary>Middleware that exposes a configuration endpoints</summary>
    internal class SystemConfigurationMiddleware
    {
        private readonly IFeatureManager featureManager;
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public SystemConfigurationMiddleware(RequestDelegate _,
            IFeatureManager featureManager,
            IConfiguration configuration)
        {
            this.featureManager = featureManager;
            this.configuration = configuration;
        }

        /// <summary>Processes a request</summary>
        /// <param name="httpContext">HTTP context</param>
        public Task InvokeAsync(HttpContext httpContext)
        {
            return httpContext.Request.Method switch
            {
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/flags", StringComparison.OrdinalIgnoreCase) => FlagsAsync(httpContext),
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/providers", StringComparison.OrdinalIgnoreCase) => ProvidersAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private async Task FlagsAsync(HttpContext httpContext)
        {
            var response = new FlagsResponse();
            await foreach (var featureName in featureManager.GetFeatureNamesAsync())
            {
                var enabled = await featureManager.IsEnabledAsync(featureName);
                response.Features.Add(new FeatureDetails { Name = featureName, Enabled = enabled });
            }

            await BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
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
