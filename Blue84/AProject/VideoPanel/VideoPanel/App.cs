using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VideoPanel
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
            #region GetSettings
            InitAppSettings();
            InitViewModel();
            #endregion
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }

        private static void InitViewModel()
        {
            DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            DefaultPath = System.IO.Path.GetDirectoryName(DefaultPath) + "\\" + "Videos";
            ViewModel = new ViewModel();
        }

        private void InitAppSettings()
        {
            string[] s = Environment.GetLogicalDrives(); if (s.Length < 4) { ShowCursor(0); IsInEmbededDevice = true; }
            ValidExtDirectoires = configReader.GetStringCollectionSettingItem("ValidExtDirectoires", new List<string>());
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            SupportedExtensionList = configReader.GetStringCollectionSettingItem("SupportedExtensionList", SupportedExtensionList);
            ThumbHeight = configReader.GetIntSettingItem("ThumbHeight", ThumbHeight);
            ThumbWidth = configReader.GetIntSettingItem("ThumbWidth", ThumbWidth);
            ThumbFramePositon = configReader.GetIntSettingItem("ThumbFramePositon", ThumbFramePositon);
            A = configReader.GetStringSettingItem("A",A);
        }
        Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        #region appSettings
        public bool IsInEmbededDevice = false;
        public List<string> ValidExtDirectoires;
        public bool IsTopMost = false;
        public int ThumbHeight = 150;
        public int ThumbWidth = 200;
        public int ThumbFramePositon = 3;
        public string A = "asd";
        public List<string> SupportedExtensionList = new List<string>() { ".WMV", ".AVI", ".MP4", ".RM", ".RMVB", ".FLV", "MKV", "MPG" };
        #endregion
        public static string DefaultPath;
        public static ViewModel ViewModel;
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
        private static readonly string ConfigPath = "VideoConfig.xml";

        
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
