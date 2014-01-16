using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Settings
{
    public class SingleWrapper : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        public SingleWrapper()
        {
            this.IsSingleInstance = true;
        }
        private App app;
        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {
            app = new App();
            app.Run();
            return false;
        }
        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            app.MainWindow.Activate();
            return;
        }
    }
    public class App : System.Windows.Application
    {
        public App()
            : base()
        {
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
            }
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
        public bool IsTopMost = false;
        //public static ViewModel ViewModel = new ViewModel();
        Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        private static string ConfigPath = "SettingsConfig.xml";
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
    }
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleWrapper sw = new SingleWrapper();
            sw.Run(args);
        }
    }
}

