using System;
using System.Management.Automation;

namespace _4ExtensionReadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteFile();
            //ConvertToJson();
        }

        private static void ConvertToJson()
        {
            var script = @"
[PSCustomObject]@{
    Person1 = @{
        Name = 'John'
        Age  = 10
    }

    Person2 = @{
        Name = 'Tom'
        Age  = 20
    }

} | ConvertTo-Json
";
            script.ExecutePowerShell().Print();
        }

        private static void ExecuteFile()
        {
            "test.ps1".ExecutePowerShell().Print();
        }
    }

    public static class PowerShellExtensions
    {
        public static PSObject ExecutePowerShell(this string script)
        {
            if (System.IO.File.Exists(script))
            {
                script = System.IO.File.ReadAllText(script);
            }

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
