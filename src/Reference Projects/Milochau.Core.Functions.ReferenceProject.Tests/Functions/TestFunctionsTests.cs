using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Tests.Functions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Net;
using Milochau.Core.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Milochau.Core.Functions.ReferenceProject.Tests.Functions
{
    [TestClass]
    public class TestFunctionsTests : BaseFunctionsTests
    {
        private Mock<IOptions<CoreHostOptions>> options = null!;
        private CoreHostOptions optionsValue = null!;
        private Mock<IHostEnvironment> hostEnvironment = null!;
        private Mock<IApplicationHostEnvironment> applicationHostEnvironment = null!;
        private Mock<IConfiguration> configuration = null!;

        private TestFunctions functions = null!;

        private const string organizationName = "organizationName";
        private const string applicationName = "applicationName";
        private const string environmentName = "environmentName";
        private const string hostName = "hostName";
        private const string regionName = "regionName";

        [TestInitialize]
        public void Initialize()
        {
            options = new Mock<IOptions<CoreHostOptions>>();
            optionsValue = new CoreHostOptions();
            options.SetupGet(x => x.Value).Returns(optionsValue);
            hostEnvironment = new Mock<IHostEnvironment>();
            applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();
            configuration = new Mock<IConfiguration>();

            functions = new TestFunctions(options.Object, hostEnvironment.Object, applicationHostEnvironment.Object, configuration.Object);
        }

        [TestMethod]
        public async Task GetCoreHostOptions_Should_ReturnOptions_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/CoreHostOptions");
            optionsValue.Application.OrganizationName = organizationName;
            optionsValue.Application.ApplicationName = applicationName;
            optionsValue.Application.EnvironmentName = environmentName;
            optionsValue.Application.HostName = hostName;
            optionsValue.Application.RegionName = regionName;

            // When
            var result = await functions.GetCoreHostOptionsAsync(httpRequestData, CancellationToken.None);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseAsText(result, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetHostEnvironment_Should_ReturnApplicationHostEnvironment_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/HostEnvironment");
            hostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            hostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            // When
            var result = await functions.GetHostEnvironmentAsync(httpRequestData, CancellationToken.None);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseAsText(result, HttpStatusCode.OK);
            Assert.AreEqual("{\"environmentName\":\"environmentName\",\"applicationName\":\"applicationName\",\"contentRootPath\":null,\"contentRootFileProvider\":null}", response);
        }

        [TestMethod]
        public async Task GetApplicationHostEnvironment_Should_ReturnApplicationHostEnvironment_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/ApplicationHostEnvironment");
            applicationHostEnvironment.SetupGet(x => x.OrganizationName).Returns(organizationName);
            applicationHostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            applicationHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);
            applicationHostEnvironment.SetupGet(x => x.HostName).Returns(hostName);
            applicationHostEnvironment.SetupGet(x => x.RegionName).Returns(regionName);

            // When
            var result = await functions.GetApplicationHostEnvironmentAsync(httpRequestData, CancellationToken.None);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseAsJson<ApplicationHostEnvironment>(result, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(organizationName, response.OrganizationName);
            Assert.AreEqual(applicationName, response.ApplicationName);
            Assert.AreEqual(environmentName, response.EnvironmentName);
            Assert.AreEqual(hostName, response.HostName);
            Assert.AreEqual(regionName, response.RegionName);
        }
    }
}
