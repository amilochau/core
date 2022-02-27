﻿using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions.Models;
using System.Linq;
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

        /// <summary>Get configuration providers</summary>
        [Function("system-configuration-providers")]
        public async Task<HttpResponseData> GetProvidersAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/configuration/providers")] HttpRequestData request)
        {
            var configurationRoot = configuration as ConfigurationRoot;
            var providersResponse = new ProvidersResponse
            {
                Providers = configurationRoot.Providers.Select(x => x.ToString())
            };

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(providersResponse);
            return response;
        }
    }
}
