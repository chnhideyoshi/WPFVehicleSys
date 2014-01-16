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

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for VideoConsole3.xaml
	/// </summary>
	public partial class VideoConsole3 : Window
	{
		public VideoConsole3()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
            Topmost = true;
			// Insert code required on object creation below this point.
		}
        public AxWMPLib.AxWindowsMediaPlayer AXWMP_VPlayer;
        private VideoConsole3Part ChildWindow;
        private void Init()
        {
            
            #region 初始化
            this.AXWMP_VPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.AXWMP_VPlayer)).BeginInit();
            this.AXWMP_VPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AXWMP_VPlayer.Enabled = true;
            this.AXWMP_VPlayer.Name = "AXWMP_VPlayer";
            //this.AXWMP_VPlayer.uiMode = "none";
           // this.AXWMP_VPlayer.settings.autoStart = true;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WMPControlLibrary.WMPControl));
            this.AXWMP_VPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            ((System.ComponentModel.ISupportInitialize)(this.AXWMP_VPlayer)).EndInit();
            WFH_Main.Child = AXWMP_VPlayer;
            #endregion
            ChildWindow = new VideoConsole3Part(this);
            //ChildWindow.Owner = this;
            
        }

        private void InitEventHandler()
        {
            this.AXWMP_VPlayer.MouseDownEvent += (sender, e) => 
            {
                if (!ChildWindow.IsVisible)
                {
                    ChildWindow.Show();
                }
                else
                {
                    ChildWindow.Hide();
                }
            };
            Loaded += (sender, e) => 
            {
                if (!System.IO.File.Exists(currentVideo.AbsolutePath)) { return; }
                AXWMP_VPlayer.URL = CurrentVideo.AbsolutePath;
            };
        }
        List<VideoInfo> videoList;
        VideoInfo currentVideo;
        public VideoInfo CurrentVideo
        {
            get { return currentVideo; }
            set { 
                currentVideo = value;
                ChildWindow.CurrentVideo = value;
            }
        }
        public List<VideoInfo> VideoList
        {
            get { return videoList; }
            set
            {
                videoList = value;
                ChildWindow.VideoList = value;
            }
        }
	}
}