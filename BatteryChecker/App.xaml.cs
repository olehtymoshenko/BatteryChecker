using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Diagnostics;

namespace BatteryChecker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            app.Startup += App_Startup;
            app.Run();

        }

        private static void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
