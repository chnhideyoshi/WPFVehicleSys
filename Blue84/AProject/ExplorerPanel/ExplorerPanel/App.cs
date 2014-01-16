using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ExplorerPanel
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
            ViewModel = new ViewModel();
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
                IsInEmbededDevice = true;
            }
            ShowOnlySupportedFile = configReader.GetBooleanSettingItem("ShowOnlySupportedFile", ShowOnlySupportedFile);
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            SupportedMusicExtensionList = configReader.GetStringCollectionSettingItem("SupportedMusicExtensionList", SupportedMusicExtensionList);
            SupportedVideoExtensionList = configReader.GetStringCollectionSettingItem("SupportedVideoExtensionList", SupportedVideoExtensionList);
            SupportedPictureExtensionList = configReader.GetStringCollectionSettingItem("SupportedPictureExtensionList", SupportedPictureExtensionList);
            SupportedBookExtensionList = configReader.GetStringCollectionSettingItem("SupportedBookExtensionList", SupportedBookExtensionList);
        }
        
        #region Settings
        public bool IsTopMost = false;
        public bool IsInEmbededDevice = false;
        public bool ShowOnlySupportedFile = false;
        public List<string> SupportedPictureExtensionList = new List<string>() { ".JPG", ".JPEG", ".PNG", ".BMP", ".GIF" };
        public List<string> SupportedVideoExtensionList = new List<string>() { ".WMV", ".AVI", ".MP4", ".RM", ".RMVB", ".FLV", "MKV", "MPG" };
        public List<string> SupportedMusicExtensionList = new List<string>() {".WMA",".MP3" };
        public List<string> SupportedBookExtensionList = new List<string>() { ".TXT" };
        #endregion
        Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        private static string ConfigPath = "ExplorerConfig.xml";
        public static ViewModel ViewModel; 
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
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
