using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Models;
using Microsoft.Extensions.Options;

namespace Milochau.Core.AspNetCore.Infrastructure.Extensions
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/></summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>Adds middlewares needed by all the features activated from configuration</summary>
        /// <param name="app">Application builder</param>
        public static IApplicationBuilder UseCoreFeatures(this IApplicationBuilder app)
        {
            var hostOptions = app.ApplicationServices.GetService<IOptions<CoreHostOptions>>();
            var servicesOptions = app.ApplicationServices.GetService<IOptions<CoreServicesOptions>>();

            return UseCoreFeaturesInternal(app, hostOptions.Value, servicesOptions.Value);
        }

        private static IApplicationBuilder UseCoreFeaturesInternal(IApplicationBuilder app, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            app = app
                .UseCoreApplication(hostOptions, servicesOptions)
                .UseCoreConfiguration(hostOptions, servicesOptions)
                .UseCoreTelemetry(hostOptions, servicesOptions);

            return app;
        }
    }
}
