using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Routing;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/></summary>    
    internal static class ConfigurationBuilderService
    {
        private const string defaultDisplayName = "Configuration";

        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        public static IServiceCollection AddCoreConfiguration(this IServiceCollection services, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.ConnectionString) || !string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                services.AddAzureAppConfiguration();
            }

            return services;
        }

        /// <summary>Adds middlewares needed by the features activated from configuration</summary>
        /// <param name="app">Application builder</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        public static IApplicationBuilder UseCoreConfiguration(this IApplicationBuilder app, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.ConnectionString) || !string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                app.UseAzureAppConfiguration();
            }

            return app;
        }

        /// <summary>Adds configuration endpoints</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreConfiguration(this IEndpointRouteBuilder endpoints, string path)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
               .UseMiddleware<SystemConfigurationMiddleware>()
               .Build();

            return endpoints.Map(path + "/configuration/{*sub}", pipeline).WithDisplayName(defaultDisplayName);
        }
    }
}
