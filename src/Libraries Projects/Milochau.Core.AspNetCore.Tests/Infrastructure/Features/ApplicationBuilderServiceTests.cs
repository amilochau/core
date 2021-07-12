using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Milochau.Core.Infrastructure.Features.Application;
using Milochau.Core.Models;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class ApplicationBuilderServiceTests
    {
        private IServiceCollection services;

        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private readonly CoreServicesOptions coreServicesOptions = new CoreServicesOptions();

        [TestInitialize]
        public void Initialize()
        {
            services = BaseFeatureBuilderServiceTest.CreateServiceCollection();
        }

        [TestMethod("MapCoreApplication - Assembly")]
        public async Task MapCoreApplication_When_AssemblyEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/system/application/assembly");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<AssemblyResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual("Microsoft Corporation", content.Company);
            Assert.AreEqual("© Microsoft Corporation. All rights reserved.", content.Copyright);
            Assert.AreEqual("Microsoft.TestHost", content.Product);
            Assert.IsFalse(content.IsLocal);
        }

        [TestMethod("MapCoreApplication - Environment")]
        public async Task MapCoreApplication_When_EnvironmentEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/system/application/environment");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [TestMethod("UseCoreApplication - Request localization enabled")]
        public void UseCoreApplication_When_RequestLocalizationIsEnabled()
        {
            // Given
            var applicationBuilder = BaseFeatureBuilderServiceTest.CreateApplicationBuilder(services);
            coreServicesOptions.RequestLocalization.Enabled = true;
            coreServicesOptions.RequestLocalization.DefaultCulture = "fr-FR";
            coreServicesOptions.RequestLocalization.SupportedCultures = new[] { "fr" };

            // When
            ApplicationBuilderService.UseCoreApplication(applicationBuilder, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(applicationBuilder);
            // Impossible to check middlewares registration with ASP.NET Core - see _components private field in ApplicationBuilder
        }
    }
}
