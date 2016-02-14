using Lifecycle.AspNet;
using Microsoft.Extensions.DependencyInjection;
using TestAssembly1;
using Xunit;

namespace Extension.Tests
{
    public class LifecycleExtensionsTests
    {
        [Fact]
        public void AddLifecycleCommands_OnAssemblyWithLifecycleContracts_RegistersCommands()
        {
            var assemblies = GetAssemblies();
            IServiceCollection services = new ServiceCollection();
            services.AddLifecycleCommands(assemblies);

            Assert.NotEmpty(services);
            Assert.Equal(2, services.Count);
        }

        [Fact(Skip = "need mocking")]
        public void UseLifecycleCommands_OnAssemblyWithLifecycleContracts_ExecutesStartupCommand()
        {
            //var assemblies = GetAssemblies();
            //IServiceCollection services = new ServiceCollection();
            //services.AddLifecycleCommands(assemblies);

            //IApplicationBuilder app = JustMock

            //Assert.NotEmpty(services);
            //Assert.Equal(2, services.Count);
            
        }

        private System.Reflection.Assembly[] GetAssemblies()
        {
            return new System.Reflection.Assembly[] { typeof(BeforeRequest1).Assembly };
        }
    }
}
