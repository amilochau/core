using Microsoft.AspNetCore.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Application;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    /// <summary>Middleware that exposes an application information response with a URL endpoint</summary>
    internal class SystemApplicationMiddleware
    {
        private readonly IApplicationHostEnvironment applicationHostEnvironment;

        /// <summary>Constructor</summary>
        public SystemApplicationMiddleware(RequestDelegate _,
            IApplicationHostEnvironment applicationHostEnvironment)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
        }

        /// <summary>Processes a request</summary>
        /// <param name="httpContext">HTTP context</param>
        public Task InvokeAsync(HttpContext httpContext)
        {
            return httpContext.Request.Method switch
            {
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/assembly", StringComparison.OrdinalIgnoreCase) => AssemblyInformationAsync(httpContext),
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/environment", StringComparison.OrdinalIgnoreCase) => EnvironmentAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private Task AssemblyInformationAsync(HttpContext httpContext)
        {
            var assembly = Assembly.GetEntryAssembly();
            var response = new AssemblyResponse
            {
                Company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
                Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration,
                Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright,
                Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description,
                FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
                InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                Product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product
            };

            response.IsLocal = AssemblyResponse.IsLocalRegex.IsMatch(response.InformationalVersion);
            response.BuildId = AssemblyResponse.BuildRegex.Match(response.InformationalVersion).Groups[1].Value;
            response.BuildSourceVersion = AssemblyResponse.BuildRegex.Match(response.InformationalVersion).Groups[2].Value;

            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }

        private Task EnvironmentAsync(HttpContext httpContext)
        {
            var response = new EnvironmentResponse
            {
                ApplicationName = applicationHostEnvironment.ApplicationName,
                HostName = applicationHostEnvironment.HostName,
                EnvironmentName = applicationHostEnvironment.EnvironmentName,

                MachineName = Environment.MachineName,
                ProcessorCount = Environment.ProcessorCount,
                OSVersion = Environment.OSVersion.ToString(),
                ClrVersion = Environment.Version.ToString(),
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,

                LocalTimeZone = TimeZoneInfo.Local.Id,
                UtcTimeZone = TimeZoneInfo.Utc.Id,

                CurrentCulture = CultureInfo.CurrentCulture.Name
            };
            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }
    }
}
