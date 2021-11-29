using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Tests.Functions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Net;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.Functions.ReferenceProject.Tests.Functions
{
    [TestClass]
    public class TestFunctionsTests : BaseFunctionsTests
    {
        private Mock<IOptions<CoreHostOptions>> options;
        private CoreHostOptions optionsValue;
        private Mock<IHostEnvironment> hostEnvironment;
        private Mock<IApplicationHostEnvironment> applicationHostEnvironment;

        private TestFunctions functions;

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

            functions = new TestFunctions(options.Object, hostEnvironment.Object, applicationHostEnvironment.Object);
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
            var result = await functions.GetCoreHostOptionsAsync(httpRequestData);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseAsText(result, HttpStatusCode.OK);
            Assert.AreEqual("{\"application\":{\"organizationName\":\"organizationName\",\"applicationName\":\"applicationName\",\"environmentName\":\"environmentName\",\"hostName\":\"hostName\",\"regionName\":\"regionName\"},\"appConfig\":{\"endpoint\":null,\"namespaceSeparator\":\"/\",\"sentinelKey\":\"Sentinel:Key\",\"refreshExpirationInMinutes\":120},\"keyVault\":{\"vault\":null},\"credential\":{\"interactiveBrowserTenantId\":null,\"sharedTokenCacheTenantId\":null,\"visualStudioTenantId\":null,\"visualStudioCodeTenantId\":null,\"sharedTokenCacheUsername\":null,\"interactiveBrowserCredentialClientId\":null,\"managedIdentityClientId\":null,\"excludeEnvironmentCredential\":false,\"excludeManagedIdentityCredential\":false,\"excludeSharedTokenCacheCredential\":true,\"excludeInteractiveBrowserCredential\":true,\"excludeAzureCliCredential\":false,\"excludeVisualStudioCredential\":false,\"excludeVisualStudioCodeCredential\":false,\"excludeAzurePowerShellCredential\":false,\"authorityHost\":\"https://login.microsoftonline.com/\",\"transport\":{},\"diagnostics\":{\"isLoggingEnabled\":true,\"isDistributedTracingEnabled\":true,\"isTelemetryEnabled\":true,\"isLoggingContentEnabled\":false,\"loggedContentSizeLimit\":4096,\"loggedHeaderNames\":[\"x-ms-request-id\",\"x-ms-client-request-id\",\"x-ms-return-client-request-id\",\"traceparent\",\"MS-CV\",\"Accept\",\"Cache-Control\",\"Connection\",\"Content-Length\",\"Content-Type\",\"Date\",\"ETag\",\"Expires\",\"If-Match\",\"If-Modified-Since\",\"If-None-Match\",\"If-Unmodified-Since\",\"Last-Modified\",\"Pragma\",\"Request-Id\",\"Retry-After\",\"Server\",\"Transfer-Encoding\",\"User-Agent\"],\"loggedQueryParameters\":[],\"applicationId\":null},\"retry\":{\"maxRetries\":3,\"delay\":\"00:00:00.8000000\",\"maxDelay\":\"00:01:00\",\"mode\":\"Exponential\",\"networkTimeout\":\"00:01:40\"}}}", response);
        }

        [TestMethod]
        public async Task GetHostEnvironment_Should_ReturnApplicationHostEnvironment_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/HostEnvironment");
            hostEnvironment.SetupGet(x => x.ApplicationName).Returns(applicationName);
            hostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);

            // When
            var result = await functions.GetHostEnvironmentAsync(httpRequestData);

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
            var result = await functions.GetApplicationHostEnvironmentAsync(httpRequestData);

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
