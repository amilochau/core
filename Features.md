# Functionalities

## Introduction

This document lists functionalities offered by `Milochau.Core.*` enterprise libraries.

## Features availability

The table below help you see if a feature is included in a dedicated package. Note that some features are not available yet.

| Feature                                                       | `*.AspNetCore` | `*.Functions` | Notes |
| -------                                                       | -------------- | ------------- | ----- |
| **Configuration**
| Connects to Azure App Configuration                           | X | X | Connection can be done with connection string or Managed Identity |
| Connects to Azure Key Vault                                   | X | X | Connection can be done with client secret or Managed Identity |
| Connects to `apssettings.local.json`                          | X | X |
| Connects to `secrets.json`                                    | X | |
| Refreshes configuration after X minutes                       | X | X |
| Uses Feature flags & Feature filters                          | X | X |
| Provides configuration providers endpoints                    | X | X |
| Provides configuration refresh endpoints                      | | |
| Provides Feature flags state endpoints                        | X | X |
| **Health checks**
| Registers health checks                                       | X | X |
| Adds default `Endpoint` check                                 | X | X |
| Adds Azure Key Vault check                                    | X | X |
| Provides all health checks endpoints                          | X | X |
| Provides light health checks endpoints                        | X | X |
| **Monitoring**
| Registers Application Insights                                | X | |
| Manage Application Insights sampling options                  | X | |
| **Cache**
| Registers local cache implementation                          | X | X |
| Provides local cache management endpoints                     | X | X |
| **Application Information**
| Provides Assembly Information endpoints                       | X | | Azure Functions can't expose assembly information |
| **API Documentation**
| Provides OpenAPI endpoints                                    | | | |

The following abstractions are available from the `Milochau.Core.Abstractions` libraries. You can safely reference this package from any application tier.

| Abstraction | Description |
| ----------- | ----------- |
| `IApplicationHostEnvironment` | Provides application description, as described in the section below |
| `IApplicationMemoryCache` | Provides simple local cache service |

## Application

You must initialize your application with few settings. These settings are used by `Milochau.Core` to properly connect to shared resources as Azure services.

These configuration keys must be set up:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Host:Application:ApplicationName` | Defines the application name, used to retrieve proper configuration from Azure App Configuration | `emails`, `milochau` | Get from `ASPNETCORE_APPLICATION` |
| `Core:Host:Application:EnvironmentName` | Defines the environment name, used to retrieve proper configuration from Azure App Configuration | `Development`, `Production` | Get from `ASPNETCORE_ENVIRONMENT` |
| `Core:Host:Application:HostName` | Defines the host name, used to retrieve proper configuration from Azure App Configuration | `dev2`, `prd` | Get from `ASPNETCORE_HOST` |
| `Core:Services:RequestLocalization:Enabled` | Enable request localization | `true` | `false` |
| `Core:Services:RequestLocalization:DefaultCulture` | Default culture for request localization | `en-US` |
| `Core:Services:RequestLocalization:SupportedCultures` | Supported (UI) cultures | `:0` to `en-US` |

## Configuration

`Milochau.Core` references **Azure App Configuration** and **Azure Key Vault**.

### App Configuration

Configuration can be automatically injected from Azure App Configuration on startup. To use Azure App Configuration, please enable it from configuration, at least with an `Endpoint`, or with a `ConnectionString`:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Host:AppConfig:Endpoint` (x) | Azure App Configuration endpoint | `Endpoint=https://XXXX.azconfig.io ;Id=XXXX;Secret=XXXX` | Get from `ASPNETCORE_APPCONFIG_ENDPOINT` |
| `Core:Host:AppConfig:ConnectionString` (x) | Azure App Configuration connection string | `https://XXXX.azconfig.io` | Get from `ASPNETCORE_APPCONFIG_CONNECTIONSTRING` |
| `Core:Host:AppConfig:SentinelKey` | Sentinel Key for Refresh with Azure App Configuration (the default namespace will be added as a prefix) | `Sentinel:Key` | `Sentinel:Key` |
| `Core:Host:AppConfig:RefreshExpirationInMinutes` | Expiration Refresh with Azure App Configuration (minutes) | `5` | `30` |

(x) `Core:AppConfig:ConnectionString` is only used if no `Core:AppConfig:Endpoint` is defined; permissions need to be granted to use `Core:AppConfig:Endpoint`

### Key Vault

Configuration can be automatically injected from Azure Key Vault on startup. To use Azure Key Vault, please enable it from configuration, with a `Vault` definition:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Host:KeyVault:Vault` | URI of the Azure Key Vault to use | `https://XXXX.vault.azure.net` | Get from `ASPNETCORE_KEYVAULT_VAULT` |
| `Core:Host:KeyVault:TenantId` | URI of the Azure Key Vault to use | `https://XXXX.vault.azure.net` | Get from `ASPNETCORE_KEYVAULT_TENANTID` |
| `Core:Host:KeyVault:ClientId` | Client ID of the Azure Key Vault to use | | Get from `ASPNETCORE_KEYVAULT_CLIENTID` |
| `Core:Host:KeyVault:ClientSecret` | Client secret the Azure Key Vault to use | | Get from `ASPNETCORE_KEYVAULT_CLIENTSECRET` |

### Feature Flags

Feature Flags allow you to centralize the activation of your features, from Azure App Configuration. Feature Flags is injected by Milochau.Core framework. You can then use Feature Flags as described [here](https://docs.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core).

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

## Azure credential

You can set up specific fields to help you application connect to Azure services. The following properties from configuration are used:

| Key | Description | Example value | Default value |
| --- | ----------- | ------------- | ------------- |
| `Core:Host:Credential` | `DefaultAzureCredentialOptions` used to connect to Azure App Configuration, Azure Key Vault | | `new DefaultAzureCredentialOptions()` |

## Cache

You can use the `IApplicationMemoryCache` as a ready-to-use local cache; see the interface to learn more.
