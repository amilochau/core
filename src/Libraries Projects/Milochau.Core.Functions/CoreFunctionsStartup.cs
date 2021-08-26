using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Functions.Infrastructure.Features;
using Milochau.Core.Infrastructure.Hosting;
using System;

namespace Milochau.Core.Functions
{
    /// <summary>Base startup class for Azure Function applications</summary>
    public abstract class CoreFunctionsStartup : CoreStartup
    {
        internal static TStartup Create<TStartup>(IConfiguration configuration)
            where TStartup : CoreFunctionsStartup, new()
        {
            var startup = new TStartup();
            startup.SetHostOptions(configuration);
            return startup;
        }

        /// <summary>Configure services</summary>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddCoreConfiguration(hostOptions);

            ConfigureHealthChecks(services);
        }

        /// <summary>Configure health checks</summary>
        protected virtual IHealthChecksBuilder ConfigureHealthChecks(IServiceCollection services)
        {
            return services.AddCoreHealthChecks(hostOptions);
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IServiceProvider serviceProvider, IFunctionsWorkerApplicationBuilder functionsWorkerApplicationBuilder)
        {
            StartupLogging.LogApplicationInformation(serviceProvider);
        }
    }
}
