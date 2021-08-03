using Milochau.Core.Abstractions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
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
        public void Configure_Should_RegisterAllServices_When_Called()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ASPNETCORE_APPCONFIG_ENDPOINT", "https://" },
                { "ASPNETCORE_KEYVAULT_VAULT", "https://xxx.vault.azure.net" }
            });

            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton(Mock.Of<IHostEnvironment>());
            StartupConfiguration.ConfigurationRefresher = Mock.Of<IConfigurationRefresher>();

            var functionsConfigurationBuilder = new Mock<IFunctionsConfigurationBuilder>();
            var functionsHostBuilder = new Mock<IFunctionsHostBuilder>();

            functionsConfigurationBuilder.SetupGet(x => x.ConfigurationBuilder).Returns(configurationBuilder);

            var startup = new Startup(serviceCollection, configuration);

            // Act
            startup.ConfigureAppConfiguration(functionsConfigurationBuilder.Object);
            startup.Configure(functionsHostBuilder.Object);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IOptions<CoreHostOptions>>());
            Assert.IsNotNull(serviceProvider.GetService<IApplicationHostEnvironment>());
            Assert.IsNotNull(serviceProvider.GetService<IFeatureManager>());
            Assert.IsNotNull(serviceProvider.GetService<HealthCheckService>());
        }

        public class Startup : CoreFunctionsStartup
        {
            private readonly IServiceCollection services;
            private readonly IConfiguration configuration;

            public Startup(IServiceCollection services, IConfiguration configuration)
            {
                this.services = services;
                this.configuration = configuration;
            }

            public override void Configure(IFunctionsHostBuilder builder)
            {
                RegisterInfrastructure(services, configuration);
            }

            protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {
            }
        }
    }
}
