using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;

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

            // Configure Milochau.Core features
            services.AddRouting();
            services.AddCoreFeatures(configuration);
        }

        /// <summary>Configure health checks</summary>
        protected virtual IHealthChecksBuilder ConfigureHealthChecks(IServiceCollection services)
        {
            return services.AddHealthChecks();
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IApplicationBuilder app)
        {
            // Use Milochau.Core features
            app.UseCoreFeatures();
        }
    }
}
