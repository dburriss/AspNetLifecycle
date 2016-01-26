using System;
using System.Collections.Generic;

namespace TestAssembly1
{
    public static class Counter
    {
        public static IDictionary<Type, int> ExecuteCount = new Dictionary<Type, int>();
    }
}
