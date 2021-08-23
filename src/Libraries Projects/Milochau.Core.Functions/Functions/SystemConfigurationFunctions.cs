﻿using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Milochau.Core.Infrastructure.Features.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>System Functions to expose configuration management</summary>
    public class SystemConfigurationFunctions
    {
        private readonly IFeatureManager featureManager;
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public SystemConfigurationFunctions(IFeatureManager featureManager,
            IConfiguration configuration)
        {
            this.featureManager = featureManager;
            this.configuration = configuration;
        }

        /// <summary>Get feature flags state</summary>
        [Function("System-Configuration-Flags")]
        public async Task<HttpResponseData> FlagsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/configuration/flags")] HttpRequestData request)
        {
            var flagsResponse = new FlagsResponse();
            await foreach (var featureName in featureManager.GetFeatureNamesAsync())
            {
                var enabled = await featureManager.IsEnabledAsync(featureName);
                flagsResponse.Features.Add(new FeatureDetails { Name = featureName, Enabled = enabled });
            }

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(flagsResponse);
            return response;
        }

        /// <summary>Get configuration providers</summary>
        [Function("System-Configuration-Providers")]
        public async Task<HttpResponseData> Providers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/configuration/providers")] HttpRequestData request)
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
