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
using System.Windows.Threading;

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for VideoConsole3Part.xaml
	/// </summary>
	public partial class VideoConsole3Part : Window
	{
		public VideoConsole3Part()
		{
			this.InitializeComponent();
			// Insert code required on object creation below this point.
		}
        public VideoConsole3Part(VideoConsole3 parentMediaWindow)
        {
            this.InitializeComponent();
            parentWindow=parentMediaWindow;
            Topmost = true;
            #region
            Init();
            InitEventHandler();
            //ReLoadMedia();
            #endregion
        }
        VideoConsole3 parentWindow;
        public VideoConsole3 ParentWindow
        {
            get{return parentWindow;}
        }
        public AxWMPLib.AxWindowsMediaPlayer ME_MainShow
        {
            get { return parentWindow.AXWMP_VPlayer; }
        }
        #region 与VideoConsole2类似代码
        private void Init()
        {
            progresstimer.Interval = TimeSpan.FromMilliseconds(500);
            tagtimer.Interval = TimeSpan.FromMilliseconds(1000);
        }
        private void InitEventHandler()
        {
            this.Loaded += (sender, e) => { GoToPlayState(); };
            this.progresstimer.Tick += new EventHandler(progresstimer_Tick);
            this.tagtimer.Tick += new EventHandler(tagtimer_Tick);
            ME_MainShow.MediaError += (sender, e) =>
            {
                App.Current.Shutdown();
            };
            #region 滑竿事件
            UC_ProgressBar.ProgressDragBegin += (sender, e) =>
                {
                    progresstimer.Stop();
                };
            UC_ProgressBar.ProgressValueChanged += (sender, e) =>
            {
                CurrentTag = "进度 : " + (int)(e.NewValue / UC_ProgressBar.Maximum * 100) + "%";
            };
            UC_ProgressBar.ProgressDragEnd += (sender, e) =>
            {
                progresstimer.Start();
                SeekProgress(e / this.UC_ProgressBar.Maximum);
            }; 
            #endregion
            #region 按钮事件
            BTN_GoForward.Click += (sender, e) =>
               {
                   int index = videoList.IndexOf(currentVideo);
                   if (index != -1)
                   {
                       currentVideo = videoList[(index + 1) % videoList.Count];
                       ReLoadMedia();
                   }
               };
            BTN_Back.Click += (sender, e) =>
            {
                ME_MainShow.close();
                this.Hide();
                ParentWindow.Hide();
            };
            BTN_GoBackward.Click += (sender, e) =>
            {
                int index = videoList.IndexOf(currentVideo);
                if (index != -1)
                {
                    currentVideo = videoList[(index - 1 + videoList.Count) % videoList.Count];
                    ReLoadMedia();
                }
            };
            BTN_Play.Click += (sender, e) => { GoToPlayState(); };
            BTN_Pause.Click += (sender, e) => { GoToPauseState(); };
            BTN_Vup.Click += (sender, e) =>
            {
                double cv = ME_MainShow.settings.volume;
                cv += 5;
                SetVolume(cv);
            };
            BTN_Vdown.Click += (sender, e) =>
            {
                double cv = ME_MainShow.settings.volume;
                cv -= 5;
                SetVolume(cv);
            };
            BTN_Screen.Click += (sender, e) => 
            {
                SwitchScreenSize();
            };
            #endregion
        }
        #region 界面逻辑
        int counttime_Tag = 0;
        void tagtimer_Tick(object sender, EventArgs e)
        {
            counttime_Tag++;
            if (counttime_Tag > 1)
            {
                counttime_Tag = 0;
                TBK_Tip.Visibility = Visibility.Collapsed;
                tagtimer.Stop();
            }
        }
        public string CurrentTag
        {
            get { return TBK_Tip.Text; }
            set
            {
                TBK_Tip.Text = value;
                TBK_Tip.Visibility = Visibility.Visible;
                counttime_Tag = 0;
                tagtimer.Start();
            }
        }
        public string CurrentShowingProgress
        {
            get { return TBK_CTime.Text; }
            set { TBK_CTime.Text = value; }
        }
        public string TotalSumTime
        {
            get { return TBK_Stime.Text; }
            set { TBK_Stime.Text = value; }
        }
        private string ConvertToString(double timeSpan)
        {
            TimeSpan t1 = TimeSpan.FromSeconds((int)timeSpan);
            return t1.ToString();
        }
        DispatcherTimer tagtimer = new DispatcherTimer();
        bool isInDrag = false;
        DispatcherTimer progresstimer = new DispatcherTimer(DispatcherPriority.Background);
        private void progresstimer_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (ME_MainShow.playState)
                {
                    case WMPLib.WMPPlayState.wmppsPlaying:
                        {
                            if (!isInDrag)
                            {
                                #region 检测播放结束
                                double d = ME_MainShow.currentMedia.duration - ME_MainShow.Ctlcontrols.currentPosition;
                                if (d <= 0.5)
                                {
                                    int index = videoList.IndexOf(currentVideo);
                                    if (index != -1)
                                    {
                                        currentVideo = videoList[(index + 1 + videoList.Count) % videoList.Count];
                                        ReLoadMedia();
                                    }
                                } 
                                #endregion
                                #region 显示时间标签和进度条
                                double ez = (ME_MainShow.Ctlcontrols.currentPosition / ME_MainShow.currentMedia.duration);
                                UC_ProgressBar.ProgressValue = ez * UC_ProgressBar.Maximum;
                                CurrentShowingProgress = ConvertToString(ME_MainShow.Ctlcontrols.currentPosition);
                                TotalSumTime = ConvertToString(ME_MainShow.currentMedia.duration); 
                                #endregion
                            }
                        } break;
                    case WMPLib.WMPPlayState.wmppsPaused:
                        {
                        } break;
                    case WMPLib.WMPPlayState.wmppsBuffering:
                        {
                            CurrentTag = "加载中";
                        } break;
                    default: return;
                }
            }
            catch { return; }
        }
        private void HideConsole()
        {
            BD_DownConsole.Visibility = Visibility.Collapsed;
        }
        private void ShowConsole()
        {
            BD_DownConsole.Visibility = Visibility.Visible;
        }
        #endregion
        #region 操作
        public void ReLoadMedia()
        {
            try
            {
                GoToBufferingState();
                if (!System.IO.File.Exists(currentVideo.AbsolutePath)) { GoToFailState(); return; }
                ME_MainShow.URL=(CurrentVideo.AbsolutePath);
                GoToPlayState();
            }
            catch
            {
                GoToFailState();
            }
        }
        public void GoToBufferingState()
        {
            //
            //CurrentState = VideoState.Buffering;

            progresstimer.Start();

            //BTN_KuaiJin.IsEnabled = false;
            //BTN_KuaiTui.IsEnabled = false;

            UC_ProgressBar.ProgressValue = 0;
            UC_ProgressBar.IsEnabled = false;

            BTN_Vdown.IsEnabled = false;
            BTN_Vup.IsEnabled = false;

            BTN_Pause.Visibility=Visibility.Visible;
            BTN_Play.Visibility=Visibility.Collapsed;
            BTN_Pause.IsEnabled = false;
            BTN_Play.IsEnabled = false;

           
            BD_DownConsole.Visibility = Visibility.Visible;
            //this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, ANI_Rotate);
            //EP_Buffering.Visibility = Visibility.Visible;
        }
        public void GoToFailState()
        {
            //ME_MainShow.LoadedBehavior = MediaState.Manual;
            //this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, null);
          
            //EP_Buffering.Visibility = Visibility.Collapsed;
            BTN_Pause.IsEnabled = false;
            BTN_Play.IsEnabled = false;
            BTN_Play.Visibility = Visibility.Visible;
            BTN_Pause.Visibility = Visibility.Collapsed;
            UC_ProgressBar.SL_Progress.IsEnabled = false;
            //CurrentState = VideoState.Failed;
        }
        public void GoToPlayState()
        {
            //ME_MainShow.LoadedBehavior = MediaState.Manual;
            ME_MainShow.Ctlcontrols.play();
            //CurrentState = VideoState.Play;
            progresstimer.Start();

            //BTN_KuaiJin.IsEnabled =true;
            //BTN_KuaiTui.IsEnabled = true;
            // UC_ProgressBar.ProgressValue = 0;
            UC_ProgressBar.IsEnabled = true;

            BTN_Vdown.IsEnabled = true;
            BTN_Vup.IsEnabled = true;

            BTN_Pause.Visibility=Visibility.Visible;
            BTN_Play.Visibility=Visibility.Collapsed;
            BTN_Pause.IsEnabled = true;
            BTN_Play.IsEnabled = true;
            BD_DownConsole.Visibility = Visibility.Visible;
        }
        public void GoToPauseState()
        {
            ME_MainShow.Ctlcontrols.pause();
            progresstimer.Start();
            //BTN_KuaiJin.IsEnabled = true;
            //BTN_KuaiTui.IsEnabled = true;
            UC_ProgressBar.IsEnabled = true;
            BTN_Vdown.IsEnabled = true;
            BTN_Vup.IsEnabled = true;
            BTN_Pause.Visibility = Visibility.Collapsed;
            BTN_Play.Visibility = Visibility.Visible;
            BTN_Pause.IsEnabled = true;
            BTN_Play.IsEnabled = true;
            BD_DownConsole.Visibility = Visibility.Visible;
        }
        public void GoToSpeedMode(bool isfast)
        {
            if (isfast)
            {
                //ME_MainShow.Ctlcontrols. += 0.05;
                //GoToPlayState();
                ME_MainShow.Ctlcontrols.fastForward();
            }
            else
            {
                //if (ME_MainShow.SpeedRatio - 0.05 > 0)
                //{
                //    ME_MainShow.SpeedRatio -= 0.05;
                //    GoToPlayState();
                //}
                ME_MainShow.Ctlcontrols.fastReverse();
            }
            //CurrentTag = "速度 : " + (int)(ME_MainShow.SpeedRatio * 100) + "%";
        }
        public void SeekProgress(double percentage)
        {
            GoToPauseState();
            ME_MainShow.Ctlcontrols.currentPosition = ME_MainShow.currentMedia.duration*percentage;
            GoToPlayState();
            CurrentTag = "进度 : " + (int)(percentage * 100) + "%";
        }
        public void SetVolume(double e)
        {
            ME_MainShow.settings.volume = (int)e;
            CurrentTag = "音量 : " + ME_MainShow.settings.volume + "%";
        }
        public void SeekVolume(double percentage)
        {
            if (percentage <= 1 && percentage >= 0)
            {
                return;
            }
            else
            {
                if (percentage < 0)
                    ME_MainShow.settings.volume = 0;
                else
                    ME_MainShow.settings.volume = 1;
            }
            CurrentTag = "音量 : " + (int)(ME_MainShow.settings.volume * 100) + "%";
        }
        #endregion    
        #region 数据
        List<VideoInfo> videoList;
        VideoInfo currentVideo;
        public VideoInfo CurrentVideo
        {
            get { return currentVideo; }
            set { currentVideo = value; }
        }
        public List<VideoInfo> VideoList
        {
            get { return videoList; }
            set { videoList = value; }
        }
        #endregion
        #region 屏幕尺寸模式
        double standartProportion = 800 / 480;
        //int strechIndex = 0;
        //List<ScreenSizeMode> strechList = new List<ScreenSizeMode>() { ScreenSizeMode.Fill, ScreenSizeMode.OringinalProportion };
        private void SwitchScreenSize()
        {
            CurrentTag = "屏幕尺寸: 原始比例(本格式不支持满屏)";
            //strechIndex = (strechIndex + 1) % strechList.Count;
            //switch (strechList[strechIndex])
            //{
            //    case ScreenSizeMode.Fill:
            //        {
            //            ResizeFill();
            //            CurrentTag = "屏幕尺寸: 全屏";
            //        } break;
            //    case ScreenSizeMode.OringinalProportion:
            //        {
            //            ResizeOriginal();
            //            CurrentTag = "屏幕尺寸: 原始比例";
            //        } break;
            //}
        }
        private void ResizeFill()
        {

        }
        private void ResizeOriginal()
        {
            int width = ME_MainShow.currentMedia.imageSourceWidth;
            int height = ME_MainShow.currentMedia.imageSourceHeight;
            double originalPro = (double)width / (double)height;
            if (originalPro > standartProportion)
            {
                ME_MainShow.Width = 800;
                ME_MainShow.Height = (int)(800 / originalPro);
            }
            else
            {
                ME_MainShow.Width = (int)(480 * originalPro);
                ME_MainShow.Height = 480;
            }
        }


        #endregion
        #endregion
    }
}