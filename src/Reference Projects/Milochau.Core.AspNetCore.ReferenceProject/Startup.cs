using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;
using Milochau.Core.AspNetCore.ReferenceProject.Models;

namespace Milochau.Core.AspNetCore.ReferenceProject
{
    public class Startup : CoreApplicationStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            RegisterOptions(services);
            RegisterServices(services);
            RegisterDataAccess(services);

            services.AddRazorPages();

            // Swagger
            services.AddControllers();
            services.AddSwaggerGen();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

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

        private void RegisterOptions(IServiceCollection services)
        {
            services.Configure<TestOptions>(configuration.GetSection("Test"));
        }

        private void RegisterServices(IServiceCollection services)
        {
            // Register services here
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            // Register data access here
        }
    }
}
