using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Milochau.Core.AspNetCore.Models;
using System.Linq;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>, specific to Application Information</summary>
    internal static class ApplicationBuilderService
    {
        private const string defaultDisplayName = "Application";

        /// <summary>Adds middlewares needed by the features activated from configuration</summary>
        /// <param name="app">Application builder</param>
        /// <param name="hostOptions">Core host options, see <see cref="CoreHostOptions"/></param>
        /// <param name="servicesOptions">Core services options, see <see cref="CoreServicesOptions"/></param>
        public static IApplicationBuilder UseCoreApplication(this IApplicationBuilder app, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            if (servicesOptions.RequestLocalization.Enabled)
            {
                var requestLocalizationOptions = new Microsoft.AspNetCore.Builder.RequestLocalizationOptions();

                var defaultCulture = !string.IsNullOrEmpty(servicesOptions.RequestLocalization.DefaultCulture)
                    ? servicesOptions.RequestLocalization.DefaultCulture
                    : servicesOptions.RequestLocalization.SupportedCultures.FirstOrDefault();

                var supportedCultures = servicesOptions.RequestLocalization.SupportedCultures.Union(new[] { defaultCulture }).ToArray();

                if (!string.IsNullOrEmpty(defaultCulture))
                {
                    requestLocalizationOptions
                        .SetDefaultCulture(defaultCulture)
                        .AddSupportedCultures(supportedCultures)
                        .AddSupportedUICultures(supportedCultures);
                }

                app.UseRequestLocalization(requestLocalizationOptions);
            }

            return app;
        }

        /// <summary>Adds application endpoints</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreApplication(this IEndpointRouteBuilder endpoints, string path)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<SystemApplicationMiddleware>()
                .Build();

            return endpoints.Map(path + "/application/{*sub}", pipeline).WithDisplayName(defaultDisplayName);
        }
    }
}
