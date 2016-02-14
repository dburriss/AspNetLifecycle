namespace TestAssembly1
{
    public class OnStartup : Lifecycle.IRunAtStartup
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
