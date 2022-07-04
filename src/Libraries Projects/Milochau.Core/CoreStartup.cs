using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Cache;
using Milochau.Core.Infrastructure.Hosting;
using System;

namespace Milochau.Core
{
    /// <summary>Base startup class for .NET Core applications</summary>
    public abstract class CoreStartup
    {
        private IConfiguration? configuration;

        private CoreHostOptions? hostOptions;

        /// <summary>Configuration</summary>
        protected IConfiguration Configuration => configuration ?? throw new ArgumentException("Application is not initialized, please call 'SetHostOptions' after creating the Startup class.", nameof(configuration));

        /// <summary>Core host options, see <see cref="CoreHostOptions"/></summary>
        protected CoreHostOptions HostOptions => hostOptions ?? throw new ArgumentException("Application is not initialized, please call 'SetHostOptions' after creating the Startup class.", nameof(configuration));

        /// <summary>Default constructor</summary>
        /// <remarks>If you use this constructor, you MUST call the <see cref="SetHostOptions(IConfiguration)"/> method right after</remarks>
        protected CoreStartup()
        {
        }

        /// <summary>Constructor</summary>
        protected CoreStartup(IConfiguration configuration)
        {
            SetHostOptions(configuration);
        }

        /// <summary>Set host options</summary>
        protected void SetHostOptions(IConfiguration configuration)
        {
            this.configuration = configuration;
            hostOptions = CoreOptionsFactory.GetCoreHostOptions(configuration);
        }

        /// <summary>Configure services</summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            RegisterInfrastructure(services);
        }

        /// <summary>Registers infrastructure dependencies</summary>
        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddSingleton<IApplicationMemoryCache, ApplicationMemoryCache>();
            services.AddOptions<CoreHostOptions>().Configure<IConfiguration>(CoreOptionsFactory.SetupCoreHostOptions);
            services.AddSingleton<IApplicationHostEnvironment>(sp =>
            {
                return new ApplicationHostEnvironment(HostOptions.Application.OrganizationName,
                    HostOptions.Application.ApplicationName,
                    HostOptions.Application.EnvironmentName,
                    HostOptions.Application.HostName,
                    HostOptions.Application.RegionName);
            });
        }
    }
}
