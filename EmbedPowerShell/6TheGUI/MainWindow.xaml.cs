using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows;

namespace _6TheGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

}
