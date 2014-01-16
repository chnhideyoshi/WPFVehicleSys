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

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for MainHost.xaml
	/// </summary>
	public partial class MainHost : UserControl
	{
		public MainHost()
		{
			this.InitializeComponent();
            InitEventHandler();
            Init();
		}
        #region 初始化
        private void Init()
        {
            CreateListShow();
        }
        private void InitEventHandler()
        {
            #region 按钮事件
            BTN_BackHome.Click += (sender, e) =>
            {
                App.Current.Shutdown();
            };
            BTN_Switch.Click += (sender, e) =>
            {
                ShowWindow();
            };
            this.CKB_Switch.Click += (sender, e) =>
            {
                if (isInListShowState)
                {
                    CreateSwicthShow();
                    isInListShowState = false;
                }
                else
                {
                    CreateListShow();
                    isInListShowState = true;
                }
            };
            #endregion
        } 
        #endregion
        #region 界面逻辑
        ListShow UC_ListShow = null;
        SwitchShow UC_SwicthShow = null;
        private bool isInListShowState = true;//标记 指示当前是Listshow可见还是switchshow可见
        private void CreateListShow()
        {
            if (this.CC_ListShow.Content == null || CC_ListShow.Content as ListShow == null)
            {
                UC_ListShow = new ListShow();
                UC_ListShow.ItemClick += new ListShow.ItemClickEventHandler(UC_MainList_ItemsClicked);
                this.CC_ListShow.Content = UC_ListShow;
                GoToListState();
            }
            else
            {
                GoToListState();
            }
        }//在本控件上创建ListShow控件
        private void CreateSwicthShow()
        {
            if (this.CC_SwitchShow.Content == null || CC_SwitchShow.Content as SwitchShow == null)
            {
                UC_SwicthShow = new SwitchShow();
                UC_SwicthShow.ItemClick += new SwitchShow.ItemClickEventHandler(UC_MainList_ItemsClicked);
                this.CC_SwitchShow.Content = UC_SwicthShow;
                GoToSwitchState();
            }
            else
            {
                GoToSwitchState();
            }
        }//在本控件上创建SwitchShow控件
        private void GoToListState()
        {
            CC_ListShow.Visibility = Visibility.Visible;
            CC_MainHold.Visibility = Visibility.Collapsed;
            CC_SwitchShow.Visibility = Visibility.Collapsed;

        }//切换到ListShow视图
        private void GoToSwitchState()
        {
            CC_ListShow.Visibility = Visibility.Collapsed;
            CC_MainHold.Visibility = Visibility.Collapsed;
            CC_SwitchShow.Visibility = Visibility.Visible;
        }//切换到SwitchShow视图
        private void GoToExpanderState()
        {
            CC_MainHold.Visibility = Visibility.Visible;
        }//切换到全屏播放视图
        private void ShowWindow()
        {
            PopWindowsLib.PopOk pop = new PopWindowsLib.PopOk();
            pop.IsShowGrid = false;
            pop.Text = "扩展功能暂未提供";
            pop.ShowDialog();
        }//点击功能按钮的弹出窗口,该函数可去掉或修改
        #endregion
        #region 播放器的选择
        void UC_MainList_ItemsClicked(VideoInfo videoInfo, List<VideoInfo> videoInfoList)
        {
            if (IsMediaElementSupported(videoInfo))//如果是MediaElement格式则用MediaElement播放
            {
                StartMediaElementControlPanel(videoInfo, videoInfoList);
            }
            else//否则采用WMPactiveX组件播放
            {
                StartAxWindowsMediaPlayerPanel(videoInfo, videoInfoList);
            }
        }//在ListShow或SwitchShow点击视频的事件处理
        private bool IsMediaElementSupported(VideoInfo itemInfo)
        {
            string ext = itemInfo.Extension;
            if (ext.ToUpper() == ".WMV"
                || ext.ToUpper() == ".MP4"
                || ext.ToUpper() == ".MPG"
                || ext.ToUpper() == ".AVI"
                )
            {
                return true;
            }
            return false;
        }//判断后缀名是否是ME支持
        private void StartAxWindowsMediaPlayerPanel(VideoInfo itemInfo, List<VideoInfo> list)
        {
            try
            {
                VideoConsole3 W3 = new VideoConsole3();
                W3.VideoList = list;
                W3.CurrentVideo = itemInfo;
                W3.Show();
            }
            catch { return; }
        }//WMP的包装控件为VideoConsole3 控制条为VideoConsole3Part
        private void StartMediaElementControlPanel(VideoInfo itemInfo, List<VideoInfo> list)
        {
            VideoConsole2 console;
            if (CC_MainHold.Content != null && CC_MainHold.Content is VideoConsole2)
            {
                console = CC_MainHold.Content as VideoConsole2;
                console.VideoList = list;
                console.CurrentVideo = itemInfo;
                console.Visibility = Visibility.Visible;
                console.ReLoadMedia();
            }
            else
            {
                console = new VideoConsole2();
                console.VideoList = list;
                console.CurrentVideo = itemInfo;
                CC_MainHold.Content = console;
                console.ReLoadMedia();
            }
            GoToExpanderState();
        }//ME的包装控件为VideoConsole2
        #endregion
    }
}