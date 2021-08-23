using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Console.Infrastructure.Hosting;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.Functions.ReferenceProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureCoreConfiguration()
                .ConfigureCoreHostBuilder<Startup>()
                .ConfigureFunctionsWorkerDefaults((hostBuilderContext, functionsWorkerApplicationBuilder) =>
                {
                    var serviceProvider = functionsWorkerApplicationBuilder.Services.BuildServiceProvider();

                    var startup = serviceProvider.GetRequiredService<CoreFunctionsStartup>();

                    startup.Configure(serviceProvider, functionsWorkerApplicationBuilder);
                });
    }
}
