# Changelog

## Introduction

This file lists all the public features of `Milochau.Core.*` libraries.

## Version 7.2.0

New features:

- Health checks are now registered and exposed as health endpoints in Functions applications

## Version 7.1.1

Bug fixes:

- Rollback minimum version of `System.Text.Json` to 4.7.2 for Functions

## Version 7.1.0

New features:

- Update dependencies to target .NET Core 3.1+
- Add more environment on environment, time zone and culture on `/system/application/environment` endpoint
- Support request localization with `Core:Services:RequestLocalization` options
- Add more logs on application initialization (ASP.NET Core only)
- Propose a `TimeSpanConverter` for System.Text.Json serialization in `Milochau.Core`

## Version 7.0.0

New features:

- Application Insights integration now proposed new settings
- Introduce `IApplicationMemoryCache` to boost local cache
- Add a configuration filter by `ApplicationName` for Azure App Configuration
- Exposes new endpoints for cache management
- Exposes new endpoints for application information
- Exposes new endpoints for health checks
- Exposes new endpoints for configuration

Breaking changes:

- `CoreApplicationOptions` is now removed:
  - Old `HealthChecks` options from `CoreServicesOptions` is removed, please register your custom checks by using services.AddHealthChecks() (idempotent method)
  - Old `HealthChecks` options from `CoreApplicationOptions` has been moved to the new `HealthChecks` options from `CoreServicesOptions`
  - Rename `Core:Application:HealthChecks:Path` configuration key to `Core:Services:HealthChecks:Path`
  - Rename `Core:Application:HealthChecks:Port` configuration key to `Core:Services:HealthChecks:Path`
- Namespaces have been merged for extension methods in ASP.NET Core 5.0 projects; you should now use `Milochau.Core.AspNetCore.Infrastructure.Extensions` namespace
- Many internal classes are not public anymore

Bug fixes:

- Add proper `AssemblyVersion` in DLLs

## Version 6.0.2

Bug fixes :

- Fix Kestrel configuration for ASP.NET Core 5.0

## Version 6.0.1

Bug fixes :

- Fix configuration providers order:
  - Azure App Configuration
  - Azure Key Vault
  - `appsettings.local.json`
  - Other Framework providers

## Version 6.0.0

Breaking changes:

- Default refresh period for Azure App Configuration is now set to 120 minutes (instead of 30 minutes before)
- `Milochau.Core` is now splitted in many libraries

You must reinstall this framework by following the steps in the `README.md` file. These packages are the new entry points for your applications:

- `Milochau.Core.AspNetCore` for ASP.NET Core 5.0 applications
- `Milochau.Core.Functions` for Azure Functions 3 applications

The features supported in the `Milochau.Core < 6.0` library are fully supported in the `Milochau.Core.AspNetCore` library.

## Version 5.0.0

Breaking changes:

- Now targetting .NET 5.0

## Version 4.0.1

Bug fixes:
- Azure App Configuration is now properly registred

## Version 4.0.0

Breaking changes:

- Update dependencies, you may need to update yours to avoid conflicts
- Remove obsolete methods

## Version 3.1.0

New features:

- Introduces `TCoreOptions` instead of `CoreOptions` to help clients extend core options registered by Milochau.Core

## Version 3.0.0

New features:

- Configure `IOptions<CoreOptions>` with Dependency Injection

Bug fixes:

- Use 30 minutes instead of 5 minutes as default refresh, feature flags expiration

Breaking changes:

- Azure App Configuration keys must now be prefixed with `[Core:Application:Namespace]/` instead of `[Core:Application:Namespace]:`

## Version 2.0.0

New features:

- Connect to Azure Key Vault as a new direct configuration provider
- Adds new settings to describe application: see `Core:Application` options
- Use key filters for Azure App Configuration, to get keys prefixed with `Shared` and `[Core:Application:Namespace]` only

Breaking changes:

- Secret settings in Azure App Configuration are no longer supported; use direct Azure Key Vault connection instead
- Configuration retrieved from Azure App Configuration is now key-filtered: you should prefix your configuration keys
- Labels are now always enabled: remove the `Core:Labels:Enabled` key
- You must define your application configuration namespace, with `Core:Application:Namespace` key

## Version 1.1.0

New features:

- Use `DefaultAzureCredential` instead of `ManagedIdentityCredential` for Configuration
- Adds new settings for `DefaultAzureCredential`: see `Core:Credential` options

## Version 1.0.1

New features:

- **Configuration**: adds support for Azure App Configuration (with Refresh and Feature Flags), Azure Key Vault
- **HealthChecks**: adds a very basic health check
- **Telemetry**: adds Application Insights support
