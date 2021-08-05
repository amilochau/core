using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Console.Infrastructure.Hosting
{
    /// <summary>
    /// HostedService handling application lifecycle and console program bootstrap.
    /// </summary>
    /// <remarks>
    /// Based on:
    ///  - https://dfederm.com/building-a-console-app-with-.net-generic-host/
    ///  - https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host
    ///  - https://stackoverflow.com/questions/41407221/startup-cs-in-a-self-hosted-net-core-console-application
    /// </remarks>
    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly ILogger<ConsoleHostedService> logger;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly IServiceProvider serviceProvider;

        private int? exitCode;

        public ConsoleHostedService(ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime hostApplicationLifetime,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.hostApplicationLifetime = hostApplicationLifetime;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        logger.LogDebug($"Configuring application");
                        var startup = serviceProvider.GetRequiredService<CoreConsoleStartup>();
                        startup.Configure(serviceProvider);

                        logger.LogDebug($"Calling IApplication.RunAsync()");
                        using var scope = serviceProvider.CreateScope();
                        var application = scope.ServiceProvider.GetRequiredService<CoreConsoleEntryPoint>();
                        exitCode = await application.RunAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled exception!");
                        exitCode = -1;
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        hostApplicationLifetime.StopApplication();
                    }
                }, cancellationToken);
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Exiting with return code: {exitCode}");

            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.ExitCode = exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}
