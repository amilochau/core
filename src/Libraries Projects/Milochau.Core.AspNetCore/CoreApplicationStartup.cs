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
            Configuration.Bind(CoreServicesOptions.DefaultConfigurationSection, servicesOptions);

            services.AddRouting();

            services.AddOptions<CoreServicesOptions>().Configure(settings => Configuration.Bind(CoreServicesOptions.DefaultConfigurationSection, settings));

            services.AddCoreTelemetry(HostOptions, servicesOptions);

            ConfigureHealthChecks(services);
        }

        /// <summary>Configure health checks</summary>
        protected virtual IHealthChecksBuilder ConfigureHealthChecks(IServiceCollection services)
        {
            return services.AddCoreHealthChecks(HostOptions);
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IApplicationBuilder app)
        {
            StartupLogging.LogApplicationInformation(app.ApplicationServices);

            var servicesOptions = app.ApplicationServices.GetRequiredService<IOptions<CoreServicesOptions>>().Value;

            app.UseCoreApplication(HostOptions, servicesOptions);
            app.UseCoreTelemetry(HostOptions, servicesOptions);
        }
    }
}
