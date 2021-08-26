# Readme - core

[![Build](https://github.com/amilochau/core/actions/workflows/build.yml/badge.svg)](https://github.com/amilochau/core/actions/workflows/build.yml)
[![Deploy libraries](https://github.com/amilochau/core/actions/workflows/deploy-libraries.yml/badge.svg)](https://github.com/amilochau/core/actions/workflows/deploy-libraries.yml)

## Introduction

`core` is a collection of libraries used as an enterprise framework, to improve applications and services with a large set of commun features.

## Getting Started

1. Installation process
From your local computer, clone the repository.

- dotnet restore
- dotnet run

2. Integration process
Please follow the development good practices, then follow the integration process.

---

## Framework installation

`Milochau.Core.*` libraries can be used in any ASP.NET Core 5.0 / Azure Functions (.NET 5.0 isolated process) / Console applications (.NET 5.0) project. To use it, you must install the library specific to your technology as a NuGet package, then add framework references in main project files.

---

### Up-to-date web applications (ASP.NET Core 5.0)

*Up-to-date web applications* are new applications that use the most recent versions of Microsoft frameworks. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.AspnetCore.ReferenceProject` is an application written with ASP.NET Core 5.0 framework

Up-to-date web applications must install the `Milochau.Core.AspNetCore` package:

```ps
Install-Package Milochau.Core.AspNetCore
```

You can then initialize your application with the following classes.

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureCoreHostBuilder<Startup>();
}
```

```csharp
public class Startup : CoreApplicationStartup
{
    public Startup(IConfiguration configuration) : base(configuration) { }

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services); // <== Here you configure Milochau.Core features

        // Configure more services: Razor Pages...
    }

    public override void Configure(IApplicationBuilder app)
    {
        base.Configure(app, env); // <== Here you configure application to use Milochau.Core features
        
        // Configure more middlewares here: authentication, etc...

        app.UseRouting();
        app.UseEndpoints(endpoint =>
        {
            // Map Milochau.Core endpoints
            endpoints.MapCoreHealthEndpoints();
            endpoints.MapCoreSystemEndpoints();
        });
    }
}
```

---

### Functions applications (Azure Functions 3 / .NET 5.0 isolated process)

*Functions applications* are applications that use the most recent versions of Microsoft frameworks for Azure Functions applications. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.Functions.ReferenceProject` is an application written with Azure Functions 3 / .NET 5.0 isolated process framework

Functions applications must install the `Milochau.Core.Functions` package:

```ps
Install-Package Milochau.Core.Functions
```

You can then initialize your application with the following classes.

```csharp
public static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    public static IHostBuilder CreateHostBuilder() =>
        new HostBuilder()
            .ConfigureCoreConfiguration()
            .ConfigureCoreHostBuilder<Startup>();
}
```

```csharp
public class Startup : CoreFunctionsStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        // Register options, services, data access...
    }
}
```

In order to add system endpoints (see the details features to learn more), you must add the following `ItemGroup` in your Azure Functions project file:

```xml
  <PropertyGroup>
    <FunctionsInDependencies>true</FunctionsInDependencies>
  </PropertyGroup>
```

---

### Console applications (.NET 5.0)

*Console applications* are small scripting applications that use the most recent versions of Microsoft frameworks for .NET applications. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.Console.ReferenceProject` is an application written with .NET 5.0 framework

| ⚠ `Milochau.Core.Console` is still in development. Even if the first stable version is release, it may contain bugs. Please report them as issues :)

Console applications must install the `Milochau.Core.Console` package:

```ps
Install-Package Milochau.Core.Console
```

You can then initialize your application with the following classes.

```csharp
public static class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureCoreHostBuilder<Startup, EntryPoint>()
            .RunConsoleAsync();
    }
}

public class Startup : CoreConsoleStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        // Register options, services, data access...
    }
}

public class EntryPoint : CoreConsoleEntryPoint
{
    public override Task<int> RunAsync(CancellationToken cancellationToken)
    {
        // Do what you want to execute...

        return Task.FromResult(0);
    }
}
```
