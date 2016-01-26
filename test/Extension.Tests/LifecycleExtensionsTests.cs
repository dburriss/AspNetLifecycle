using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestAssembly1;

namespace Extension.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class LifecycleExtensionsTests
    {
        public void AddLifecycleCommands_On()
        {
            IServiceCollection services = new ServiceCollection();
        }

        private System.Reflection.Assembly[] GetAssemblies()
        {
#if NET451
            return new System.Reflection.Assembly[] { typeof(BeforeRequest1).Assembly };
#endif

#if DNXCORE50
            return new System.Reflection.Assembly[] { typeof(BeforeRequest1).GetTypeInfo().Assembly };
#endif

#if DOTNET5_4
            return null;
#endif
        }
    }
}
