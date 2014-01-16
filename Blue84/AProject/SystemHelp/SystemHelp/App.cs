using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemHelp
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
                IsInEmbededDevice = true;
            }
            IsTopMost = reader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
        private Config.ConfigReader reader = new Config.ConfigReader(ConfigPath);
        public bool IsInEmbededDevice = false;
        public bool IsTopMost = false;
        public static string ConfigPath = "HelpConfig.xml";
        public static ViewModel ViewModel = new ViewModel();
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
