[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- `CoreApplicationStartup.Configure` method now uses only one parameter (`IApplicationBuilder`); you can inject the `IWebHostEnvironment` from constructor if needed
- Azure App Configuration can no more be configured with ConnectionString; you should now use Managed Identity
- `AddCoreFeatures` and `UseCoreFeature` are no more proposed from Milochau.Core.AspNetCore, you should now use the new `CoreApplicationStartup`
- Namespaces change: `Milochau.Core.Infrastructure.Extensions` becomes `Milochau.Core.Infrastructure.Hosting`, `Milochau.Core.AspNetCore.Infrastructure.Extensions` becomes `Milochau.Core.AspNetCore.Infrastructure.Hosting`

## New features

- Host configuration is now read with `DOTNET_` prefix, before trying with `ASPNETCORE_` prefix
- Allow Functions applications without Azure App Configuration
- Add configuration file to test Functions reference file locally
- Remove indirect references to EntityFramework Core
- Creates a new Milochau.Core.Console library, to expose all Milochau.Core features to console applications (**BETA**)
- Support an `OrganizationName` property in host options
- Creates a new Milochau.Core.HealthChecks library, to expose all Milochau.Core custom health checks registration; you should prefer using Milochau.Core.AspNetCore or Milochau.Core.Functions to avoid complexity

## Bug fixes

- appsettings.json file is not read
