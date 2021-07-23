using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Models;
using System;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Hosting;
using Milochau.Core.AspNetCore.Infrastructure.Features;

namespace Milochau.Core.AspNetCore.Infrastructure.Extensions
{
    /// <summary>Extensions for <see cref="IServiceCollection"/></summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds all the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration to use to retrieve options</param>
        /// <remarks>
        /// Core host options (<see cref="CoreHostOptions"/>) are retrieved from <see cref="CoreHostOptions.DefaultConfigurationSection"/> configuration section.
        /// Core services options (<see cref="CoreServicesOptions"/>) are retrieved from <see cref="CoreServicesOptions.DefaultConfigurationSection"/> configuration section.
        /// </remarks>
        public static IServiceCollection AddCoreFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} should not be null.");

            return AddCoreFeatures(services,
                hostOptionsAction: hostOptions => CoreOptionsFactory.SetupCoreHostOptions(hostOptions, configuration),
                servicesOptionsAction: servicesOptions => configuration.Bind(CoreServicesOptions.DefaultConfigurationSection, servicesOptions));
        }

        /// <summary>Adds all the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptionsAction">Action to setup core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptionsAction">Action to setup core services options, see <see cref="CoreServicesOptions"/></param>
        public static IServiceCollection AddCoreFeatures(this IServiceCollection services, Action<CoreHostOptions> hostOptionsAction, Action<CoreServicesOptions> servicesOptionsAction)
        {
            var hostOptions = new CoreHostOptions();
            if (hostOptionsAction != null)
                hostOptionsAction.Invoke(hostOptions);

            var servicesOptions = new CoreServicesOptions();
            if (servicesOptionsAction != null)
                servicesOptionsAction.Invoke(servicesOptions);

            services.AddOptions<CoreServicesOptions>().Configure(servicesOptionsAction);

            return AddCoreFeaturesInternal(services, hostOptions, servicesOptions);
        }

        private static IServiceCollection AddCoreFeaturesInternal(IServiceCollection services, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            return services
                .AddCoreConfiguration(hostOptions, servicesOptions)
                .AddCoreHealthChecks(hostOptions, servicesOptions)
                .AddCoreTelemetry(hostOptions, servicesOptions);
        }
    }
}
