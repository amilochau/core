using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Application;
using Moq;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class SystemApplicationFunctionsTests : BaseFunctionsTests
    {
        private Mock<IApplicationHostEnvironment> applicationHostEnvironment;

        private SystemApplicationFunctions functions;

        private const string applicationName = "applicationName";
        private const string hostName = "hostName";
        private const string environmentName = "environmentName";

        [TestInitialize]
        public void Initialize()
        {
            applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();

            functions = new SystemApplicationFunctions(applicationHostEnvironment.Object);
        }
        
        [TestMethod("Application - Environment")]
        public void Environment_Should_ReturnEnvironment_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/application/environment");
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            // When
            var result = functions.Environment(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<EnvironmentResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
        }
    }
}
