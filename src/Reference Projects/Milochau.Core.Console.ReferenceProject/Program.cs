using Microsoft.Extensions.Hosting;
using Milochau.Core.Console.Infrastructure.Hosting;
using System.Threading.Tasks;

namespace Milochau.Core.Console.ReferenceProject
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureCoreHostBuilder<Startup, EntryPoint>()
                .RunConsoleAsync();
        }
    }
}
