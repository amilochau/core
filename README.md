# Readme - core

[![Build](https://github.com/amilochau/core/actions/workflows/build.yml/badge.svg)](https://github.com/amilochau/core/actions/workflows/build.yml)
[![Deploy libraries](https://github.com/amilochau/core/actions/workflows/deploy-libraries.yml/badge.svg)](https://github.com/amilochau/core/actions/workflows/deploy-libraries.yml)

## Introduction

`core` is a collection of libraries used as an enterprise framework, to improve applications and services with a large set of common features.

## What's new

You can find the new releases on the [GitHub releases page](https://github.com/amilochau/core/releases).

The current version of `Milochau.Core.*` packages only work with .NET 6.0. If you want to use these packages with .NET 5.0 or .NET Framework 4.7.2+, you should use [a version before v11, such as the v10.0.1](https://github.com/amilochau/core/tree/v10.0.1).

---

## Framework installation

`Milochau.Core.*` libraries can be used in any ASP.NET Core 6.0 / Azure Functions (.NET 6.0 isolated process) / Console applications (.NET 6.0) project. To use it, you must install the library specific to your technology as a NuGet package, then add framework references in main project files.

---

### Up-to-date web applications (ASP.NET Core 6.0)

*Up-to-date web applications* are new applications that use the most recent versions of Microsoft frameworks. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.AspNetCore.ReferenceProject` is an application written with ASP.NET Core 6.0 framework

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

### Functions applications (Azure Functions 4 / .NET 6.0 isolated process)

*Functions applications* are applications that use the most recent versions of Microsoft frameworks for Azure Functions applications. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.Functions.ReferenceProject` is an application written with Azure Functions 4 / .NET 6.0 isolated process framework

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

### Console applications (.NET 6.0)

*Console applications* are small scripting applications that use the most recent versions of Microsoft frameworks for .NET applications. One complete sample is proposed to help you interface these applications with Milochau.Core libraries:

- `Milochau.Core.Console.ReferenceProject` is an application written with .NET 6.0 framework

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
