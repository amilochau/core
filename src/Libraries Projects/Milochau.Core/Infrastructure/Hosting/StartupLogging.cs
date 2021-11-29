using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Milochau.Core.Abstractions;
using System;
using System.Text;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Helper methods to log on application startup</summary>
    public static class StartupLogging
    {
        private const string loggingCategoryName = "Milochau.Core.Infrastructure.Hosting";

        /// <summary>Log application information: assembly and environment</summary>
        public static void LogApplicationInformation(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var hostEnvironment = serviceProvider.GetRequiredService<IApplicationHostEnvironment>();
            var logger = loggerFactory.CreateLogger(loggingCategoryName);

            logger.LogInformation($"Application is now initialized by {loggingCategoryName}.");

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Application information:");
            stringBuilder.AppendLine($"   Organization: {hostEnvironment.OrganizationName}");
            stringBuilder.AppendLine($"   Application: {hostEnvironment.ApplicationName}");
            stringBuilder.AppendLine($"   Environment: {hostEnvironment.EnvironmentName}");
            stringBuilder.AppendLine($"   Host: {hostEnvironment.HostName}");
            stringBuilder.AppendLine($"   Region: {hostEnvironment.RegionName}");

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
