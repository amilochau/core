using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.AspNetCore.Infrastructure.Hosting;

namespace Milochau.Core.AspNetCore.ReferenceProject
{
    public class Startup : CoreApplicationStartup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration)
        {
            this.env = env;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddRazorPages();

            // Swagger
            services.AddControllers();
            services.AddSwaggerGen();

            // Register dependencies
            DependenciesRegistrar.Register(services, configuration);
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Application API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCoreHealthEndpoints();
                endpoints.MapCoreSystemEndpoints();
                endpoints.MapRazorPages();
            });
        }
    }
}
