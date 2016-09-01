# AspNet Core Lifecycle

| | |
|---|----|
|   CI  |[![Build status](https://ci.appveyor.com/api/projects/status/05drr0dq7omoru07?svg=true)](https://ci.appveyor.com/project/dburriss/aspnetlifecycle) |
| MASTER |[![Master Build status](https://ci.appveyor.com/api/projects/status/pmgou6qm452s50d0/branch/master?svg=true)](https://ci.appveyor.com/project/dburriss/aspnetlifecycle/branch/master) |
|BLEEDING|[![MyGet CI](https://img.shields.io/myget/dburriss-ci/vpre/Lifecycle.AspNetCore.svg)](https://www.myget.org/feed/dburriss-ci/package/nuget/Lifecycle.AspNetCore) |
|  NUGET |[![NuGet CI](https://img.shields.io/nuget/v/Lifecycle.AspNetCore.svg)](https://www.nuget.org/packages/Lifecycle.AspNetCore/) |

I help execute custom commands at key interception points within the OWIN request lifecycle. As an example you can trigger code to fire before a request or after a response by implementing a a particular interface.

Based on code [presented on Pluralsight](https://www.pluralsight.com/courses/build-application-framework-aspdotnet-mvc-5) by [@matthoneycutt](https://twitter.com/matthoneycutt). He blogs at http://trycatchfail.com/blog.

## Usage

Install with the following nuget command:
> `Install-Package Lifecycle.AspNetCore

Implement at least one of the `IRun` *interfaces*.

```csharp
public class TestRequestStartTask : IRunOnEachRequest
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Request Start: " + DateTime.UtcNow);
    }
}
```

In your Startup.cs file wire up the inplementing classes into the DI container using the `AddLifecycleCommands` extension method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var assembliesToInspect = GetAssemblies();//implement GetAssemblies to return assemblies you want to scan for interfaces
    services.AddLifecycleCommands(assembliesToInspect);

    services.AddMvc();
}
```

Then in the app configuration tell the app to use the **Lifecycle** middleware using the `UseLifecycleCommands` extension method.

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseLifecycleCommands();

    app.UseMvc()
}
```

## Available interfaces

```csharp
public class AppInitTask : IRunAtInit
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("App Init: " + DateTime.UtcNow);
    }
}

public class AppStartTask : IRunAtStartup
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("App Start: " + DateTime.UtcNow);
    }
}

public class ErrorTask : IRunOnError
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Error: " + DateTime.UtcNow);
    }
}

public class TestRequestStartTask : IRunOnEachRequest
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Request Start: " + DateTime.UtcNow);
    }
}

public class TestRequestEndTask : IRunAfterEachRequest
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Request End: " + DateTime.UtcNow);
    }
}

public class TestResponseStartTask : IRunOnEachResponse
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Response Start: " + DateTime.UtcNow);
    }
}

public class TestResponseEndTask : IRunAfterEachResponse
{
    public void Execute()
    {
        System.Diagnostics.Debug.WriteLine("Response End: " + DateTime.UtcNow);
    }
}
```