﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures host configuration</summary>
        /// <param name="hostBuilder">Host builder</param>
        public static IHostBuilder ConfigureCoreHostBuilder(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    var environmentName = CoreOptionsFactory.GetCurrentEnvironmentFromEnvironmentVariables();
                    var hostName = CoreOptionsFactory.GetCurrentHostFromEnvironmentVariables();

                    configurationBuilder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{hostName}.json", optional: true, reloadOnChange: false);
                })
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    ConfigurationRegistration.AddCoreConfiguration(webHostBuilderContext.Configuration, configurationBuilder);
                });
        }
    }
}
