﻿using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Milochau.Core.HealthChecks;
using Milochau.Core.HealthChecks.Models;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>Health Functions to expose health checks results</summary>
    public class HealthFunctions
    {
        private readonly HealthCheckService healthCheckService;

        /// <summary>Constructor</summary>
        public HealthFunctions(HealthCheckService healthCheckService)
        {
            this.healthCheckService = healthCheckService;
        }

        /// <summary>Get default application health</summary>
        [Function("Health-Default")]
        public async Task<HttpResponseData> GetHealthDefaultAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData request)
        {
            var healthReport = await healthCheckService.CheckHealthAsync();
            return await ConvertHealthReportToActionResultAsync(request, healthReport);
        }

        /// <summary>Get light application health</summary>
        [Function("Health-Light")]
        public async Task<HttpResponseData> GetHealthLightAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/light")] HttpRequestData request)
        {
            var healthReport = await healthCheckService.CheckHealthAsync(x => x.Tags.Contains(HealthChecksRegistration.LightTag));
            return await ConvertHealthReportToActionResultAsync(request, healthReport);
        }

        private async Task<HttpResponseData> ConvertHealthReportToActionResultAsync(HttpRequestData request, HealthReport healthReport)
        {
            var statusCode = healthReport.Status switch
            {
                HealthStatus.Unhealthy => HttpStatusCode.ServiceUnavailable,
                HealthStatus.Degraded => HttpStatusCode.OK,
                HealthStatus.Healthy => HttpStatusCode.OK,
                _ => HttpStatusCode.OK,
            };

            var detailedHealthReport = DetailedHealthReport.CreateFrom(healthReport);
            var jsonHealthReport = JsonSerializer.Serialize(detailedHealthReport, HealthChecksResponseWriter.JsonOptions.Value);

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(jsonHealthReport);
            response.StatusCode = statusCode;
            return response;
        }
    }
}
