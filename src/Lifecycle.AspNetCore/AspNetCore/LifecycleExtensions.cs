using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Lifecycle.AspNetCore
{
    public static class LifecycleExtensions
    {
        /// <summary>
        /// I register all the Lifecycle commands in the given assemblies.
        /// </summary>
        /// <param name="services">AspNet service collection</param>
        /// <param name="assemblies">Assemblies to scan for Lifecycle contracts.</param>
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
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunAtInit));

            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        private static void RegisterStartTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunAtStartup));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        private static void RegisterOnEachRequestTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunOnEachRequest));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        private static void RegisterAfterEachRequestTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunAfterEachRequest));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }


        private static void RegisterOnEachResponseTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunOnEachResponse));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        private static void RegisterAfterEachResponseTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunAfterEachResponse));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        private static void RegisterOnErrorTasks(IServiceCollection services, IEnumerable<Type> allTypes)
        {
            var matches = allTypes
                .Select(type => new { Service = type.GetInterfaces().Single(x => x.Name.StartsWith("IRun")), Implementation = type })
                .Where(x => x.Service == typeof(IRunOnError));
            foreach (var match in matches)
            {
                services.AddTransient(match.Service, match.Implementation);
            }
        }

        /// <summary>
        /// I hookup the Lifecycle OWIN Middleware. Also fires Init and then Startup commands.
        /// </summary>
        /// <param name="app"></param>
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
