using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Text.Json;
using Milochau.Core.Abstractions.Models;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class ConfigurationBuilderServiceTests
    {
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
            Assert.AreEqual(1, content.Providers.Count());
            Assert.AreEqual("Microsoft.Extensions.Configuration.ChainedConfigurationProvider", content.Providers.ElementAt(0));
        }
    }
}
