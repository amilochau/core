using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Milochau.Core.Infrastructure.Features.Application;
using Moq;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Middlewares
{
    [TestClass]
    public class SystemApplicationMiddlewareTests
    {
        private Mock<RequestDelegate> requestDelegate;
        private Mock<IApplicationHostEnvironment> applicationHostEnvironment;

        private SystemApplicationMiddleware middleware;

        private const string applicationName = "applicationName";
        private const string hostName = "hostName";
        private const string environmentName = "environmentName";

        [TestInitialize]
        public void Initialize()
        {
            requestDelegate = new Mock<RequestDelegate>();
            applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();

            middleware = new SystemApplicationMiddleware(requestDelegate.Object, applicationHostEnvironment.Object);
        }

        [TestMethod("Application - Assembly")]
        public async Task InvokeAsync_Should_ReturnAssembly_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/application/assembly");

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<AssemblyResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual("Microsoft Corporation", response.Company);
            Assert.AreEqual("© Microsoft Corporation. All rights reserved.", response.Copyright);
            Assert.AreEqual("Microsoft.TestHost", response.Product);
            Assert.IsFalse(response.IsLocal);
        }

        [TestMethod("Application - Environment")]
        public async Task InvokeAsync_Should_ReturnEnvironment_When_CalledAsync()
        {
            // Given
            var httpContext = BaseMiddlewareTests.CreateHttpContext(Keys.GetMethod, "/api/system/application/environment");
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            // When
            await middleware.InvokeAsync(httpContext);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = BaseMiddlewareTests.GetResponseFromHttpContext<EnvironmentResponse>(httpContext);
            Assert.IsNotNull(response);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
        }
    }
}
