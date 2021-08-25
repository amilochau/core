using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Cache;
using Moq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public async Task LocalCount_Should_ReturnCount_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/cache/local/count");
            var count = 10;
            applicationMemoryCache.SetupGet(x => x.Count).Returns(count);

            // When
            var httpResponseData = await functions.GetLocalCountAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<CountResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(count, response.Count);
        }

        [TestMethod("Cache - LocalContains")]
        public async Task Invoke_Should_ReturnLocalContains_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/cache/local/contains", new QueryString("?key=test"));

            // When
            var httpResponseData = await functions.GetLocalContainsAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<ContainsResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count);
            Assert.AreEqual("test", response.Keys.First());
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalContains without keys")]
        public async Task Invoke_Should_ReturnLocalContains_When_CalledWithoutKeysAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/cache/local/contains");

            // When
            var httpResponseData = await functions.GetLocalContainsAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsText(httpResponseData, HttpStatusCode.NotFound);
            Assert.AreEqual("Please provide a cache key to test.", response);
        }

        [TestMethod("Cache - LocalContains with empty keys")]
        public async Task Invoke_Should_ReturnLocalContains_When_CalledWithEmptyKeysAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/cache/local/contains", new QueryString("?key="));

            // When
            var httpResponseData = await functions.GetLocalContainsAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<ContainsResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count);
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalCompact")]
        public async Task Invoke_Should_CompactLocal_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("post", "/api/system/cache/local/compact", new QueryString("?percentage=0.2"));

            // When
            var httpResponseData = await functions.CompactLocalAsync(httpRequestData);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(0.2), Times.Once);
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<CompactResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(0.2, response.Percentage);
        }

        [TestMethod("Cache - LocalCompact without percentage")]
        public async Task Invoke_Should_CompactLocal_When_CalledWithoutPercentageAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("post", "/api/system/cache/local/compact");

            // When
            var httpResponseData = await functions.CompactLocalAsync(httpRequestData);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(1), Times.Once);
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<CompactResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Percentage);
        }

        [TestMethod("Cache - LocalRemove")]
        public async Task Invoke_Should_RemoveLocal_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("post", "/api/system/cache/local/remove", new QueryString("?key=test"));

            // When
            var httpResponseData = await functions.RemoveLocalAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<RemoveResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count);
            Assert.AreEqual("test", response.Keys.First());
        }

        [TestMethod("Cache - LocalRemove without keys")]
        public async Task Invoke_Should_RemoveLocal_When_CalledWithoutKeysAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("post", "/api/system/cache/local/remove");

            // When
            var httpResponseData = await functions.RemoveLocalAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsText(httpResponseData, HttpStatusCode.NotFound);
            Assert.AreEqual("Please provide a cache key to remove.", response);
        }

        [TestMethod("Cache - LocalRemove with empty keys")]
        public async Task Invoke_Should_RemoveLocal_When_CalledWithEmptyKeysAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("post", "/api/system/cache/local/remove", new QueryString("?key="));

            // When
            var httpResponseData = await functions.RemoveLocalAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<RemoveResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count);
        }
    }
}
