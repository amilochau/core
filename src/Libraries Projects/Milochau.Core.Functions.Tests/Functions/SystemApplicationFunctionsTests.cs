using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Abstractions.Models;
using Milochau.Core.Functions.Functions;
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
        private const string environmentName = "environmentName";
        private const string hostName = "hostName";
        private const string regionName = "regionName";

        [TestInitialize]
        public void Initialize()
        {
            applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();

            functions = new SystemApplicationFunctions(applicationHostEnvironment.Object);
        }
        
        [TestMethod("Application - Environment")]
        public async Task GetEnvironment_Should_ReturnEnvironment_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/application/environment");
            applicationHostEnvironment.SetupGet(x => x.OrganizationName).Returns(organizationName);
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.RegionName).Returns(regionName);

            // When
            var httpResponseData = await functions.GetEnvironmentAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<EnvironmentResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(organizationName, response.OrganizationName);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(regionName, response.RegionName);
        }
    }
}
