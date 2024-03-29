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

        [Function("test-corehostoptions")]
        public async Task<HttpResponseData> GetCoreHostOptionsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CoreHostOptions")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(coreHostOptions, cancellationToken);
            return response;
        }

        [Function("test-hostenvironment")]
        public async Task<HttpResponseData> GetHostEnvironmentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HostEnvironment")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(hostEnvironment, cancellationToken);
            return response;
        }

        [Function("test-applicationhostenvironment")]
        public async Task<HttpResponseData> GetApplicationHostEnvironmentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ApplicationHostEnvironment")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(applicationHostEnvironment, cancellationToken);
            return response;
        }

        [Function("test-configuration")]
        public async Task<HttpResponseData> GetConfigurationAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Configuration")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var response = request.CreateResponse();

            var keys = System.Web.HttpUtility.ParseQueryString(request.Url.Query).GetValues("key")?.Where(x => !string.IsNullOrEmpty(x));
            var valuesResponse = new Dictionary<string, string?>();
            if (keys != null && keys.Any())
            {
                foreach (var key in keys)
                {
                    valuesResponse.Add(key, configuration[key]);
                }
            }

            await response.WriteAsJsonAsync(valuesResponse, cancellationToken);
            return response;
        }

        [Function("test-notfoundexception")]
        public Task<HttpResponseData> GetNotFoundExceptionAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "NotFoundException")] HttpRequestData request)
        {
            throw new NotFoundException();
        }

        [Function("test-badrequest")]
        public async Task<HttpResponseData> GetValidationFromQueryAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ValidationFromQuery")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var validationResult = await request.ReadAndValidateRequestQueryAsync<ValidationFromQuery>(cancellationToken);
            if (!validationResult.IsValid || validationResult.Data == null)
            {
                return await request.WriteResponseAsJsonAsync(validationResult.ProblemDetails, HttpStatusCode.BadRequest, cancellationToken);
            }

            return await request.WriteResponseAsJsonAsync(validationResult.Data, HttpStatusCode.OK, cancellationToken);
        }

        [Function("test-serializationexample")]
        public async Task<HttpResponseData> GetSerializationExampleAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "SerializationExample")] HttpRequestData request, CancellationToken cancellationToken)
        {
            var serializationExample = new SerializationExample();
            return await request.WriteResponseAsJsonAsync(serializationExample, HttpStatusCode.OK, cancellationToken);
        }
    }
}
