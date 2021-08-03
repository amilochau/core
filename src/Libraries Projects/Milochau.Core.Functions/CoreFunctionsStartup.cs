using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Milochau.Core.Infrastructure.Hosting;
using Milochau.Core.Infrastructure.Features.Configuration;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Cache;
using Milochau.Core.Infrastructure.Features.Health;
using System.IO;
using System.Reflection;

namespace Milochau.Core.Functions
{
    /// <summary>Base class for Azure Function applications</summary>
    public abstract class CoreFunctionsStartup : FunctionsStartup
    {
        /// <summary>Configures Azure App Configuration</summary>
        /// <param name="builder"></param>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // On Azure, we need to get where the app is.
            var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
            builder.ConfigurationBuilder.SetBasePath(basePath);

            var hostingContextConfiguration = builder.ConfigurationBuilder.Build();

            StartupConfiguration.RegisterConfiguration(hostingContextConfiguration, builder.ConfigurationBuilder);
        }

        /// <summary>Configures infrastructure and services</summary>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.GetContext().Configuration;

            RegisterInfrastructure(services, configuration);
            ConfigureServices(services, configuration);
        }

        /// <summary>Registers infrastructure dependencies</summary>
        protected virtual void RegisterInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(configuration);

            if (!string.IsNullOrEmpty(hostOptions.AppConfig.Endpoint))
            {
                services.AddSingleton(StartupConfiguration.ConfigurationRefresher);
            }

            services.AddSingleton<IApplicationMemoryCache, ApplicationMemoryCache>();
            services.AddOptions<CoreHostOptions>().Configure<IConfiguration>(CoreOptionsFactory.SetupCoreHostOptions);
            services.AddSingleton<IApplicationHostEnvironment>(sp =>
            {
                return new ApplicationHostEnvironment(hostOptions.Application.ApplicationName, hostOptions.Application.EnvironmentName, hostOptions.Application.HostName);
            });

            RegisterFeatureManagement(services);
            RegisterHealthChecks(services, hostOptions);
        }

        /// <summary>Registers Feature Management</summary>
        /// <remarks>Override this method if you want to add more Feature Filters</remarks>
        protected virtual IFeatureManagementBuilder RegisterFeatureManagement(IServiceCollection services)
        {
            return services.AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<ApplicationFilter>()
                .AddFeatureFilter<EnvironmentFilter>()
                .AddFeatureFilter<HostFilter>();
        }

        /// <summary>Registers health checks</summary>
        /// <remarks>Override this method if you want to add more health checks</remarks>
        protected virtual IHealthChecksBuilder RegisterHealthChecks(IServiceCollection services, CoreHostOptions hostOptions)
        {
            return HealthChecksRegistration.RegisterHealthChecks(services, hostOptions);
        }

        /// <summary>Configures services</summary>
        protected abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
