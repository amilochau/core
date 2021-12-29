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
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class HealthChecksBuilderServiceTests
    {
        private IServiceCollection services;
        private IConfiguration configuration;

        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private const string endpointCheckName = "Endpoint";
        private const string applicationHostEnvironmentCheckName = "Application Host Environment";
        private const string keyVaultServiceName = "Key Vault";

        [TestInitialize]
        public void Initialize()
        {
            services = BaseFeatureBuilderServiceTest.CreateServiceCollection();

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ASPNETCORE_ORGANIZATION", "org" },
                    { "ASPNETCORE_APPLICATION", "app" },
                    { "ASPNETCORE_ENVIRONMENT", "Development" },
                    { "ASPNETCORE_HOST", "dev" },
                    { "ASPNETCORE_REGION", "en1" }
                }).Build();

            coreHostOptions.Application.OrganizationName = "org";
            coreHostOptions.Application.ApplicationName = "app";
            coreHostOptions.Application.EnvironmentName = "Development";
            coreHostOptions.Application.HostName = "dev";
            coreHostOptions.Application.RegionName = "en1";
        }

        [TestMethod("AddCoreHealthChecks - Empty configuration")]
        public void AddCoreHealthChecks_When_NoConfigurationIsDefined()
        {
            // Given

            // When
            HealthChecksBuilderService.AddCoreHealthChecks(services, new CoreHostOptions());

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
            Assert.AreEqual(applicationHostEnvironmentCheckName, secondRegistration.Name);
        }

        [TestMethod("AddCoreHealthChecks - Standard configuration")]
        public void AddCoreHealthChecks_When_HealthChecksOptionsIsDefined()
        {
            // Given

            // When
            HealthChecksBuilderService.AddCoreHealthChecks(services, coreHostOptions);

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
            Assert.AreEqual(applicationHostEnvironmentCheckName, secondRegistration.Name);
        }

        [TestMethod("AddCoreHealthChecks - Key Vault defined from vault")]
        public void AddCoreHealthChecks_When_KeyVaultIsDefinedFromVault()
        {
            // Given
            var vault = "https://xxxx.vault.azure.net";
            coreHostOptions.KeyVault.Vault = vault;

            // When
            HealthChecksBuilderService.AddCoreHealthChecks(services, coreHostOptions);

            // Then
            Assert.IsNotNull(services);
            var serviceProvider = services.BuildServiceProvider();
            Assert.IsNotNull(serviceProvider.GetService<HealthCheckService>());

            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            Assert.IsNotNull(options.Value);
            Assert.IsNotNull(options.Value.Registrations);
            Assert.AreEqual(3, options.Value.Registrations.Count);
            var firstRegistration = options.Value.Registrations.ElementAt(0);
            Assert.IsNotNull(firstRegistration);
            Assert.AreEqual(endpointCheckName, firstRegistration.Name);
            var secondRegistration = options.Value.Registrations.ElementAt(1);
            Assert.IsNotNull(secondRegistration);
            Assert.AreEqual(applicationHostEnvironmentCheckName, secondRegistration.Name);
            var thirdRegistration = options.Value.Registrations.ElementAt(2);
            Assert.IsNotNull(thirdRegistration);
            Assert.AreEqual(keyVaultServiceName, thirdRegistration.Name);
        }

        [TestMethod("MapCoreHealthChecks - Default health checks")]
        public async Task MapCoreHealthChecks_When_DefaultHealthChecksEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

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
            Assert.AreEqual(2, content.Entries.Count);
            Assert.AreEqual("Endpoint", content.Entries.First().Key);
        }

        [TestMethod("MapCoreHealthChecks - Light health checks")]
        public async Task MapCoreHealthChecks_When_LightHealthChecksEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

            // When
            var response = await client.GetAsync("/health/light");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new TimeSpanConverter());
            options.Converters.Add(new JsonStringEnumConverter());
            var content = JsonSerializer.Deserialize<DetailedHealthReport>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(DetailedHealthStatus.Healthy, content.Status);
            Assert.AreEqual(2, content.Entries.Count);
            Assert.AreEqual("Endpoint", content.Entries.First().Key);
        }
    }
}
