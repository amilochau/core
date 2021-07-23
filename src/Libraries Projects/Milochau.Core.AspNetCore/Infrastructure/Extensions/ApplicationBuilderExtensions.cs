using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Milochau.Core.AspNetCore.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System;

namespace Milochau.Core.AspNetCore.Infrastructure.Extensions
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/></summary>
    public static class ApplicationBuilderExtensions
    {
        private const string loggingCategoryName = "Milochau.Core.AspNetCore.Infrastructure";

        /// <summary>Adds middlewares needed by all the features activated from configuration</summary>
        /// <param name="app">Application builder</param>
        public static IApplicationBuilder UseCoreFeatures(this IApplicationBuilder app)
        {
            var hostOptions = app.ApplicationServices.GetService<IOptions<CoreHostOptions>>();
            var servicesOptions = app.ApplicationServices.GetService<IOptions<CoreServicesOptions>>();

            return UseCoreFeaturesInternal(app, hostOptions.Value, servicesOptions.Value);
        }

        private static IApplicationBuilder UseCoreFeaturesInternal(IApplicationBuilder app, CoreHostOptions hostOptions, CoreServicesOptions servicesOptions)
        {
            app = app
                .UseCoreApplication(hostOptions, servicesOptions)
                .UseCoreConfiguration(hostOptions, servicesOptions)
                .UseCoreTelemetry(hostOptions, servicesOptions);

            LogApplicationBuilder(app);

            return app;
        }

        internal static void LogApplicationBuilder(IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var hostEnvironment = app.ApplicationServices.GetRequiredService<IApplicationHostEnvironment>();
            var logger = loggerFactory.CreateLogger(loggingCategoryName);

            logger.LogInformation($"Application is now initialized by {loggingCategoryName}.");

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Application information:");
            stringBuilder.AppendLine($"   Environment: {hostEnvironment.EnvironmentName}");
            stringBuilder.AppendLine($"   Host: {hostEnvironment.HostName}");
            stringBuilder.AppendLine($"   Application: {hostEnvironment.ApplicationName}");

            var machineName = Environment.MachineName;
            var cpus = Environment.ProcessorCount;
            stringBuilder.AppendLine($"   Machine name: {machineName} // {cpus} CPUs");

            var osVersion = Environment.OSVersion;
            var osBits = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            var clrVersion = Environment.Version;
            var processBits = Environment.Is64BitProcess ? "x64" : "x86";
            var frameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            stringBuilder.AppendLine($"   OS Version: {osVersion} ({osBits})");
            stringBuilder.AppendLine($"   CLR Version: {clrVersion} ({processBits})");
            stringBuilder.AppendLine($"   Framework Version: {frameworkDescription}");

            stringBuilder.AppendLine($"   Local Time Zone: {TimeZoneInfo.Local}");
            stringBuilder.AppendLine($"   UTC Time Zone: {TimeZoneInfo.Utc}");

            logger.LogInformation(stringBuilder.ToString());
        }
    }
}
