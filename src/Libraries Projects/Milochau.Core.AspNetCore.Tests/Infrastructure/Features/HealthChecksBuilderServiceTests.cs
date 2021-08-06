using Milochau.Core.AspNetCore.Models;
using Milochau.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Milochau.Core.Infrastructure.Converters;
using System.Text.Json.Serialization;
using Milochau.Core.HealthChecks.Models;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class HealthChecksBuilderServiceTests
    {
        private IServiceCollection services;

        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private readonly CoreServicesOptions coreServicesOptions = new CoreServicesOptions();
        private const string keyVaultServiceName = "Key Vault";
        private const string endpointCheckName = "Endpoint";

        [TestInitialize]
        public void Initialize()
        {
            services = BaseFeatureBuilderServiceTest.CreateServiceCollection();
        }

        [TestMethod("AddCoreHealthChecks - Empty configuration")]
        public void AddCoreHealthChecks_When_HealthChecksOptionsIsDefined()
        {
            // Given

            // When
            HealthChecksBuilderService.AddCoreHealthChecks(services, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(services);
            var serviceProvider = services.BuildServiceProvider();
            Assert.IsNotNull(serviceProvider.GetService<HealthCheckService>());

            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            Assert.IsNotNull(options.Value);
            Assert.IsNotNull(options.Value.Registrations);
            Assert.AreEqual(1, options.Value.Registrations.Count);
            var firstRegistration = options.Value.Registrations.ElementAt(0);
            Assert.IsNotNull(firstRegistration);
            Assert.AreEqual(endpointCheckName, firstRegistration.Name);
        }

        [TestMethod("AddCoreHealthChecks - Key Vault defined from vault")]
        public void AddCoreHealthChecks_When_KeyVaultIsDefinedFromVault()
        {
            // Given
            var vault = "https://xxxx.vault.azure.net";
            coreHostOptions.KeyVault.Vault = vault;

            // When
            HealthChecksBuilderService.AddCoreHealthChecks(services, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(services);
            var serviceProvider = services.BuildServiceProvider();
            Assert.IsNotNull(serviceProvider.GetService<HealthCheckService>());

            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            Assert.IsNotNull(options.Value);
            Assert.IsNotNull(options.Value.Registrations);
            Assert.AreEqual(2, options.Value.Registrations.Count);
            var firstRegistration = options.Value.Registrations.ElementAt(0);
            Assert.IsNotNull(firstRegistration);
            Assert.AreEqual(endpointCheckName, firstRegistration.Name);
            var secondRegistration = options.Value.Registrations.ElementAt(1);
            Assert.IsNotNull(secondRegistration);
            Assert.AreEqual(keyVaultServiceName, secondRegistration.Name);
        }

        [TestMethod("MapCoreHealthChecks - Default health checks")]
        public async Task MapCoreHealthChecks_When_DefaultHealthChecksEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/health");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new TimeSpanConverter());
            options.Converters.Add(new JsonStringEnumConverter());
            var content = JsonSerializer.Deserialize<DetailedHealthReport>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(DetailedHealthStatus.Healthy, content.Status);
            Assert.AreEqual(1, content.Entries.Count);
            Assert.AreEqual("Endpoint", content.Entries.First().Key);
        }

        [TestMethod("MapCoreHealthChecks - Light health checks")]
        public async Task MapCoreHealthChecks_When_LightHealthChecksEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/health/light");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new TimeSpanConverter());
            options.Converters.Add(new JsonStringEnumConverter());
            var content = JsonSerializer.Deserialize<DetailedHealthReport>(await response.Content.ReadAsStringAsync(), options); Assert.AreEqual(DetailedHealthStatus.Healthy, content.Status);
            Assert.AreEqual(1, content.Entries.Count);
            Assert.AreEqual("Endpoint", content.Entries.First().Key);
        }
    }
}
