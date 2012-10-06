using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.Generic;

namespace _5RunSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uses a runspace to add .NET objects as variables
            // Adds a variable named Person
            AddPersonVariable();

            "$Person".ExecutePowerShell().Print();
            //"$Person | ConvertTo-Json".ExecutePowerShell().Print();

            // Adds a variable named People            
            AddListOfPeople();
            
            //"$People".ExecutePowerShell()
            //    .Print();

            //AddListOfPeople();
            //"$People | ConvertTo-Json".ExecutePowerShell()
            //    .Print();
        }

        private static void AddListOfPeople()
        {
            var people = new List<Person>
            {
                new Person { Name = "John", Age = 10, Pets = new List<string> { "dog", "cat" } },
                new Person { Name = "Jane", Age = 20, Pets = new List<string> { "bird", "snake" } },
                new Person { Name = "Tom", Age = 30, Pets = new List<string> { "fish", "hamster" } }
            };


            // 'Inject' the object into PowerShell
            PowerShellExtensions.AddVariable("People", people);

        }

        private static void AddPersonVariable()
        {
            var p = new Person
            {
                Name = "John",
                Age = 10,
                Pets = new List<string> { "dog", "cat" }
            };

            // 'Inject' the object into PowerShell
            PowerShellExtensions
                .AddVariable("Person", p);
        }
    }

    public static class PowerShellExtensions
    {
        static Runspace rs;

        static Runspace TheRunSpace
        {
            get
            {
                if (rs == null)
                {
                    rs = RunspaceFactory.CreateRunspace();
                    rs.Open();
                }

                return rs;
            }
        }

        public static void AddVariable(string name, object value)
        {
            TheRunSpace.SessionStateProxy.SetVariable(name, value);
        }

        public static PSObject ExecutePowerShell(this string script)
        {
            if (System.IO.File.Exists(script))
            {
                script = System.IO.File.ReadAllText(script);
            }

            var ps = PowerShell.Create();
            ps.Runspace = TheRunSpace;

            return ps
                .AddScript(script)
                .AddCommand("Out-String")
                .Invoke()[0];
        }

        public static void Print(this PSObject target)
        {
            Console.WriteLine(target);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Pets { get; set; }
    }
}
