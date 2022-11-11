using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Milochau.Core.Functions.Tests
{
    [TestClass]
    public class CoreFunctionsStartupTests
    {
        [TestMethod]
        public void ConfigureServices_Should_RegisterAllServices_When_Called()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "AZURE_FUNCTIONS_KEYVAULT_VAULT", "https://xxx.vault.azure.net" }
            });

            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton(Mock.Of<IHostEnvironment>());
            serviceCollection.AddLogging();

            var startup = CoreFunctionsStartup.Create<Startup>(configuration);

            // Act
            startup.ConfigureServices(serviceCollection);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IOptions<CoreHostOptions>>());
            Assert.IsNotNull(serviceProvider.GetService<IApplicationHostEnvironment>());
            Assert.IsNotNull(serviceProvider.GetService<HealthCheckService>());
        }

        public class Startup : CoreFunctionsStartup
        {
        }
    }
}
