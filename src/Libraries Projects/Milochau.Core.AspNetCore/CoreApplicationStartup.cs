using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.AspNetCore
{
    /// <summary>Base startup class for ASP.NET Core applications</summary>
    public abstract class CoreApplicationStartup : CoreStartup
    {
        /// <summary>Constructor</summary>
        protected CoreApplicationStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>Configure services</summary>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var servicesOptions = new CoreServicesOptions();
            configuration.Bind(CoreServicesOptions.DefaultConfigurationSection, servicesOptions);

            services.AddRouting();

            services.AddOptions<CoreServicesOptions>().Configure(settings => configuration.Bind(CoreServicesOptions.DefaultConfigurationSection, settings));

            services.AddCoreConfiguration(hostOptions, servicesOptions);
            services.AddCoreTelemetry(hostOptions, servicesOptions);

            ConfigureHealthChecks(services);
        }

        /// <summary>Configure health checks</summary>
        protected virtual IHealthChecksBuilder ConfigureHealthChecks(IServiceCollection services)
        {
            return services.AddCoreHealthChecks(hostOptions);
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IApplicationBuilder app)
        {
            StartupLogging.LogApplicationInformation(app.ApplicationServices);

            var servicesOptions = app.ApplicationServices.GetService<IOptions<CoreServicesOptions>>().Value;

            app.UseCoreApplication(hostOptions, servicesOptions);
            app.UseCoreConfiguration(hostOptions, servicesOptions);
            app.UseCoreTelemetry(hostOptions, servicesOptions);
        }
    }
}
