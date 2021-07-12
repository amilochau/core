using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Milochau.Core.Infrastructure.Converters;
using Milochau.Core.Infrastructure.Features.Health;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
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
        [FunctionName("Health-Default")]
        public async Task<IActionResult> HealthDefaultAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest request, CancellationToken cancellationToken)
        {
            var healthReport = await healthCheckService.CheckHealthAsync(cancellationToken);
            return ConvertHealthReportToActionResult(healthReport);
        }

        /// <summary>Get light application health</summary>
        [FunctionName("Health-Light")]
        public async Task<IActionResult> HealthLightAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/light")] HttpRequest request, CancellationToken cancellationToken)
        {
            var healthReport = await healthCheckService.CheckHealthAsync(x => x.Tags.Contains(HealthChecksRegistration.LightTag), cancellationToken);
            return ConvertHealthReportToActionResult(healthReport);
        }

        private IActionResult ConvertHealthReportToActionResult(HealthReport healthReport)
        {
            var statusCode = healthReport.Status switch
            {
                HealthStatus.Unhealthy => StatusCodes.Status503ServiceUnavailable,
                HealthStatus.Degraded => StatusCodes.Status200OK,
                HealthStatus.Healthy => StatusCodes.Status200OK,
                _ => StatusCodes.Status200OK,
            };

            var uiHealthReport = UIHealthReport.CreateFrom(healthReport);
            var jsonHealthReport = JsonSerializer.Serialize(uiHealthReport, CreateJsonOptions());
            return new ObjectResult(jsonHealthReport) { StatusCode = statusCode };
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
                Converters =
                {
                  new JsonStringEnumConverter(),
                  new TimeSpanConverter()
                }
            };
        }
    }
}
