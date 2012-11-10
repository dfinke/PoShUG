using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows;
using System.Windows.Input;

namespace _6TheGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lastCaretIndex;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.Focus();
            DisplayPrompt();

            PowerShellExtensions.AddVariable("window", this);
            PowerShellExtensions.AddVariable("psc", this.Console);
        }

        private void Console_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:

                    InvokeScript();
                    DisplayPrompt();
                    e.Handled = true;

                    break;
            }
        }

        private void InvokeScript()
        {
            var script = Console.Text.Substring(lastCaretIndex);

            switch (script.Trim().ToLower())
            {
                case "cls":
                    Console.Text = "";
                    break;
                case "exit":
                    this.Close();
                    break;
                default:
                    Console.Text += script.ExecutePowerShell();
                    break;
            }
        }

        private void DisplayPrompt()
        {
            Console.Text += "> ";
            Console.CaretIndex = Console.Text.Length;
            lastCaretIndex = Console.CaretIndex;
        }
    }

    #region PowerShellExtensions
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
                    rs.ThreadOptions = PSThreadOptions.UseCurrentThread;
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
    #endregion
}
