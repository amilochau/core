using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Configuration registration</summary>
    /// <remarks>
    /// These configuration providers will be used:
    /// <list type="bullet">
    ///    <item>JSON file appsettings.{host}.json</item>
    /// </list>
    /// </remarks>
    public static class ConfigurationRegistration
    {
        /// <summary>Add application configuration providers to the configuration builder <paramref name="configurationBuilder"/></summary>
        /// <param name="hostingContextConfiguration">Hosting context configuration</param>
        /// <param name="configurationBuilder">Configuration builder</param>
        public static void AddCoreConfiguration(IConfiguration hostingContextConfiguration, IConfigurationBuilder configurationBuilder)
        {
            var hostOptions = CoreOptionsFactory.GetCoreHostOptions(hostingContextConfiguration);

            // Configure appsettings.{host}.json
            configurationBuilder.Sources.Insert(0, new JsonConfigurationSource
            {
                Path = $"appsettings.{hostOptions.Application.HostName}.json",
                Optional = true,
                ReloadOnChange = false,
            });
        }
    }
}
