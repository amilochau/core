using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Microsoft.AspNetCore.Routing;
using Milochau.Core.HealthChecks;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Milochau.Core.HealthChecks.Models;

namespace Milochau.Core.Functions.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>, specific to HealthChecks</summary>
    internal static class HealthChecksBuilderService
    {
        private const string defaultContentType = "application/json";
        private static readonly byte[] emptyResponse = new[] { (byte)'{', (byte)'}' };

        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        public static IHealthChecksBuilder AddCoreHealthChecks(this IServiceCollection services, CoreHostOptions hostOptions)
        {
            return HealthChecksRegistration.RegisterHealthChecks(services, hostOptions);
        }

        /// <summary>Adds default health checks endpoint</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreDefaultHealthChecks(this IEndpointRouteBuilder endpoints, string path)
        {
            return endpoints.MapHealthChecks(path, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = WriteHealthChecksResponse
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
                ResponseWriter = WriteHealthChecksResponse
            });
        }

        private static async Task WriteHealthChecksResponse(HttpContext httpContext, HealthReport healthReport)
        {
            if (healthReport != null)
            {
                httpContext.Response.ContentType = defaultContentType;

                var detailedHealthReport = DetailedHealthReport.CreateFrom(healthReport);

                using var responseStream = new MemoryStream();

                await JsonSerializer.SerializeAsync(responseStream, detailedHealthReport, HealthChecksResponseWriter.JsonOptions.Value);
                await httpContext.Response.BodyWriter.WriteAsync(responseStream.ToArray());
            }
            else
            {
                await httpContext.Response.BodyWriter.WriteAsync(emptyResponse);
            }
        }
    }
}
