using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Milochau.Core.Abstractions;

namespace Milochau.Core.HealthChecks
{
    /// <summary>Health checks registration</summary>
    public static class HealthChecksRegistration
    {
        /// <summary>Tag for light checks</summary>
        public const string LightTag = "light";

        /// <summary>Register health checks into <paramref name="services"/></summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Host options, see <see cref="CoreHostOptions"/></param>
        public static IHealthChecksBuilder RegisterHealthChecks(IServiceCollection services, CoreHostOptions hostOptions)
        {
            IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

            // Add default endpoint health check
            healthChecksBuilder.AddCheck("Endpoint", () => HealthCheckResult.Healthy(), new[] { LightTag });

            // Add Application Host Environment health check
            healthChecksBuilder.AddCheck("Application Host Environment", () => {
                return string.IsNullOrWhiteSpace(hostOptions.Application.OrganizationName)
                    || string.IsNullOrWhiteSpace(hostOptions.Application.ApplicationName)
                    || string.IsNullOrWhiteSpace(hostOptions.Application.EnvironmentName)
                    || string.IsNullOrWhiteSpace(hostOptions.Application.HostName)
                    || string.IsNullOrWhiteSpace(hostOptions.Application.RegionName)
                    ? HealthCheckResult.Degraded("Application Host Environment is partially set. You should add app settings for Organization, Application, Environment, Host and Region.")
                    : HealthCheckResult.Healthy();
            }, new[] { LightTag });

            return healthChecksBuilder;
        }
    }
}
