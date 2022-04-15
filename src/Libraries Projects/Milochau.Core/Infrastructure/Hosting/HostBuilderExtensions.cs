using Microsoft.Extensions.Configuration.Json;
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

                    // Configure appsettings files
                    configurationBuilder.Sources.Insert(0, new JsonConfigurationSource { Path = $"appsettings.{hostName}.json", Optional = true, ReloadOnChange = false });
                    configurationBuilder.Sources.Insert(0, new JsonConfigurationSource { Path = $"appsettings.{environmentName}.json", Optional = true, ReloadOnChange = false });
                    configurationBuilder.Sources.Insert(0, new JsonConfigurationSource { Path = $"appsettings.json", Optional = true, ReloadOnChange = false });
                });
        }
    }
}
