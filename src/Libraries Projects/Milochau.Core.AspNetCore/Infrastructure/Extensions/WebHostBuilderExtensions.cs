using Microsoft.AspNetCore.Hosting;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.AspNetCore.Infrastructure.Extensions
{
    /// <summary>Extensions for <see cref="IWebHostBuilder"/></summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>Configures Milochau services, with all the features activated from configuration</summary>
        public static IWebHostBuilder ConfigureCoreWebHostBuilder(this IWebHostBuilder webBuilder)
        {
            return webBuilder
                .ConfigureKestrel(c => c.AddServerHeader = false)
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    ConfigurationRegistration.AddApplicationConfiguration(webHostBuilderContext.Configuration, configurationBuilder);
                });
        }
    }
}
