using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PictureBrowserPanel
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
            InitViewModel();
            #endregion
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }

        private static void InitViewModel()
        {
            DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ViewModel = new ViewModel();
        }
        private void InitAppSettings()
        {
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
            }
            ValidExtDirectoires = configReader.GetStringCollectionSettingItem("ValidExtDirectoires", new List<string>());
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            SupportedExtensionList = configReader.GetStringCollectionSettingItem("SupportedExtensionList", SupportedExtensionList);
            ThumbHeight = configReader.GetIntSettingItem("ThumbHeight", ThumbHeight);
            ThumbWidth = configReader.GetIntSettingItem("ThumbWidth", ThumbWidth);
            SmallHeight = configReader.GetIntSettingItem("SmallHeight", SmallHeight);
            SmallWidth = configReader.GetIntSettingItem("SmallWidth", SmallWidth);
        }
        
        #region settings
        public bool IsInEmbededDevice = false;
        public List<string> ValidExtDirectoires;
        public bool IsTopMost=true;
        public int ThumbHeight = 150;
        public int ThumbWidth = 150;
        public int SmallHeight = 480;
        public int SmallWidth = 800;
        public List<string> SupportedExtensionList = new List<string>() { ".JPG", ".JPEG", ".PNG", ".BMP", ".GIF" };//默认支持的格式，可在配置中修改
        #endregion
        public static string DefaultPath;
        public static ViewModel ViewModel;
        #region private
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
        private Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        private static readonly string ConfigPath = "PictureConfig.xml";
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
