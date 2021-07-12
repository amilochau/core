using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.Models;
using Microsoft.AspNetCore.Routing;
using Milochau.Core.Infrastructure.Features.Health;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>, specific to HealthChecks</summary>
    internal static class HealthChecksBuilderService
    {
        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        public static IServiceCollection AddCoreHealthChecks(this IServiceCollection services, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            HealthChecksRegistration.RegisterHealthChecks(services, hostOptions);
            return services;
        }

        /// <summary>Adds default health checks endpoint</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreDefaultHealthChecks(this IEndpointRouteBuilder endpoints, string path)
        {
            return endpoints.MapHealthChecks(path, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }

        /// <summary>Adds light health checks endpoint</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreLightHealthChecks(this IEndpointRouteBuilder endpoints, string path)
        {
            return endpoints.MapHealthChecks($"{path}/{HealthChecksRegistration.LightTag}", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthChecksRegistration.LightTag),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
