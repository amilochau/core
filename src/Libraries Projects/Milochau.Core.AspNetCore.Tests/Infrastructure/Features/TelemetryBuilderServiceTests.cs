using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Infrastructure.Features;
using Microsoft.ApplicationInsights.Extensibility;
using Milochau.Core.AspNetCore.Tests.TestHelpers;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Features
{
    [TestClass]
    public class TelemetryBuilderServiceTests
    {
        private IServiceCollection services;

        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private readonly CoreServicesOptions coreServicesOptions = new CoreServicesOptions();

        [TestInitialize]
        public void Initialize()
        {
            services = BaseFeatureBuilderServiceTest.CreateServiceCollection();
        }

        [TestMethod("AddCoreTelemetry - Telemetry enabled")]
        public void AddCoreTelemetry_When_TelemetryIsEnabled()
        {
            // Given
            coreServicesOptions.Telemetry.Enabled = true;

            // When
            TelemetryBuilderService.AddCoreTelemetry(services, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(services);
            var serviceProvider = services.BuildServiceProvider();
            Assert.IsNotNull(serviceProvider.GetService<ITelemetryModule>());
        }

        [TestMethod("AddCoreTelemetry - Telemetry disabled")]
        public void AddCoreTelemetry_When_TelemetryIsDisabled()
        {
            // Given
            coreServicesOptions.Telemetry.Enabled = false;

            // When
            TelemetryBuilderService.AddCoreTelemetry(services, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(services);
            var serviceProvider = services.BuildServiceProvider();
            Assert.IsNull(serviceProvider.GetService<ITelemetryModule>());
        }

        [TestMethod("UseCoreTelemetry")]
        public void UseCoreTelemetry_When_Called()
        {
            // Given
            var applicationBuilder = BaseFeatureBuilderServiceTest.CreateApplicationBuilder(services);

            // When
            TelemetryBuilderService.UseCoreTelemetry(applicationBuilder, coreHostOptions, coreServicesOptions);

            // Then
            Assert.IsNotNull(applicationBuilder);
            // Impossible to check middlewares registration with ASP.NET Core - see _components private field in ApplicationBuilder
        }
    }
}
