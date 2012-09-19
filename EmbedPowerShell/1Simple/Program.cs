using System;
using System.Management.Automation;

namespace Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            var powerShellResult = PowerShell
                .Create()
                .AddScript("Get-ChildItem")                
                .Invoke();

            foreach (var item in powerShellResult)
            {
                Console.WriteLine(item);
            }
        }
    }
}
