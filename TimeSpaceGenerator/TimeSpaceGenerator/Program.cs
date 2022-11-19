using System;
using System.Windows.Forms;
using TimeSpaceGenerator.Managers;

namespace TimeSpaceGenerator
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ConststringManager.ImportStrings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}