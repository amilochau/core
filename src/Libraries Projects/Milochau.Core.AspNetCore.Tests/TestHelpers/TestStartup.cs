using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
    public class TestStartup : CoreApplicationStartup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCoreHealthEndpoints();
                endpoints.MapCoreSystemEndpoints();
            });
        }
    }
}
