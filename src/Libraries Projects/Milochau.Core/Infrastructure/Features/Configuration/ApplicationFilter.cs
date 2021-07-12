using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.Infrastructure.Features.Configuration
{
    /// <summary>
    /// This Feature Filter is used to enable a new feature (from Azure App Configuration) only when the current application is targetted.
    /// In Azure App Configuration UI, use a custom filter named 'Application', and set a filter parameter named 'Value'.
    /// By example, set the 'Value' key with the 'Luca,Sofia' value to enable your feature in Luca and Sofia Applications, but disable in MonEspace, Crm, etc.
    /// </summary>
    public class ApplicationFilter : IFeatureFilter
    {
        private const string alias = "Application";
        private readonly char[] separator = { ',', ';', ' ' };

        private readonly IApplicationHostEnvironment applicationHostEnvironment;
        private readonly ILogger<ApplicationFilter> logger;

        /// <summary>Constructor</summary>
        /// <param name="applicationHostEnvironment">Application host environment</param>
        /// <param name="logger">Logger</param>
        public ApplicationFilter(IApplicationHostEnvironment applicationHostEnvironment,
            ILogger<ApplicationFilter> logger)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
            this.logger = logger;
        }

        /// <summary>Evaluates filter</summary>
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var applicationFilterSettings = context.Parameters.Get<ApplicationFilterSettings>() ?? new ApplicationFilterSettings();
            if (string.IsNullOrEmpty(applicationFilterSettings.Value))
            {
                logger.LogWarning($"The '{alias}' feature filter does not have a valid 'Value' value for feature '{context.FeatureName}'");
                return Task.FromResult(false);
            }

            var flag = applicationFilterSettings.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries).Contains(applicationHostEnvironment.ApplicationName, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(flag);
        }
    }
}
