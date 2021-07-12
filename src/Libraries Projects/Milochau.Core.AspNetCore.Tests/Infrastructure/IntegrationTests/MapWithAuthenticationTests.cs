using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Net;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Authentication
{
    [TestClass]
    public class MapWithAuthenticationTests
    {
        [TestMethod("MapCore - With authorization")]
        public async Task MapCoreCache_When_CountEndpointCalledAsync()
        {
            // Given
            using var client = await BaseEndpointsTests.CreateHttpClientFromCoreWithAuthenticationAsync();

            // When
            var response = await client.GetAsync("/system/application/assembly");

            // Then
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
