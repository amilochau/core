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
        [Function("health")]
        public async Task<HttpResponseData> GetHealthDefaultAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/{tag?}")] HttpRequestData request, string? tag = null)
        {
            HealthReport? healthReport;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                healthReport = await healthCheckService.CheckHealthAsync(x => x.Tags.Contains(tag));
            }
            else
            {
                healthReport = await healthCheckService.CheckHealthAsync();
            }
            return await ConvertHealthReportToActionResultAsync(request, healthReport);
        }

        private static async Task<HttpResponseData> ConvertHealthReportToActionResultAsync(HttpRequestData request, HealthReport healthReport)
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
            await response.WriteStringAsync(jsonHealthReport);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.StatusCode = statusCode;
            return response;
        }
    }
}
