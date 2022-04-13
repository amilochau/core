# Functionalities

## Introduction

This document lists functionalities offered by `Milochau.Core.*` enterprise libraries.

## Features availability

The table below help you see if a feature is included in a dedicated package. Note that some features are not available yet.

| Feature                                                       | `*.AspNetCore` | `*.Functions` | `*.Console` | Notes |
| -------                                                       | -------------- | ------------- | ----------- | ----- |
| **Configuration**
| Connects to Azure Key Vault                                   | X | | |
| Connects to `appsettings.{host}.json`                         | X | X | X |
| Connects to `secrets.json`                                    | X | | |
| Refreshes configuration after X minutes                       | X | X | X |
| Provides configuration providers endpoints                    | X | X | |
| Provides configuration refresh endpoints                      | | | |
| **Health checks**
| Registers health checks                                       | X | X | |
| Adds default `Endpoint` check                                 | X | X | |
| Adds Application Host Environment check                       | X | X | |
| Provides all health checks endpoints                          | X | X | |
| Provides light health checks endpoints                        | X | X | |
| **Monitoring**
| Registers Application Insights                                | X | | |
| Manage Application Insights sampling options                  | X | | |
| **Cache**
| Registers local cache implementation                          | X | X | X |
| Provides local cache management endpoints                     | X | X | |
| **Application Information**
| Provides Assembly Information endpoints                       | X | X | |
| **API Documentation**
| Provides OpenAPI endpoints                                    | | | | |

The following abstractions are available from the `Milochau.Core.Abstractions` libraries. You can safely reference this package from any application tier.

| Abstraction | Description |
| ----------- | ----------- |
| `IOptions<CoreHostOptions>` | Core host options, containing application name, environment name, etc |
| `IApplicationHostEnvironment` | Provides application description, as described in the section below |
| `IApplicationMemoryCache` | Provides simple local cache service |

## Application

You must initialize your application with few settings. These settings are used by `Milochau.Core` to properly connect to shared resources as Azure services.

These configuration keys must be set up:

| Key | Description | Example values | Default value |
| --- | ----------- | -------------- | ------------- |
| `DOTNET_ORGANIZATION`, `ASPNETCORE_ORGANIZATION`, `AZURE_FUNCTIONS_ORGANIZATION` | Defines the organization name |
| `DOTNET_APPLICATION`, `ASPNETCORE_APPLICATION`, `AZURE_FUNCTIONS_APPLICATION` | Defines the application name |
| `DOTNET_ENVIRONMENT`, `ASPNETCORE_ENVIRONMENT`, `AZURE_FUNCTIONS_ENVIRONMENT` | Defines the environment name | `Development`, `Production` | `Development` |
| `DOTNET_HOST`, `ASPNETCORE_HOST`, `AZURE_FUNCTIONS_HOST` | Defines the host name | `dev2`, `prd` | `local` |
| `DOTNET_REGION`, `ASPNETCORE_REGION`, `AZURE_FUNCTIONS_REGION` | Defines the Azure region name | `ew1`, `en1` |

## Configuration

`Milochau.Core` references **Azure Key Vault**.

### Key Vault

Configuration can be automatically injected from Azure Key Vault on startup. To use Azure Key Vault, please enable it from configuration, with a `Vault` definition:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `DOTNET_KEYVAULT_VAULT`, `ASPNETCORE_KEYVAULT_VAULT`, `AZURE_FUNCTIONS_KEYVAULT_VAULT` | URI of the Azure Key Vault to use | `https://XXXX.vault.azure.net` |

## Health checks

Default plain health checks are automatically configured by `Milochau.Core`.

The following endpoints are exposed to get health states:

- `GET /api/health` to get the health with all the checks
- `GET /api/health/light` to get the health with the checks flagged with the `light` tag

## Telemetry

Telemetry uses Application Insights to collect requests, dependencies, exceptions, performance counters, heartbeats, and logs from your ASP.NET Core application.

To activate telemetry, please enable it from configuration, and set up an instrumentation key.

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `ApplicationInsights:InstrumentationKey` (x) | Application Insights instrumentation key | `aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee` | `null` (Feature is not set) |
| `Core:Services:Telemetry:Enabled` | Enable Telemetry with Application Insights | `true` | `false` (Feature is disabled) |
| `Core:Services:Telemetry:DisableAdaptiveSampling` | Disable Adaptive Sampling | `true` | `false` |

## Localization

Request localization let you define the culture that is used from your requests.

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Services:RequestLocalization:Enabled` | Enable request localization | `true` | `false` |
| `Core:Services:RequestLocalization:DefaultCulture` | Default culture for request localization | `en-US` |
| `Core:Services:RequestLocalization:SupportedCultures` | Supported (UI) cultures | `:0` to `en-US` |

## Azure credential

You can set up specific fields to help you application connect to Azure services. The following properties from configuration are used:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Host:Credential` | `DefaultAzureCredentialOptions` used to connect to Azure services | | `new DefaultAzureCredentialOptions()` |

## Cache

You can use the `IApplicationMemoryCache` as a ready-to-use local cache; see the interface to learn more.
