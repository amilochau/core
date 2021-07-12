using Microsoft.Extensions.Hosting;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;

namespace Milochau.Core.AspNetCore.ReferenceProject
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
