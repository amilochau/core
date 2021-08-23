using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Milochau.Core.Infrastructure.Features.Cache;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Middlewares
{
    [TestClass]
    public class SystemCacheMiddlewareTests
    {
        private Mock<RequestDelegate> requestDelegate;
        private Mock<IApplicationMemoryCache> applicationMemoryCache;

        private SystemCacheMiddleware middleware;

        [TestInitialize]
        public void Initialize()
        {
            requestDelegate = new Mock<RequestDelegate>();
            applicationMemoryCache = new Mock<IApplicationMemoryCache>();

            middleware = new SystemCacheMiddleware(requestDelegate.Object, applicationMemoryCache.Object);
        }

        [TestMethod("Cache - LocalCount")]
        public async Task InvokeAsync_Should_ReturnLocalCount_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/cache/local/count");
            var count = 10;
            applicationMemoryCache.SetupGet(x => x.Count).Returns(count);

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<CountResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(count, response.Count);
        }

        [TestMethod("Cache - LocalContains")]
        public async Task InvokeAsync_Should_ReturnLocalContains_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/cache/local/contains", new QueryString("?key=test"));

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<ContainsResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count);
            Assert.AreEqual("test", response.Keys.First());
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalContains without keys")]
        public async Task InvokeAsync_Should_ReturnLocalContains_When_CalledWithoutKeysAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/cache/local/contains");

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        }

        [TestMethod("Cache - LocalContains with empty keys")]
        public async Task InvokeAsync_Should_ReturnLocalContains_When_CalledWithEmptyKeysAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/cache/local/contains", new QueryString("?key="));

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<ContainsResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count);
            Assert.IsFalse(response.Contains);
        }

        [TestMethod("Cache - LocalCompact")]
        public async Task InvokeAsync_Should_CompactLocal_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.PostMethod, "/api/system/cache/local/compact", new QueryString("?percentage=0.2"));

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(0.2), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<CompactResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(0.2, response.Percentage);
        }

        [TestMethod("Cache - LocalCompact without percentage")]
        public async Task InvokeAsync_Should_CompactLocal_When_CalledWithoutPercentageAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.PostMethod, "/api/system/cache/local/compact");

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            applicationMemoryCache.Verify(x => x.Compact(1), Times.Once);
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<CompactResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Percentage);
        }

        [TestMethod("Cache - LocalRemove")]
        public async Task InvokeAsync_Should_RemoveLocal_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.PostMethod, "/api/system/cache/local/remove", new QueryString("?key=test"));

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<RemoveResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(1, response.Keys.Count());
            Assert.AreEqual("test", response.Keys.First());
        }

        [TestMethod("Cache - LocalRemove without keys")]
        public async Task InvokeAsync_Should_RemoveLocal_When_CalledWithoutKeysAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.PostMethod, "/api/system/cache/local/remove");

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        }

        [TestMethod("Cache - LocalRemove with empty keys")]
        public async Task InvokeAsync_Should_RemoveLocal_When_CalledWithEmptyKeysAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.PostMethod, "/api/system/cache/local/remove", new QueryString("?key="));

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<RemoveResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Keys);
            Assert.AreEqual(0, response.Keys.Count());
        }
    }
}
