using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Functions.Functions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class HealthFunctionsTests : BaseFunctionsTests
    {
        private Mock<HealthCheckService> healthCheckService;

        private HealthFunctions functions;

        [TestInitialize]
        public void Initialize()
        {
            healthCheckService = new Mock<HealthCheckService>();

            functions = new HealthFunctions(healthCheckService.Object);
        }

        [TestMethod("Health-Default")]
        public async Task Health_Should_ReturnHealthy_When_NoEntryAsync()
        {
            // Given
            var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), default);

            var httpRequestData = CreateHttpRequestData("get", "/api/health");
            healthCheckService.Setup(x => x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>())).ReturnsAsync(healthReport);

            // When
            var httpResponseData = await functions.GetHealthDefaultAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<HealthReport>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(HealthStatus.Healthy, response.Status);
        }

        [TestMethod("Health-Default - unhealth with default checks")]
        public async Task Health_Should_ReturnUnhealthy_When_CalledWithDefaultChecksAsync()
        {
            // Given
            var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>
            {
                { "Unhealth check", new HealthReportEntry(HealthStatus.Unhealthy, "Unhealth check", default, default, default) }
            }, default);

            var httpRequestData = CreateHttpRequestData("get", "/api/health");
            healthCheckService.Setup(x => x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>())).ReturnsAsync(healthReport);

            // When
            var httpResponseData = await functions.GetHealthDefaultAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<HealthReport>(httpResponseData, HttpStatusCode.ServiceUnavailable);
            Assert.IsNotNull(response);
            Assert.AreEqual(HealthStatus.Unhealthy, response.Status);
        }

        [TestMethod("Health-Light - unhealth with filtered checks")]
        public async Task Health_Should_ReturnUnhealthy_When_CalledWithFilteredChecksAsync()
        {
            // Given
            var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), default);

            var httpRequestData = CreateHttpRequestData("get", "/api/health/light");
            healthCheckService.Setup(x => x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>())).ReturnsAsync(healthReport);

            // When
            var httpResponseData = await functions.GetHealthLightAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<HealthReport>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(HealthStatus.Healthy, response.Status);
        }
    }
}
