using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Milochau.Core.Infrastructure.Features.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Middlewares
{
    [TestClass]
    public class SystemConfigurationMiddlewareTests
    {
        private Mock<RequestDelegate> requestDelegate;
        private Mock<IFeatureManager> featureManager;
        private IConfiguration configuration;

        private SystemConfigurationMiddleware middleware;

        private const string featureName = "featureName";

        [TestInitialize]
        public void Initialize()
        {
            requestDelegate = new Mock<RequestDelegate>();
            featureManager = new Mock<IFeatureManager>();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>());
            configuration = configurationBuilder.Build();

            middleware = new SystemConfigurationMiddleware(requestDelegate.Object, featureManager.Object, configuration);
        }

        [TestMethod("Configuration - Flags")]
        public async Task Flags_Should_ReturnFlags_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/configuration/flags");
            featureManager.Setup(x => x.GetFeatureNamesAsync()).Returns(GetTestFeaturesAsync());
            featureManager.Setup(x => x.IsEnabledAsync(featureName)).ReturnsAsync(true);

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<FlagsResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Features.Count);
            Assert.IsNotNull(response.Features.First());
            Assert.IsTrue(response.Features.First().Enabled);
        }

        [TestMethod("Configuration - Providers")]
        public async Task Providers_Should_ReturnProviders_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/configuration/providers");

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<ProvidersResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Providers.Count());
            Assert.AreEqual("MemoryConfigurationProvider", response.Providers.First());
        }

        private static async IAsyncEnumerable<string> GetTestFeaturesAsync()
        {
            yield return featureName;

            await Task.CompletedTask; // to make the compiler warning go away
        }
    }
}
