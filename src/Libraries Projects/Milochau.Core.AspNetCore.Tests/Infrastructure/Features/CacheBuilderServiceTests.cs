using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class CacheBuilderServiceTests
    {
        [TestMethod("MapCoreCache - Count")]
        public async Task MapCoreCache_When_CountEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreAsync();

            // When
            var response = await client.GetAsync("/system/cache/local/count");

            // Then
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = JsonSerializer.Deserialize<CountResponse>(await response.Content.ReadAsStringAsync(), options);
            Assert.AreEqual(0, content?.Count);
        }
    }
}
