[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- `CoreApplicationStartup.Configure` method now uses only one parameter (`IApplicationBuilder`); you can inject the `IWebHostEnvironment` from constructor if needed

## New features

- Host configuration is now read with `DOTNET_` prefix, before trying with `ASPNETCORE_` prefix
