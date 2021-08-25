using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Application;
using Moq;
using System.Net;
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
            var httpRequestData = CreateHttpRequestData("get", "/api/system/application/environment");
            applicationHostEnvironment.SetupGet(x => x.OrganizationName).Returns(organizationName);
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            // When
            var httpResponseData = await functions.GetEnvironmentAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<EnvironmentResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
        }
    }
}
