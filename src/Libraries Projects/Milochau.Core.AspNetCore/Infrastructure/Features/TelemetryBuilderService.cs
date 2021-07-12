using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.Models;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>, specific to Telemetry</summary>
    internal static class TelemetryBuilderService
    {
        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        /// <remarks>Telemetry uses Application Insights</remarks>
        public static IServiceCollection AddCoreTelemetry(this IServiceCollection services, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            if (servicesOptions.Telemetry.Enabled)
            {
                var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
                {
                    EnableAdaptiveSampling = !servicesOptions.Telemetry.DisableAdaptiveSampling
                };
                services.AddApplicationInsightsTelemetry(aiOptions);
            }

            return services;
        }

        /// <summary>Adds middlewares needed by the features activated from configuration</summary>
        /// <param name="app">Application builder</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        public static IApplicationBuilder UseCoreTelemetry(this IApplicationBuilder app, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            if (servicesOptions.Telemetry.Enabled && !servicesOptions.Telemetry.DisableAdaptiveSampling)
            {
                var telemetryConfiguration = app.ApplicationServices.GetService<TelemetryConfiguration>();
                telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessorChainBuilder
                    .UseAdaptiveSampling(maxTelemetryItemsPerSecond: 5, "Trace;Exception")
                    .Build();
            }

            return app;
        }
    }
}
