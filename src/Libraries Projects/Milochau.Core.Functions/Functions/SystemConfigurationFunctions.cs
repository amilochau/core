using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.Functions.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>System Functions to expose configuration management</summary>
    public class SystemConfigurationFunctions
    {
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public SystemConfigurationFunctions(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>Get configuration information</summary>
        [Function("system-configuration")]
        public async Task<HttpResponseData> GetInformationAsync([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "system/configuration/{type}")] HttpRequestData request, string type)
        {
            return request.Method switch
            {
                Keys.GetMethod when type.Equals("providers", StringComparison.OrdinalIgnoreCase) => await GetProvidersAsync(request),
                _ => request.WriteEmptyResponseAsync(HttpStatusCode.NotFound),
            };
        }

        /// <summary>Get configuration providers</summary>
        internal async Task<HttpResponseData> GetProvidersAsync(HttpRequestData request)
        {
            var providersResponse = new ProvidersResponse();

            if (configuration is ConfigurationRoot configurationRoot)
            {
                providersResponse.Providers = configurationRoot.Providers.Select(x => x.ToString());
            }

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(providersResponse);
            return response;
        }
    }
}
