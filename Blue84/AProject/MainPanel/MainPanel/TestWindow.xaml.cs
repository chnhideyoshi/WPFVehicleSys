using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MainPanel
{
	/// <summary>
	/// Interaction logic for TestWindow.xaml
	/// </summary>
	public partial class TestWindow : Window
	{
		public TestWindow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();  
		}
        #region 初始化
        MainPanelCircle.TouchMouseInMainPanel touchClass = new MainPanelCircle.TouchMouseInMainPanel();
        private void Init()
        {
            touchClass.InitTouchOperation(LayoutRoot,UC_Circle);
            GoToMainMenu();
        }
        private void InitEventHandler()
        {
            UC_Circle.ButtonClick += (waitforstart) => 
            {
                if (waitforstart == "Tool")
                {
                    GoToSubMenu(waitforstart);
                    return;
                }
                RunProcess(waitforstart);
            };
        }
        #endregion
        #region 启动新进程
        private void RunProcess(string waitForStart)
        {
            string appPath = App.GetStartPath(waitForStart);
            if (string.IsNullOrEmpty(appPath) || !System.IO.File.Exists(appPath))
            {
                new PopOk().ShowDialog("尚未开发完成");
                return;
            }
            try
            {
                Run(appPath);
            }
            catch
            {
                new PopOk().ShowDialog("尚未开发完成");
                return;
            }
        }
        public static void Run(string appPath)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(appPath);
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = appPath;
            p.Start();
            p.Close();
        }
        #endregion
        #region 界面逻辑（二级界面跳转返回）
        private void GoToMainMenu()
        {
            this.LayoutRoot.Visibility = Visibility.Visible;
            this.CC_SubHost.Visibility = Visibility.Collapsed;
        }
        private void GoToSubMenu(string id)
        {
            if (CC_SubHost.Content == null || CC_SubHost.Content as Page == null)
            {
                Page p = new Page();
                p.Back += () => { GoToMainMenu(); };
                CC_SubHost.Content = p;
            }
            this.LayoutRoot.Visibility = Visibility.Collapsed;
            this.CC_SubHost.Visibility = Visibility.Visible;
        }
        #endregion
    }
}