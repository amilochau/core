using Microsoft.Extensions.Hosting;
using Milochau.Core.Functions.Infrastructure.Hosting;

namespace Milochau.Core.Functions.ReferenceProject
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureCoreHostBuilder<Startup>();
    }
}
