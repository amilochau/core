using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Application;

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
            var response = new EnvironmentResponse(applicationHostEnvironment);
            return new OkObjectResult(response);
        }

        /// <summary>Get application asembly</summary>
        [FunctionName("System-Application-Assembly")]
        public IActionResult Assembly([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/application/assembly")] HttpRequest request)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var response = new AssemblyResponse(assembly);
            return new OkObjectResult(response);
        }
    }
}
