using Microsoft.Extensions.Hosting;
using Milochau.Core.Functions.Infrastructure.Hosting;

namespace Milochau.Core.Functions.ReferenceProject
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            new HostBuilder()
                .ConfigureFunctionsCoreHostBuilder<Startup>();
    }
}
