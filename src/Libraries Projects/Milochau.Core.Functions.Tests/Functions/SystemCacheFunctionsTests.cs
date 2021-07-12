using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Cache;
using Moq;
using System.Linq;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class SystemCacheFunctionsTests : BaseFunctionsTests
    {
        private Mock<IApplicationMemoryCache> applicationMemoryCache;

        private SystemCacheFunctions functions;

        [TestInitialize]
        public void Initialize()
        {
            applicationMemoryCache = new Mock<IApplicationMemoryCache>();

            functions = new SystemCacheFunctions(applicationMemoryCache.Object);
        }
        
        [TestMethod("Cache - LocalCount")]
        public void LocalCount_Should_ReturnCount_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/cache/local/count");
            var count = 10;
            applicationMemoryCache.SetupGet(x => x.Count).Returns(count);

            // When
            var result = functions.LocalCount(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<CountResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(count, response.Count);
        }

        [TestMethod("Cache - LocalContains")]
        public void Invoke_Should_ReturnLocalContains_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/cache/local/contains", new QueryString("?key=test"));

            // When
            var result = functions.LocalContains(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<ContainsResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count);
            Assert.AreEqual("test", response.Keys.First());
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalContains without keys")]
        public void Invoke_Should_ReturnLocalContains_When_CalledWithoutKeys()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/cache/local/contains");

            // When
            var result = functions.LocalContains(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            GetResponseFromActionResult<string>(result, StatusCodes.Status404NotFound);
        }

        [TestMethod("Cache - LocalContains with empty keys")]
        public void Invoke_Should_ReturnLocalContains_When_CalledWithEmptyKeys()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/cache/local/contains", new QueryString("?key="));

            // When
            var result = functions.LocalContains(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<ContainsResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count);
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalCompact")]
        public void Invoke_Should_CompactLocal_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("post", "/api/system/cache/local/compact", new QueryString("?percentage=0.2"));

            // When
            var result = functions.LocalCompact(httpContext.Request);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(0.2), Times.Once);
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<CompactResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(0.2, response.Percentage);
        }

        [TestMethod("Cache - LocalCompact without percentage")]
        public void Invoke_Should_CompactLocal_When_CalledWithoutPercentage()
        {
            // Given
            var httpContext = CreateHttpContext("post", "/api/system/cache/local/compact");

            // When
            var result = functions.LocalCompact(httpContext.Request);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(1), Times.Once);
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<CompactResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Percentage);
        }

        [TestMethod("Cache - LocalRemove")]
        public void Invoke_Should_RemoveLocal_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("post", "/api/system/cache/local/remove", new QueryString("?key=test"));

            // When
            var result = functions.LocalRemove(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<RemoveResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count);
            Assert.AreEqual("test", response.Keys.First());
        }

        [TestMethod("Cache - LocalRemove without keys")]
        public void Invoke_Should_RemoveLocal_When_CalledWithoutKeys()
        {
            // Given
            var httpContext = CreateHttpContext("post", "/api/system/cache/local/remove");

            // When
            var result = functions.LocalRemove(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            GetResponseFromActionResult<string>(result, StatusCodes.Status404NotFound);
        }

        [TestMethod("Cache - LocalRemove with empty keys")]
        public void Invoke_Should_RemoveLocal_When_CalledWithEmptyKeys()
        {
            // Given
            var httpContext = CreateHttpContext("post", "/api/system/cache/local/remove", new QueryString("?key="));

            // When
            var result = functions.LocalRemove(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<RemoveResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count);
        }
    }
}
