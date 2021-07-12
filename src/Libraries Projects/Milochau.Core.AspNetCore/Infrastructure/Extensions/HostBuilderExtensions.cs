using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Infrastructure.Hosting;
using System;

namespace Milochau.Core.AspNetCore.Infrastructure.Extensions
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures core host defaults, using <see cref="WebHostBuilderExtensions.ConfigureCoreWebHostBuilder"/> and setting up <typeparamref name="TStartup"/> class</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <param name="hostBuilder">Host builder</param>
        public static IHostBuilder ConfigureCoreHostBuilder<TStartup>(this IHostBuilder hostBuilder)
            where TStartup : CoreApplicationStartup
        {
            return hostBuilder
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    var hostName = CoreOptionsFactory.GetCurrentHostFromEnvironment();

                    configurationBuilder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    if (string.Equals(hostName, ApplicationHostEnvironment.LocalHostName, StringComparison.OrdinalIgnoreCase))
                    {
                        configurationBuilder.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureCoreWebHostBuilder()
                        .UseStartup<TStartup>();
                });
        }
    }
}
