using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Application;
using System.Globalization;
using System;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>System Functions to expose application information</summary>
    public class SystemApplicationFunctions
    {
        private readonly IApplicationHostEnvironment applicationHostEnvironment;

        /// <summary>Constructor</summary>
        public SystemApplicationFunctions(IApplicationHostEnvironment applicationHostEnvironment)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
        }

        /// <summary>Get application environment</summary>
        [FunctionName("System-Application-Environment")]
        public IActionResult Environment([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/application/environment")] HttpRequest request)
        {
            var response = new EnvironmentResponse
            {
                ApplicationName = applicationHostEnvironment.ApplicationName,
                HostName = applicationHostEnvironment.HostName,
                EnvironmentName = applicationHostEnvironment.EnvironmentName,

                MachineName = System.Environment.MachineName,
                ProcessorCount = System.Environment.ProcessorCount,
                OSVersion = System.Environment.OSVersion.ToString(),
                ClrVersion = System.Environment.Version.ToString(),
                Is64BitOperatingSystem = System.Environment.Is64BitOperatingSystem,
                Is64BitProcess = System.Environment.Is64BitProcess,

                LocalTimeZone = TimeZoneInfo.Local.Id,
                UtcTimeZone = TimeZoneInfo.Utc.Id,

                CurrentCulture = CultureInfo.CurrentCulture.Name
            };
            return new OkObjectResult(response);
        }
    }
}
