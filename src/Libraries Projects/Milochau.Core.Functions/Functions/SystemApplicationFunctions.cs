using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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
        [Function("System-Application-Environment")]
        public IActionResult Environment([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/application/environment")] HttpRequest request)
        {
            var response = new EnvironmentResponse(applicationHostEnvironment);
            return new OkObjectResult(response);
        }

        /// <summary>Get application asembly</summary>
        [Function("System-Application-Assembly")]
        public IActionResult Assembly([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/application/assembly")] HttpRequest request)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var response = new AssemblyResponse(assembly);
            return new OkObjectResult(response);
        }
    }
}
