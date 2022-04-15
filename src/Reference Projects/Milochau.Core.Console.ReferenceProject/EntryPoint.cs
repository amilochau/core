using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Console.ReferenceProject
{
    public class EntryPoint : CoreConsoleEntryPoint
    {
        private readonly CoreHostOptions options;
        private readonly IConfiguration configuration;
        private readonly ILogger<EntryPoint> logger;

        public EntryPoint(IOptions<CoreHostOptions> options,
            IConfiguration configuration,
            ILogger<EntryPoint> logger)
        {
            this.options = options.Value;
            this.configuration = configuration;
            this.logger = logger;
        }

        public override Task<int> RunAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning(options.Application.ApplicationName);

            var configurationRoot = configuration as ConfigurationRoot;
            logger.LogWarning("Providers: {0}", configurationRoot.Providers.Count());
            foreach (var provider in configurationRoot.Providers)
            {
                logger.LogWarning("   {0}", provider.ToString());
            }

            logger.LogWarning($"Testing configuration build");
            logger.LogWarning($"   A: {configuration["A"]}");
            logger.LogWarning($"   B: {configuration["B"]}");
            logger.LogWarning($"   C: {configuration["C"]}");
            logger.LogWarning($"   D: {configuration["D"]}");

            return Task.FromResult(0);
        }
    }
}
