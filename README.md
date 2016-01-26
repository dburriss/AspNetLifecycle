# AspNetLifecycle
I help execute custom commands at key interception points within the OWIN request lifecycle. As an example you can trigger code to fire before a request or after a response by implementing a a particular interface.

## Usage

Implement at least one of the `IRun` *interfaces*.

    public class TestRequestStartTask : IRunOnEachRequest
    {
        public void Execute()
        {
            System.Diagnostics.Debug.WriteLine("Request Start: " + DateTime.UtcNow);
        }
    }

In your Startup.cs file wire up the inplementing classes into the DI container using the `AddLifecycleCommands` extension method.

    public void ConfigureServices(IServiceCollection services)
    {
        var assembliesToInspect = GetAssemblies();//implement GetAssemblies to return assemblies you want to scan for interfaces
        services.AddLifecycleCommands(assembliesToInspect);

        services.AddMvc();
    }
    
Then in the app configuration tell the app to use the **Lifecycle** middleware using the `UseLifecycleCommands` extension method.

    public void Configure(IApplicationBuilder app)
    {
        app.UseLifecycleCommands();

        app.UseMvc()
    }
    
# Available interfaces


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

