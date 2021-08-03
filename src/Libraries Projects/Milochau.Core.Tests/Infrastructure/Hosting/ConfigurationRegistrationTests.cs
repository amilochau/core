using Milochau.Core.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Milochau.Core.Tests.Infrastructure.Hosting
{
    [TestClass]
    public class ConfigurationRegistrationTests
    {
        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        [TestMethod]
        public void AddApplicationConfiguration_Where_AppConfigEndpointIsSet()
        {
            // Given
            var hostingConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
            }).Build();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ASPNETCORE_APPCONFIG_ENDPOINT", "https://" }
            });

            // When
            ConfigurationRegistration.AddApplicationConfiguration(hostingConfiguration, configurationBuilder);

            // Then
        }

        [TestMethod]
        public void AddApplicationConfiguration_Where_AppConfigConnectionStringIsSet()
        {
            // Given
            var hostingConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
            }).Build();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ASPNETCORE_APPCONFIG_CONNECTIONSTRING", "Endpoint=https://" }
            });

            // When
            ConfigurationRegistration.AddApplicationConfiguration(hostingConfiguration, configurationBuilder);

            // Then
        }
    }
}
