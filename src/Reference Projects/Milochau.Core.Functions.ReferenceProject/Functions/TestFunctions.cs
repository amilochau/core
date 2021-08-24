using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Milochau.Core.Functions.ReferenceProject
{
    public class TestFunctions
    {
        private readonly CoreHostOptions coreHostOptions;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IApplicationHostEnvironment applicationHostEnvironment;

        public TestFunctions(IOptions<CoreHostOptions> coreHostOptions,
            IHostEnvironment hostEnvironment,
            IApplicationHostEnvironment applicationHostEnvironment)
        {
            this.coreHostOptions = coreHostOptions.Value;
            this.hostEnvironment = hostEnvironment;
            this.applicationHostEnvironment = applicationHostEnvironment;
        }

        [Function("CoreHostOptions")]
        public async Task<HttpResponseData> GetCoreHostOptionsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CoreHostOptions")] HttpRequestData request)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(coreHostOptions);
            return response;
        }

        [Function("HostEnvironment")]
        public async Task<HttpResponseData> GetHostEnvironmentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HostEnvironment")] HttpRequestData request)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(hostEnvironment);
            return response;
        }

        [Function("ApplicationHostEnvironment")]
        public async Task<HttpResponseData> GetApplicationHostEnvironmentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ApplicationHostEnvironment")] HttpRequestData request)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(applicationHostEnvironment);
            return response;
        }
    }
}
