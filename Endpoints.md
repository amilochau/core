# Endpoints

## Introduction

This file lists all the public endpoints exposed when you reference `Milochau.Core.*` libraries.

## Endpoint categories

Endpoints proposed by `Milochau.Core` are groupped by categories.

| Category | Default endpoint path prefix | Comment |
| -------- | ---------------------------- | ------- |
| Application | `/api/system/application` | That exposes information on the application |
| Cache | `/api/system/cache` | That exposes management endpoints on caches |
| Configuration | `/api/system/configuration` | That exposes information on the configuration providers |
| Health checks | `/api/health` | That exposes information on the application health |

Note that these default endpoints path prefixes can be configured for ASP.NET Core 6.0 applications, not for Azure Functions. In Azure Functions, these endpoints do not include the `/api` prefix, while this prefix is included by default in the Microsoft template.

---

## Web applications endpoints (ASP.NET Core 6.0)

Here are all the endpoints exposed for Web applications.

| Category | Name | Default endpoint | Comment |
| -------- | ---- | ---------------- | ------- |
| Application | Assembly | `GET /api/system/application/assembly` | That exposes information on the entry assembly of the application |
| Application | Environment | `GET /api/system/application/environment` | That exposes information on the environment of the application |
| Cache | LocalCount | `GET /api/system/cache/local/count` | That returns the count of items in the application local cache |
| Cache | LocalContains | `GET /api/system/cache/local/contains?key=xxx` | That returns the existence of a list of items in the application local cache |
| Cache | LocalCompact | `POST /api/system/cache/local/compact?percentage=0.xx` | That compacts the application local cache by the defined percentage |
| Cache | LocalRemove | `POST /api/system/cache/local/remove?key=xxx` | That removes a list of items from the application local cache |
| Configuration | Flags | `GET /api/system/configuration/flags` | That returns the state of all feature flags (typically stored in Azure App Configuration) |
| Configuration | Providers | `GET /api/system/configuration/providers` | That returns the configuration providers |
| Health checks | Default | `GET /api/health` | That returns the health of the application with all checks defined |
| Health checks | Light | `GET /api/health/light` | That returns the health of the application with only the `light` checks |

---

## Functions endpoints (Azure Functions 4 / .NET 6.0 isolated process)

Here are all the endpoints exposed for Functions applications.

| Category | Name | Default endpoint | Comment |
| -------- | ---- | ---------------- | ------- |
| Application | Assembly | `GET /api/system/application/assembly` | That exposes information on the entry assembly of the application |
| Application | Environment | `GET /api/system/application/environment` | That exposes information on the environment of the application |
| Cache | LocalCount | `GET /api/system/cache/local/count` | That returns the count of items in the application local cache |
| Cache | LocalContains | `GET /api/system/cache/local/contains?key=xxx` | That returns the existence of a list of items in the application local cache |
| Cache | LocalCompact | `POST /api/system/cache/local/compact?percentage=0.xx` | That compacts the application local cache by the defined percentage |
| Cache | LocalRemove | `POST /api/system/cache/local/remove?key=xxx` | That removes a list of items from the application local cache |
| Configuration | Flags | `GET /api/system/configuration/flags` | That returns the state of all feature flags (typically stored in Azure App Configuration) |
| Configuration | Providers | `GET /api/system/configuration/providers` | That returns the configuration providers |
| Health checks | Default | `GET /api/health` | That returns the health of the application with all checks defined |
| Health checks | Light | `GET /api/health/light` | That returns the health of the application with only the `light` checks |
