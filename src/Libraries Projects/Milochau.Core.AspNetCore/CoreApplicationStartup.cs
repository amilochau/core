using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;
using Milochau.Core.Infrastructure.Features.Cache;
using Milochau.Core.Infrastructure.Features.Configuration;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.AspNetCore
{
    /// <summary>Base class for ASP.NET Core applications</summary>
    public abstract class CoreApplicationStartup
    {
        /// <summary>Configuration</summary>
        protected readonly IConfiguration configuration;

        /// <summary>Core host options, see <see cref="CoreHostOptions"/></summary>
        protected readonly CoreHostOptions hostOptions;

        /// <summary>Constructor</summary>
        public CoreApplicationStartup(IConfiguration configuration)
        {
            this.configuration = configuration;
            hostOptions = CoreOptionsFactory.GetCoreHostOptions(configuration);
        }

        /// <summary>Configure services</summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Configure Milochau.Core features
            services.AddCoreFeatures(configuration);

            // Register infrastructure
            RegisterInfrastructure(services);
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use Milochau.Core features
            app.UseCoreFeatures();
        }

        /// <summary>Registers infrastructure dependencies</summary>
        protected virtual void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddRouting();

            services.AddSingleton<IApplicationMemoryCache, ApplicationMemoryCache>();
            services.AddOptions<CoreHostOptions>().Configure<IConfiguration>(CoreOptionsFactory.SetupCoreHostOptions);
            services.AddSingleton<IApplicationHostEnvironment>(sp =>
            {
                return new ApplicationHostEnvironment(hostOptions.Application.ApplicationName, hostOptions.Application.EnvironmentName, hostOptions.Application.HostName);
            });

            RegisterFeatureManagement(services);
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
    }
}
