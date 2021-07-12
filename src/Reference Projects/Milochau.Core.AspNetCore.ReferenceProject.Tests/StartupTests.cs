using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Core.AspNetCore.ReferenceProject.Tests
{
    [TestClass]
    public class StartupTests
    {
        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        [TestMethod]
        public void ConfigureServices_When_FeatureFlagsIsEnabled()
        {
            // Given
            var serviceCollection = new ServiceCollection();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Core:Host:AppConfig:FeatureFlags:Enabled", "true" },
                { "Core:Services:Telemetry:Enabled", "true" }
            });
            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton(configuration);

            var startup = new Startup(configuration);

            // When
            startup.ConfigureServices(serviceCollection);

            // Then
            // --- Check Feature Flags
            Assert.IsTrue(serviceCollection.Any(x => x.ServiceType.Name == "IFeatureManager"));

            // --- Check Health Checks
            Assert.IsTrue(serviceCollection.Any(x => x.ServiceType.Name == "HealthCheckService"));

            // --- Check Telemetry
            Assert.IsTrue(serviceCollection.Any(x => x.ServiceType.Name == "ITelemetryModule"));
        }
    }
}
