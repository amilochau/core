﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Cache;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core
{
    /// <summary>Base startup class for .NET Core applications</summary>
    public abstract class CoreStartup
    {
        /// <summary>Configuration</summary>
        protected IConfiguration configuration;

        /// <summary>Core host options, see <see cref="CoreHostOptions"/></summary>
        protected CoreHostOptions hostOptions;

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
                return new ApplicationHostEnvironment(hostOptions.Application.OrganizationName,
                    hostOptions.Application.ApplicationName,
                    hostOptions.Application.EnvironmentName,
                    hostOptions.Application.HostName,
                    hostOptions.Application.RegionName);
            });
        }
    }
}
