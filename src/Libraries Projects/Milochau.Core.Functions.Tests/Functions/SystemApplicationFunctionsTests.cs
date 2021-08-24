using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Application;
using Moq;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class SystemApplicationFunctionsTests : BaseFunctionsTests
    {
        private Mock<IApplicationHostEnvironment> applicationHostEnvironment;

        private SystemApplicationFunctions functions;

        private const string organizationName = "organizationName";
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
        public async Task Environment_Should_ReturnEnvironment_When_CalledAsync()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/application/environment");
            applicationHostEnvironment.SetupGet(x => x.OrganizationName).Returns(organizationName);
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            var context = new Mock<FunctionContext>();

            var httpRequestData = new Mock<HttpRequestData>(context.Object);

            // When
            var response = await functions.GetEnvironmentAsync(httpRequestData.Object);

            // Then
            Assert.IsNotNull(response);
            var response = GetResponseFromActionResult<EnvironmentResponse>(response, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
        }
    }
}
