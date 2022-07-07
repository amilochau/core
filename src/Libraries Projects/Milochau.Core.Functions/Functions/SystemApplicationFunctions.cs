using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Abstractions.Models.System;
using System.Threading.Tasks;

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
        [Function("system-application-environment")]
        public async Task<HttpResponseData> GetEnvironmentAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/application/environment")] HttpRequestData request)
        {
            var environmentResponse = new EnvironmentResponse(applicationHostEnvironment);

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(environmentResponse);
            return response;
        }

        /// <summary>Get application asembly</summary>
        [Function("system-application-assembly")]
        public async Task<HttpResponseData> GetAssemblyAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/application/assembly")] HttpRequestData request)
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly()!;
            var assemblyResponse = new AssemblyResponse(assembly);

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(assemblyResponse);
            return response;
        }
    }
}
