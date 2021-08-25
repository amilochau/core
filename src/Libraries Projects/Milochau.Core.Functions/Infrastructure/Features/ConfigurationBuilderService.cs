using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using System.Linq;

namespace Milochau.Core.Functions.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IServiceCollection"/></summary>    
    public static class ConfigurationBuilderService
    {
        /// <summary>Adds the features activated from configuration</summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        public static IServiceCollection AddCoreConfiguration(this IServiceCollection services, CoreHostOptions hostOptions)
        {
            if (!string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                services.AddAzureAppConfiguration();
                services.AddSingleton<IConfigurationRefresher>(serviceProvider => serviceProvider.GetService<IConfigurationRefresherProvider>().Refreshers.First());
            }

            return services;
        }
    }
}
