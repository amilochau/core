using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.Functions.Helpers;
using System;
using System.Net;
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

        /// <summary>Get application information</summary>
        [Function("system-application")]
        public async Task<HttpResponseData> GetInformationAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/application/{type}")] HttpRequestData request, string type)
        {
            return request.Method switch
            {
                Keys.GetMethod when type.Equals("assembly", StringComparison.OrdinalIgnoreCase) => await GetAssemblyAsync(request),
                Keys.GetMethod when type.Equals("environment", StringComparison.OrdinalIgnoreCase) => await GetEnvironmentAsync(request),
                _ => request.WriteEmptyResponseAsync(HttpStatusCode.NotFound),
            };
        }

        /// <summary>Get application asembly</summary>
        internal static async Task<HttpResponseData> GetAssemblyAsync(HttpRequestData request)
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly()!;
            var assemblyResponse = new AssemblyResponse(assembly);

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(assemblyResponse);
            return response;
        }

        /// <summary>Get application environment</summary>
        internal async Task<HttpResponseData> GetEnvironmentAsync(HttpRequestData request)
        {
            var environmentResponse = new EnvironmentResponse(applicationHostEnvironment);

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(environmentResponse);
            return response;
        }
    }
}
