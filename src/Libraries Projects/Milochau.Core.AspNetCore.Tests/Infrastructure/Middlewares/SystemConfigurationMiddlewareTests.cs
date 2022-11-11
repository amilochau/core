using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Middlewares
{
    [TestClass]
    public class SystemConfigurationMiddlewareTests
    {
        private Mock<RequestDelegate> requestDelegate = null!;
        private IConfiguration configuration = null!;

        private SystemConfigurationMiddleware middleware = null!;

        private const string featureName = "featureName";

        [TestInitialize]
        public void Initialize()
        {
            requestDelegate = new Mock<RequestDelegate>();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>());
            configuration = configurationBuilder.Build();

            middleware = new SystemConfigurationMiddleware(requestDelegate.Object, configuration);
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
