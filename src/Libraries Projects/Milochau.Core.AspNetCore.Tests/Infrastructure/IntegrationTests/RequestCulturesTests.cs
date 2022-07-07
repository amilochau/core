using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.IntegrationTests
{
    [TestClass]
    public class RequestCulturesTests
    {
        [TestMethod("Culture - Use default culture")]
        [DataRow("fr-FR")]
        [DataRow("fr")]
        [DataRow("en-US")]
        [DataRow("de-AT")]
        public async Task ReturnDefaultCulture_When_EnvironmentEndpointCalledAsync(string defaultCulture)
        {
            // Given
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Core:Services:RequestLocalization:Enabled", "true" },
                    { "Core:Services:RequestLocalization:DefaultCulture", defaultCulture }
                }).Build();

            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

            // When
            var response = await client.GetAsync("/system/application/environment");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<EnvironmentResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(defaultCulture, content?.CurrentCulture);
        }

        [TestMethod("Culture - Use default culture")]
        public async Task ReturnDefaultCulture_When_DefaultCultureIsDefinedAsync()
        {
            // Given
            var defaultCulture = "fr-FR";

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Core:Services:RequestLocalization:Enabled", "true" },
                    { "Core:Services:RequestLocalization:DefaultCulture", defaultCulture },
                    { "Core:Services:RequestLocalization:SupportedCultures:0", "en-US" }
                }).Build();

            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

            // When
            var response = await client.GetAsync("/system/application/environment");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<EnvironmentResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(defaultCulture, content?.CurrentCulture);
        }

        [TestMethod("Culture - Multiple supported cultures - Query")]
        public async Task ReturnDefaultCulture_When_AnotherSupportedCultureIsDefinedFromQueryAsync()
        {
            // Given
            var defaultCulture = "fr-FR";
            var secondCulture = "en-US";

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Core:Services:RequestLocalization:Enabled", "true" },
                    { "Core:Services:RequestLocalization:DefaultCulture", defaultCulture },
                    { "Core:Services:RequestLocalization:SupportedCultures:0", secondCulture }
                }).Build();

            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

            // When
            var response = await client.GetAsync($"/system/application/environment?culture={secondCulture}");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<EnvironmentResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(secondCulture, content?.CurrentCulture);
        }

        [TestMethod("Culture - Multiple supported cultures - HTTP header")]
        public async Task ReturnDefaultCulture_When_AnotherSupportedCultureIsDefinedFromHttpHeaderAsync()
        {
            // Given
            var defaultCulture = "fr-FR";
            var secondCulture = "en-US";

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Core:Services:RequestLocalization:Enabled", "true" },
                    { "Core:Services:RequestLocalization:DefaultCulture", defaultCulture },
                    { "Core:Services:RequestLocalization:SupportedCultures:0", secondCulture }
                }).Build();

            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync(configuration);

            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(secondCulture);

            // When
            var response = await client.GetAsync("/system/application/environment");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<EnvironmentResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(secondCulture, content?.CurrentCulture);
        }
    }
}
