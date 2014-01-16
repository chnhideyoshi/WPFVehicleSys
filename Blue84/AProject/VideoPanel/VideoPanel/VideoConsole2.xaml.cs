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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for VideoConsole2.xaml
	/// </summary>
	public partial class VideoConsole2 : UserControl
	{
		public VideoConsole2()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
        }
        private void Init()
        {
            ME_MainShow.LoadedBehavior = MediaState.Play;
            ME_MainShow.UnloadedBehavior = MediaState.Manual;
            progressTimer.Interval = TimeSpan.FromMilliseconds(500);
            ANI_Rotate = this.FindResource("ANI_Rotate") as System.Windows.Media.Animation.DoubleAnimation;
            tagTimer.Interval = TimeSpan.FromMilliseconds(1000);
        }  
        private void InitEventHandler()
        {
            this.Loaded += (sender, e) => { GoToBufferingState(); };
            this.progressTimer.Tick += new EventHandler(progresstimer_Tick);
            this.tagTimer.Tick += new EventHandler(tagtimer_Tick);
            #region 触摸操作
            Point startPoint = new Point();
            //Point lastPoint = new Point();
            LayoutRoot.PreviewMouseDown += new MouseButtonEventHandler((sender, e) =>
            {
                //isDrag = true;
                startPoint = e.GetPosition(LayoutRoot);
                //lastPoint = startPoint;
            });
            #region 
            //LayoutRoot.PreviewMouseMove += new MouseEventHandler((sender, e) =>
            //{
            //    if (e.LeftButton == MouseButtonState.Pressed && isDrag)
            //    {
            //        Point currentPoint = e.GetPosition(LayoutRoot);
            //        if (GetDistence(lastPoint, currentPoint) > 5)
            //        {
            //            double DeltaX = currentPoint.X - startPoint.X;
            //            double DeltaY = currentPoint.Y - startPoint.Y;
            //            if (JudgeDirection(DeltaX, DeltaY))
            //            {
            //                double pecentage = DeltaX / Width;
            //                double currentPercentage = ME_MainShow.Position.TotalMilliseconds / ME_MainShow.NaturalDuration.TimeSpan.TotalMilliseconds;
            //                double result = (currentPercentage + pecentage);
            //                if (result < 0)
            //                    CurrentTag = "进度 : " + 0 + "%";
            //                else if (result >= 1)
            //                    CurrentTag = "进度 : " + 99 + "%";
            //                else
            //                    CurrentTag = "进度 : " + (int)(result * 100) + "%";
            //            }
            //            else
            //            {
            //                if (Math.Abs(DeltaY) > bound_dy)
            //                {
            //                    ShowConsole();
            //                    double deltaRate = DeltaY / this.Width;
            //                    SeekVolume(ME_MainShow.Volume-deltaRate*vrate);
            //                   // SetVolume(deltaRate);
            //                }
            //            }
            //            lastPoint = currentPoint;
            //        }
            //    }
            //}); 
            #endregion
            LayoutRoot.PreviewMouseUp += new MouseButtonEventHandler((sender, e) =>
            {
                if (startPoint.X != 0 && startPoint.Y != 0 && GetDistence(e.GetPosition(LayoutRoot), startPoint) < 3)
                {
                    if (BD_DownConsole.Visibility == Visibility.Visible)
                    {
                        HideConsole();
                    }
                    else
                    {
                        ShowConsole();
                    }
                }
                #region 
                //else
                //{
                //    Point currentPoint = e.GetPosition(LayoutRoot);
                //    if (GetDistence(startPoint, currentPoint) > 3)
                //    {
                //        double DeltaX = currentPoint.X - startPoint.X;
                //        double DeltaY = currentPoint.Y - startPoint.Y;
                //        if (JudgeDirection(DeltaX, DeltaY))
                //        {
                //            double pecentage = DeltaX / Width;
                //            double currentPercentage = ME_MainShow.Position.TotalMilliseconds / ME_MainShow.NaturalDuration.TimeSpan.TotalMilliseconds;
                //            SeekProgress(currentPercentage + pecentage);
                //        }
                //    }
                //} 
                #endregion
                //isDrag = false;
            }); 
            #endregion
            #region 媒体自身状态的事件
            ME_MainShow.MediaEnded += new RoutedEventHandler((sender, e) =>
                {
                    int index = videoList.IndexOf(currentVideo);
                    if (index != -1)
                    {
                        currentVideo = videoList[(index + 1) % videoList.Count];
                        ReLoadMedia();
                    }
                });
            ME_MainShow.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>((sender, e) =>
            {
                GoToFailState();
            });
            ME_MainShow.MediaOpened += new RoutedEventHandler((sender, e) =>
            {
                UC_ProgressBar.SL_Progress.IsEnabled = ME_MainShow.IsLoaded;
                // this.UC_VolumeBar.SL_Progress.IsEnabled = ME_MainShow.IsLoaded;
                GoToPlayState();
            });
            ME_MainShow.SourceUpdated += new EventHandler<DataTransferEventArgs>((sender, e) =>
            {
                GoToPlayState();
            });
            ME_MainShow.BufferingStarted += new RoutedEventHandler((sender, e) =>
            {
                GoToBufferingState();
            });
            ME_MainShow.BufferingEnded += new RoutedEventHandler((sender, e) =>
            {
                GoToPlayState();
            }); 
            #endregion
            #region 按钮事件
            BTN_GoForward.Click += new RoutedEventHandler((sender, e) =>
            {
                int index = videoList.IndexOf(currentVideo);
                if (index != -1)
                {
                    currentVideo = videoList[(index + 1) % videoList.Count];
                    ReLoadMedia();
                }
            });
            BTN_Back.Click += new RoutedEventHandler((sender, e) =>
            {
                ME_MainShow.Close();
                this.Visibility = Visibility.Collapsed;
                if (Closed != null)
                {
                    Closed(this, e);
                }
            });
            BTN_GoBackward.Click += new RoutedEventHandler((sender, e) =>
            {
                int index = videoList.IndexOf(currentVideo);
                if (index != -1)
                {
                    currentVideo = videoList[(index - 1 + videoList.Count) % videoList.Count];
                    ReLoadMedia();
                }
            });
            BTN_Play.Click += (sender, e) => { GoToPlayState(); };
            BTN_Pause.Click += (sender, e) => { GoToPauseState(); };
            BTN_Vdown.Click += (sender, e) => { SeekVolume(ME_MainShow.Volume-0.05); };
            BTN_Vup.Click += (sender, e) => { SeekVolume(ME_MainShow.Volume + 0.05); };
            BTN_Screen.Click += (sender, e) => { SwitchScreenSize(); };
            #endregion
            #region 滑竿事件
            UC_ProgressBar.ProgressDragEnd += new MyProgressBar.ProgressDragEndEventHandler((sender, e) =>
               {
                   SeekProgress(e / this.UC_ProgressBar.Maximum);
               });
            UC_ProgressBar.ProgressValueChanged += (sender, e) =>
            {
                CurrentTag = "进度 : " + (int)(e.NewValue / UC_ProgressBar.Maximum * 100) + "%";
            }; 
            #endregion
        }
        #region 其他界面逻辑
        VideoState CurrentState = VideoState.Buffering;
        DoubleAnimation ANI_Rotate;
        #region 操作便签显示
        int counttime_TagTimer = 0;
        void tagtimer_Tick(object sender, EventArgs e)
        {
            counttime_TagTimer++;
            if (counttime_TagTimer > 1)
            {
                counttime_TagTimer = 0;
                TBK_Tip.Visibility = Visibility.Collapsed;
                tagTimer.Stop();
            }
        }
        private string CurrentTag
        {
            get { return TBK_Tip.Text; }
            set
            {
                TBK_Tip.Text = value;
                TBK_Tip.Visibility = Visibility.Visible;
                counttime_TagTimer = 0;
                tagTimer.Start();
            }
        }
        DispatcherTimer tagTimer = new DispatcherTimer(DispatcherPriority.Background); 
        #endregion
        #region 控制台显示隐藏
        private double GetDistence(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
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
        public delegate void CloseEventHandler(object sender, EventArgs e);
        public event CloseEventHandler Closed;
        #endregion
        #region 数据逻辑
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
        #region 进度操作
        DispatcherTimer progressTimer = new DispatcherTimer(DispatcherPriority.Background);
        bool isInDrag = false;
        private string CurrentShowingProgress
        {
            get { return TBK_CTime.Text; }
            set { TBK_CTime.Text = value; }
        }
        private string TotalSumTime
        {
            get { return TBK_Stime.Text; }
            set { TBK_Stime.Text = value; }
        }
        private void progresstimer_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (CurrentState)
                {
                    case VideoState.Play:
                        {
                            if (!isInDrag)
                            {
                                if (ME_MainShow.NaturalDuration.HasTimeSpan)
                                {
                                    UC_ProgressBar.ProgressValue = (ME_MainShow.Position.TotalMilliseconds / ME_MainShow.NaturalDuration.TimeSpan.TotalMilliseconds) * UC_ProgressBar.Maximum;
                                    CurrentShowingProgress = ConvertToString(ME_MainShow.Position);
                                    TotalSumTime = ConvertToString(ME_MainShow.NaturalDuration.TimeSpan);
                                }
                            }
                        } break;
                    case VideoState.Pause:
                        {

                        } break;
                    case VideoState.Buffering:
                        {

                        } break;
                    default: return;
                }
            }
            catch { return; }
        }
        private string ConvertToString(TimeSpan timeSpan)
        {
            TimeSpan t1 = new TimeSpan(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return t1.ToString();
        }
        private void SeekProgress(double percentage)
        {
            if (percentage < 0)
            {
                SeekProgress(0);
                return;
            }
            else if (percentage >= 1)
            {
                SeekProgress(0.99);
                return;
            }
            else
            {
                GoToPauseState();
                ME_MainShow.Position = TimeSpan.FromMilliseconds(ME_MainShow.NaturalDuration.TimeSpan.TotalMilliseconds * percentage);
                GoToPlayState();
                CurrentTag = "进度 : " + (int)(percentage * 100) + "%";
            }
        }
        #endregion
        #region 播放状态控制操作
        public void ReLoadMedia()
        {
            try
            {
                if (!System.IO.File.Exists(currentVideo.AbsolutePath))
                {
                    GoToFailState();
                    return;
                }
                ME_MainShow.Source = new Uri(CurrentVideo.AbsolutePath, UriKind.Absolute);
                GoToPlayState();
            }
            catch
            {
                GoToFailState();
            }
        }
        private void GoToBufferingState()
        {
            CurrentState = VideoState.Buffering;

            progressTimer.Start();

            //BTN_KuaiJin.IsEnabled = false;
            //BTN_KuaiTui.IsEnabled = false;

            UC_ProgressBar.ProgressValue = 0;
            UC_ProgressBar.IsEnabled = false;

            BTN_Vdown.IsEnabled = false;
            BTN_Vup.IsEnabled = false;

            BTN_Pause.Visibility = Visibility.Visible;
            BTN_Play.Visibility = Visibility.Collapsed;
            BTN_Pause.IsEnabled = false;
            BTN_Play.IsEnabled = false;

            TBK_Fail.Visibility = Visibility.Collapsed;
            BD_DownConsole.Visibility = Visibility.Visible;
            this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, ANI_Rotate);
            EP_Buffering.Visibility = Visibility.Visible;
        }
        private void GoToFailState()
        {
            ME_MainShow.LoadedBehavior = MediaState.Manual;
            this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, null);
            TBK_Fail.Visibility = Visibility.Visible;
            EP_Buffering.Visibility = Visibility.Collapsed;
            BTN_Pause.IsEnabled = false;
            BTN_Play.IsEnabled = false;
            BTN_Play.Visibility = Visibility.Visible;
            BTN_Pause.Visibility = Visibility.Collapsed;
            UC_ProgressBar.SL_Progress.IsEnabled = false;
            CurrentState = VideoState.Failed;
        }
        private void GoToPlayState()
        {
            ME_MainShow.LoadedBehavior = MediaState.Manual;
            ME_MainShow.Play();
            CurrentState = VideoState.Play;
            progressTimer.Start();

            //BTN_KuaiJin.IsEnabled =true;
            //BTN_KuaiTui.IsEnabled = true;
            // UC_ProgressBar.ProgressValue = 0;
            UC_ProgressBar.IsEnabled = true;

            BTN_Vdown.IsEnabled = true;
            BTN_Vup.IsEnabled = true;

            BTN_Pause.Visibility = Visibility.Visible;
            BTN_Play.Visibility = Visibility.Collapsed;
            BTN_Pause.IsEnabled = true;
            BTN_Play.IsEnabled = true;

            TBK_Fail.Visibility = Visibility.Collapsed;
            BD_DownConsole.Visibility = Visibility.Visible;
            this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, null);
            EP_Buffering.Visibility = Visibility.Collapsed;
        }
        private void GoToPauseState()
        {
            ME_MainShow.LoadedBehavior = MediaState.Manual;
            ME_MainShow.Pause();
            CurrentState = VideoState.Pause;
            progressTimer.Start();

            //BTN_KuaiJin.IsEnabled = true;
            //BTN_KuaiTui.IsEnabled = true;
            // UC_ProgressBar.ProgressValue = 0;
            UC_ProgressBar.IsEnabled = true;

            BTN_Vdown.IsEnabled = true;
            BTN_Vup.IsEnabled = true;

            BTN_Pause.Visibility = Visibility.Collapsed;
            BTN_Play.Visibility = Visibility.Visible;
            BTN_Pause.IsEnabled = true;
            BTN_Play.IsEnabled = true;

            TBK_Fail.Visibility = Visibility.Collapsed;
            BD_DownConsole.Visibility = Visibility.Visible;
            this.RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, null);
            EP_Buffering.Visibility = Visibility.Collapsed;
        }
        #endregion
        #region 音量操作
        private void SeekVolume(double percentage)
        {
            if (percentage <= 1 && percentage >= 0)
            {
                ME_MainShow.Volume = percentage;
            }
            else
            {
                if (percentage < 0)
                    ME_MainShow.Volume = 0;
                else
                    ME_MainShow.Volume = 1;
            }
            CurrentTag = "音量 : " + (int)(ME_MainShow.Volume * 100 + 0.5) + "%";
        }
        #endregion
        #region 屏幕尺寸操作
        double standartProportion = 800 / 480;
        int strechIndex=0;
        List<ScreenSizeMode> strechList = new List<ScreenSizeMode>() { ScreenSizeMode.Fill, ScreenSizeMode.OringinalProportion };
        private void SwitchScreenSize()
        {
            try
            {
                strechIndex = (strechIndex + 1) % strechList.Count; ;
                int height = ME_MainShow.NaturalVideoHeight;
                int width = ME_MainShow.NaturalVideoWidth;
                if (height == 0 && width == 0)
                {
                    CurrentTag = "改变比例失败";
                    return;
                }
                else
                {
                    switch (strechList[strechIndex])
                    {
                        case ScreenSizeMode.Fill:
                            {
                                ChangeScreenSizeFill(ref width, ref height);
                                CurrentTag = "屏幕尺寸: 全屏";
                            } break;
                        case ScreenSizeMode.OringinalProportion:
                            {
                                ChangeScreenSizeOringinalProportion(ref width, ref height);
                                CurrentTag = "屏幕尺寸: 原始比例";
                            } break;
                    }
                }
            }
            catch { CurrentTag = "改变比例失败"; }
        }

        private void ChangeScreenSizeFill(ref int width, ref int height)
        {
            ME_MainShow.Height = 480;
            ME_MainShow.Width = 800;
        }

        private void ChangeScreenSizeOringinalProportion(ref int width, ref int height)
        {
            double originalPro=(double)width / (double)height;
            if (originalPro > standartProportion)
            {
                 ME_MainShow.Width = 800;
                 ME_MainShow.Height = 800 / originalPro;
            }
            else
            {
                ME_MainShow.Width = 480 * originalPro ;
                ME_MainShow.Height = 480;
            }
        }
        #endregion
    }
    public enum ScreenSizeMode
    {
        OringinalProportion = 0, Fill = 2
    }
    public enum VideoState
    {
        Buffering = 1, Failed = 2, Play = 4, Pause = 5, Close = 7
    }
}