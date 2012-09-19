using System;
using System.Management.Automation;

namespace SimpleWithFormatting
{
    class Program
    {
        static void Main(string[] args)
        {
            // Adds the command Out-String
            // Works with args

            var script = "Get-ChildItem";
            if (args.Length > 0)
            {
                script = args[0];
            }

            var powerShellResult = PowerShell
                .Create()
                .AddScript(script)
                .AddCommand("Out-String")
                .Invoke();

            foreach (var item in powerShellResult)
            {
                Console.WriteLine(item);
            }

        }
    }
}
