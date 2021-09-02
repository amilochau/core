using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.HealthChecks;

namespace Milochau.Core.Functions.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IServiceCollection"/>, specific to HealthChecks</summary>
    internal static class HealthChecksBuilderService
    {
        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        public static IHealthChecksBuilder AddCoreHealthChecks(this IServiceCollection services, CoreHostOptions hostOptions)
        {
            return HealthChecksRegistration.RegisterHealthChecks(services, hostOptions);
        }
    }
}
