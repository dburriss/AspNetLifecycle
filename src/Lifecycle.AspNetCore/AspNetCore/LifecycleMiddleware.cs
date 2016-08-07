using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Lifecycle.AspNetCore
{
    public class LifecycleMiddleware
    {
        private readonly RequestDelegate next;

        public LifecycleMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            #region IRunOnEachRequest
            var befores = serviceProvider.GetServices<IRunOnEachRequest>();
            foreach (var cmd in befores)
            {
                cmd.Execute();
            }
            #endregion

            #region IRunOnEachResponse
            Func<object, Task> iRunOnResponseStarted = ctx => RunWhenReponseStarts(serviceProvider);
            context.Response.OnStarting(iRunOnResponseStarted, context);
            #endregion

            #region IRunAfterEachResponse
            Func<object, Task> iRunOnResponseCompleted = ctx => RunWhenReponseCompletes(serviceProvider);
            context.Response.OnCompleted(iRunOnResponseCompleted, context);
            #endregion

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                #region IRunOnError
                var errorCmds = serviceProvider.GetServices<IRunOnError>();
                foreach (var cmd in befores)
                {
                    cmd.Execute();
                }
                #endregion

                throw;
            }
            #region IRunAfterEachRequest
            var afters = serviceProvider.GetServices<IRunAfterEachRequest>();
            foreach (var cmd in afters)
            {
                cmd.Execute();
            }
            #endregion
        }

        private async Task RunWhenReponseStarts(object serviceProviderObject)
        {
            IServiceProvider serviceProvider = serviceProviderObject as IServiceProvider;
            var afters = serviceProvider.GetServices<IRunOnEachResponse>();
            foreach (var cmd in afters)
            {
                cmd.Execute();
            }
        }

        private async Task RunWhenReponseCompletes(object serviceProviderObject)
        {
            IServiceProvider serviceProvider = serviceProviderObject as IServiceProvider;
            var afters = serviceProvider.GetServices<IRunAfterEachResponse>();
            foreach (var cmd in afters)
            {
                cmd.Execute();
            }

        }
    }
}
