using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Milochau.Core.Abstractions.Exceptions;
using Milochau.Core.Functions.Helpers;
using System.Threading;
using Milochau.Core.Functions.ReferenceProject.Models;
using System.Net;

namespace Milochau.Core.Functions.ReferenceProject
{
    public class TestFunctions
    {
        private readonly CoreHostOptions coreHostOptions;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IApplicationHostEnvironment applicationHostEnvironment;
        private readonly IConfiguration configuration;

        public TestFunctions(IOptions<CoreHostOptions> coreHostOptions,
            IHostEnvironment hostEnvironment,
            IApplicationHostEnvironment applicationHostEnvironment,
            IConfiguration configuration)
        {
            this.coreHostOptions = coreHostOptions.Value;
            this.hostEnvironment = hostEnvironment;
            this.applicationHostEnvironment = applicationHostEnvironment;
            this.configuration = configuration;
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

        [Function("Configuration")]
        public async Task<HttpResponseData> GetConfigurationAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Configuration")] HttpRequestData request)
        {
            var response = request.CreateResponse();

            var keys = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues("key")?.Where(x => !string.IsNullOrEmpty(x));
            var valuesResponse = new Dictionary<string, string>();
            if (keys != null && keys.Any())
            {
                foreach (var key in keys)
                {
                    valuesResponse.Add(key, configuration[key]);
                }
            }

            await response.WriteAsJsonAsync(valuesResponse);
            return response;
        }

        [Function("NotFoundException")]
        public Task<HttpResponseData> GetNotFoundExceptionAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "NotFoundException")] HttpRequestData request)
        {
            throw new NotFoundException();
        }

        [Function("BadRequest")]
        public async Task<HttpResponseData> GetValidationFromQueryAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ValidationFromQuery")] HttpRequestData request)
        {
            var validationResult = await request.ReadAndValidateRequestQueryAsync<ValidationFromQuery>(CancellationToken.None);
            if (!validationResult.IsValid || validationResult.Data == null)
            {
                return await request.WriteResponseAsJsonAsync(validationResult.ProblemDetails, HttpStatusCode.BadRequest, CancellationToken.None);
            }

            return await request.WriteResponseAsJsonAsync(validationResult.Data, HttpStatusCode.OK, CancellationToken.None);
        }

        [Function("SerializationExample")]
        public async Task<HttpResponseData> GetSerializationExampleAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "SerializationExample")] HttpRequestData request)
        {
            var serializationExample = new SerializationExample();
            return await request.WriteResponseAsJsonAsync(serializationExample, HttpStatusCode.OK, CancellationToken.None);
        }
    }
}
