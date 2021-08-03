using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Models;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Threading.Tasks;
using System.Net;
using Milochau.Core.Infrastructure.Features.Configuration;
using System.Linq;
using System.Text.Json;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class ConfigurationBuilderServiceTests
    {
        private IServiceCollection services;

        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private readonly CoreServicesOptions coreServicesOptions = new CoreServicesOptions();

        [TestInitialize]
        public void Initialize()
        {
            services = BaseFeatureBuilderServiceTest.CreateServiceCollection();
        }

        [TestMethod("UseCoreConfiguration - No AppConfig defined")]
        public void UseCoreConfiguration_When_NoAppConfigIsDefined()
        {
            // Given
            var applicationBuilder = BaseFeatureBuilderServiceTest.CreateApplicationBuilder(services);
            coreHostOptions.AppConfig.Endpoint = "";

            // When
            ConfigurationBuilderService.UseCoreConfiguration(applicationBuilder, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(applicationBuilder);
            // Impossible to check middlewares registration with ASP.NET Core - see _components private field in ApplicationBuilder
        }

        [TestMethod("UseCoreConfiguration - AppConfig defined")]
        public void UseCoreConfiguration_When_AppConfigIsDefined()
        {
            // Given
            services.AddScoped(_ => Mock.Of<IConfigurationRefresherProvider>());
            var applicationBuilder = BaseFeatureBuilderServiceTest.CreateApplicationBuilder(services);
            coreHostOptions.AppConfig.Endpoint = "http://";

            // When
            ConfigurationBuilderService.UseCoreConfiguration(applicationBuilder, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(applicationBuilder);
            // Impossible to check middlewares registration with ASP.NET Core - see _components private field in ApplicationBuilder
        }

        [TestMethod("MapCoreConfiguration - Flags")]
        public async Task Flags_Should_ReturnFlags_When_CalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/system/configuration/flags");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [TestMethod("MapCoreConfiguration - Providers")]
        public async Task Providers_Should_ReturnProviders_When_CalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/system/configuration/providers");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<ProvidersResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(2, content.Providers.Count());
            Assert.AreEqual("JsonConfigurationProvider for 'appsettings.local.json' (Optional)", content.Providers.ElementAt(0));
            Assert.AreEqual("Microsoft.Extensions.Configuration.ChainedConfigurationProvider", content.Providers.ElementAt(1));
        }
    }
}
