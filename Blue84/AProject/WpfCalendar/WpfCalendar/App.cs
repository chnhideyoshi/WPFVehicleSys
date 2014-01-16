using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WpfCalendar
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
            #region settings
            InitAppSettings();
            #endregion
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }

        private void InitAppSettings()
        {
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
            }
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
        }
        
        #region settings
        public bool IsInEmbededDevice = false;
        public bool IsTopMost=true;
        #endregion
        #region private
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
        private Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        private static readonly string ConfigPath = "CalendarConfig.xml";
        #endregion
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
