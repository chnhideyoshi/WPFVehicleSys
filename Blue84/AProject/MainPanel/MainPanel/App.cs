using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using Config;

namespace MainPanel
{
    public class SingleWrapper : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        /// <summary>
        /// 该类的基类引用Microsoft.VisualBasic程序集，作用是实现App类的单进程实例启动。
        /// </summary>
        public SingleWrapper()
        {
            this.IsSingleInstance = true;
        }
        private App app;
        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {
            app = new App();//创建当前的App实例
            app.Exit += (sender, e) =>
            {
                if (app.AutoReStart)
                {
                    ReStart();
                }
            };//关闭本程序时 如果配置设定AutoReStart为true则自动重启
            app.DispatcherUnhandledException += (sender, e) =>
            {
                if (app.AutoReStart)
                {
                    ReStart();
                }
            };//出现任何不可处理的异常时 如果配置设定AutoReStart为true则自动重启
            app.Run();
            return false;
        }
        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            //此处的操作是用户在已经有一个当前的程序进程运行时又启动了一个这个进程时执行的代码，单实例运行时此处应为空
            return;
        }
        /// <summary>
        /// 重启本应用程序
        /// </summary>
        public void ReStart()
        {
             System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
             app.Shutdown();
        }
    }
    public class App : System.Windows.Application
    {
        public App()
            : base()
        {
            InitDefaultPaths();
            InitAppSettings();
            this.StartupUri = new Uri("TestWindow.xaml", UriKind.Relative);//起始页面为TestWindow.xaml
        }

        private void InitAppSettings()
        {
            #region 启动参数从配置文件中获取
            #region 查看机器的盘数 <4则是在真设备上(真设备上只有C D 两个盘)
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
                IsInEmbededDevice = true;
            }
            #endregion
            IsTopMost = configReader.GetBooleanSettingItem("IsTopMost", IsTopMost);
            AutoReStart = configReader.GetBooleanSettingItem("AutoReStart", AutoReStart);
            Paths = configReader.GetStringCollectionSettingItem("Paths", Paths);
            #endregion
        }
        private void InitDefaultPaths()
        {
            Paths = new Dictionary<string, string>();
            Paths.Add("Book", "Book/BookReader.exe");
            Paths.Add("Picture", "Picture/PictureBrowserPanel.exe");
            Paths.Add("Music", "Music/MusicPlayer.exe");
            Paths.Add("Video", "Video/VideoPanel.exe");
            Paths.Add("GPS", "GPS/GPS.exe");
            Paths.Add("Caculator", "Tools/Calculator/Calculator.exe");
            Paths.Add("Explorer", "Tools/Explorer/ExplorerPanel.exe");
            Paths.Add("Calindar", "Tools/Calendar/WpfCalendar.exe");
            Paths.Add("Setting", "Settings/Settings.exe");
            Paths.Add("Help", "Help/SystemHelp.exe");
            Paths.Add("FM", "FM/FMRadio.exe");
        }
        #region 应用程序设置
        public bool IsTopMost=false;
        public bool IsInEmbededDevice = false;
        public bool AutoReStart = false;
        public Dictionary<string, string> Paths;
        #endregion
        #region 设置鼠标是否显示的函数
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern void ShowCursor(int status);
        #endregion   
        private static string ConfigPath = "Config.xml";
        private ConfigReader configReader = new ConfigReader(ConfigPath);
     

        public static string GetStartPath(string waitForStart)
        {
            return (App.Current as App).Paths[waitForStart];
        }
    }
    public class Program
    {
        #region 应用程序入口
        [STAThread] 
        public static void Main(string[] args)
        {
            SingleWrapper sw = new SingleWrapper();
            sw.Run(args);
        }
        #endregion
    }
}
