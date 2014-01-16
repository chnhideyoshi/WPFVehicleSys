using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BookReader
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
            #region initSettings
            InitAppSettings();
            InitViewModel();
            #endregion
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
        private static void InitViewModel()
        {
            DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ViewModel = new ViewModel();
        }
        private void InitAppSettings()
        {
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
                IsInEmbededDevice = true;
            }
            IsEnableSpeech = configReader.GetBooleanSettingItem("IsEnableSpeech", IsEnableSpeech);
            ValidExtDirectoires = configReader.GetStringCollectionSettingItem("ValidExtDirectoires", new List<string>());
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            FontSize = configReader.GetIntSettingItem("FontSize", FontSize);
        }
        public static string DefaultPath;
        public static ViewModel ViewModel;
        #region Settings
        public int FontSize = 21;
        public bool IsInEmbededDevice = false;
        public bool IsEnableSpeech = false;
        public bool IsTopMost = false;
        public List<string> ValidExtDirectoires;
        #endregion
        public void SaveSettingValue(string name, object value)
        {
            configReader.SetStringValue(name,value);
        }
        #region private 
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
        private static readonly string ConfigPath = "BookConfig.xml";
        Config.ConfigReader configReader = new Config.ConfigReader(ConfigPath);
        #endregion
    }
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                SingleWrapper sw = new SingleWrapper();
                sw.Run(args);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                System.Windows.MessageBox.Show(e.StackTrace);
                System.Windows.MessageBox.Show(e.Source);
                System.Windows.MessageBox.Show(e.Data.ToString());
            }
        }
    }
}
