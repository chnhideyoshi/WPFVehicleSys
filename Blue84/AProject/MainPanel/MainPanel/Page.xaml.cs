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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MainPanel
{
	/// <summary>
	/// Interaction logic for Page.xaml
	/// </summary>
	public partial class Page : UserControl
	{
		public Page()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        HorizontalVersion.SegmentTouchMouseWithRebound touchClass = new HorizontalVersion.SegmentTouchMouseWithRebound();
        private void Init()
        {
            touchClass.InitTouchListOperation(LayoutRoot,STK_MainStk);
            touchClass.PageNumber = 1;
            STB_NewProcess = this.FindResource("STB_NewProcess") as Storyboard;
            STB_NewProcess.FillBehavior = FillBehavior.Stop;
        }
        private void InitEventHandler()
        {
            STB_NewProcess.Completed += (sender, e) =>
            {
                isInAnime = false;
            };
            STB_NewProcess.Completed += (sender, e) =>
            {
                RunProcess(moduleNameWaitForStart);
            };
            this.BTN_Caculator.Click += (sender, e) => { BeginSTB("Caculator"); };
            this.BTN_Explorer.Click += (sender, e) => { BeginSTB("Explorer"); };
            this.BTN_Calindar.Click += (sender, e) => { BeginSTB("Calindar"); };
            this.BTN_BackHome.Click += (sender, e) =>
            {
                if (Back != null) { Back(); }
            };
        }
        #endregion
        #region 判断点击按钮并且引发事件的逻辑
        bool isInAnime = false;
        Storyboard STB_NewProcess;
        string moduleNameWaitForStart;
        private void RunProcess(string waitForStart)
        {
            string appPath = App.GetStartPath(waitForStart);
            if (string.IsNullOrEmpty(appPath) || !System.IO.File.Exists(appPath)) { new PopOk().ShowDialog("尚未开发完成"); return; }
            try
            {
                TestWindow.Run(appPath);
            }
            catch { new PopOk().ShowDialog("尚未开发完成"); }
        }
        private void SetSTB(Storyboard storyboard, string buttonName)
        {
            Button targetButton = this.FindName("BTN_" + buttonName) as Button;
            DoubleAnimationUsingKeyFrames ani_scX = storyboard.Children[0] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames ani_scY = storyboard.Children[1] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames ani_opa = storyboard.Children[2] as DoubleAnimationUsingKeyFrames;
            Storyboard.SetTarget(ani_scX, targetButton);
            Storyboard.SetTarget(ani_scY, targetButton);
            Storyboard.SetTarget(ani_opa, targetButton);
        }//设定动画作用的Button
        private void BeginSTB(string moduleName)
        {
            if (!isInAnime)
            {
                isInAnime = true;
                SetSTB(STB_NewProcess, moduleName);
                STB_NewProcess.Begin();
                moduleNameWaitForStart = moduleName;
            }
        }//开始动画播放
        #endregion
        #region 事件
        public delegate void BackEventHandler();
        public event BackEventHandler Back;
        #endregion
    }
}