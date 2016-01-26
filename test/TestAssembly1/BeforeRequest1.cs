namespace TestAssembly1
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class BeforeRequest1 : Lifecycle.IRunOnEachRequest
    {
        public void Execute()
        {
            var t = this.GetType();
            if(Counter.ExecuteCount.ContainsKey(t))
            {
                var v = Counter.ExecuteCount[t] + 1;
                Counter.ExecuteCount[t] = v;
            }
            else
            {
                Counter.ExecuteCount[t] = 1;
            }                
        }
    }
}
