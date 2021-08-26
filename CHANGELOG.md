[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- `Milochau.Core.Functions` now references .NET 5.0 isolated process. You need to migrate your application to .NET 5.0 isolated process to benefic from the last features of Milochau.Core.* libraries

## New features

- Host configuration is now read with `AZURE_FUNCTIONS_` prefix, before trying with `DOTNET_` prefix

### Migration guide

Change your `.csproj` file:

- Change the `TargetFramework` to `net5.0`
- Add an `OutputType` as `Exe`
- Do not reference `Microsoft.NET.Sdk.Functions` anymore, reference `Microsoft.Azure.Functions.Worker.Sdk` instead (with `OutputItemType="Analyzer"`)
- Upgrade `Milochau.Core.Functions` to version 10+

Every Functions class must be changed:

- `FunctionName` attribute must be replaced by `Function` attribute
- HTTP triggered Function must use `HttpRequestData` as parameter, and return a `HttpResponseData` as response
- Cancellation tokens should not be passed in Function arguments anymore
- Use `request.CreateResponse()`, then `response.WriteAsJsonAsync()` methods to create an HTTP response

A new `Program.cs` file must be added; use this pattern:

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

Adapt the `Startup.cs` file:

- Do not reference the `FunctionsStartup` as `assembly` attribute
- `ConfigureServices` is not a public method, don't have an `IConfigure` parameter anymore, and needs to reference base implementation

Adapt the `local.settings.json` file:

- The `FUNCTIONS_WORKER_RUNTIME` setting value must be set to `dotnet-isolated`
