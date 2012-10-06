using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace _3ExtensionMethod
{
    class Program
    {
        static void Main(string[] args)
        {
             // Adds a Print ext method

            "dir".ExecutePowerShell().Print();

            //var pi = "[Math]::Pi".ExecutePowerShell();
            //pi.Print();
        }
    }

    public static class PowerShellExtensions
    {
        public static PSObject ExecutePowerShell(this string script)
        {
            return PowerShell
                  .Create()
                  .AddScript(script)
                  .AddCommand("Out-String")
                  .Invoke()[0];
        }

        public static void Print(this PSObject target)
        {
            Console.WriteLine(target);
        }
    }
}