using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lifecycle.OWIN
{
    public static class LifecycleExtensions
    {
        public static void AddLifecycleCommands(this IServiceCollection services, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var registrations =
                from type in assembly.GetExportedTypes()
                where type.GetInterfaces().Any(
                        t => t == typeof(IRunAtInit)
                        || t == typeof(IRunAtStartup)
                        || t == typeof(IRunOnEachRequest)
                        || t == typeof(IRunOnError)
                        || t == typeof(IRunAfterEachRequest)
                        || t == typeof(IRunOnEachResponse)
                        || t == typeof(IRunAfterEachResponse)
                    )
                select type;

                var regList = registrations.ToList();

                RegisterInitTasks(services, regList);
                RegisterStartTasks(services, regList);
                RegisterOnEachRequestTasks(services, regList);
                RegisterAfterEachRequestTasks(services, regList);
                RegisterOnEachResponseTasks(services, regList);
                RegisterAfterEachResponseTasks(services, regList);
                RegisterOnErrorTasks(services, regList);
            }
        }

        private static void RegisterInitTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunAtInit)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunAtInit>(f => f.GetService(impl) as IRunAtInit);
            }
        }

        private static void RegisterStartTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunAtStartup)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunAtStartup>(f => f.GetService(impl) as IRunAtStartup);
            }
        }

        private static void RegisterOnEachRequestTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunOnEachRequest)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunOnEachRequest>(f => f.GetService(impl) as IRunOnEachRequest);
            }
        }

        private static void RegisterAfterEachRequestTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunAfterEachRequest)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunAfterEachRequest>(f => f.GetService(impl) as IRunAfterEachRequest);
            }
        }


        private static void RegisterOnEachResponseTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunOnEachResponse)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunOnEachResponse>(f => f.GetService(impl) as IRunOnEachResponse);
            }
        }

        private static void RegisterAfterEachResponseTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunAfterEachResponse)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunAfterEachResponse>(f => f.GetService(impl) as IRunAfterEachResponse);
            }
        }

        private static void RegisterOnErrorTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var implementations = allTypes.Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type });
            var implementingTypes = implementations.Where(t => t.Service == typeof(IRunOnError)).Select(x => x.Implementation);
            foreach (var impl in implementingTypes)
            {
                services.AddTransient(impl);
                services.AddTransient<IRunOnError>(f => f.GetService(impl) as IRunOnError);
            }
        }

        public static void UseLifecycleCommands(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            Init(serviceProvider);
            StartUp(serviceProvider);
            app.UseMiddleware<LifecycleMiddleware>();
        }

        private static void Init(IServiceProvider serviceProvider)
        {
            var inits = serviceProvider.GetServices<IRunAtInit>();
            foreach (var cmd in inits)
            {
                cmd.Execute();
            }
        }

        private static void StartUp(IServiceProvider serviceProvider)
        {
            var starts = serviceProvider.GetServices<IRunAtStartup>();
            foreach (var cmd in starts)
            {
                cmd.Execute();
            }
        }

    }
}
