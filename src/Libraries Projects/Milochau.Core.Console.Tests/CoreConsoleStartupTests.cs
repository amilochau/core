using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Milochau.Core.Console;

namespace Milochau.Core.Functions.Tests
{
    [TestClass]
    public class CoreConsoleStartupTests
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

            var startup = CoreConsoleStartup.Create<Startup>(configuration);

            // Act
            startup.ConfigureServices(serviceCollection);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IOptions<CoreHostOptions>>());
            Assert.IsNotNull(serviceProvider.GetService<IApplicationHostEnvironment>());
        }

        public class Startup : CoreConsoleStartup
        {
        }
    }
}
