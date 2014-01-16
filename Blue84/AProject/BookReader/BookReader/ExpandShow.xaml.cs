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
using System.ComponentModel;
namespace BookReader
{
	/// <summary>
	/// Interaction logic for ExpandShow.xaml
	/// </summary>
	public partial class ExpandShow : UserControl
	{
		public ExpandShow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		} 
        ManipTest.TouchMouseWithRebound touchClass = new ManipTest.TouchMouseWithRebound();
        private void Init()
        {
            touchClass.InitTouchListOperation(LayoutRoot, STK_Containner);
            ANI_Rotate = this.FindResource("ANI_Rotate") as System.Windows.Media.Animation.DoubleAnimation;
            GD_Console.Visibility = Visibility.Collapsed;
        }
        private void InitEventHandler()
        {
            #region 拖动滑竿上下移动
            this.CA_ExpSL.PreviewMouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double rate = e.GetPosition(CA_ExpSL).Y / CA_ExpSL.ActualHeight;
                    touchClass.ResetBack(false);
                    SL_Bar.ProgressValue = rate * SL_Bar.Maximum;
                    Canvas.SetTop(STK_Containner, -(touchClass.MaxTop) * rate);
                }
            };
            touchClass.EndOperation += () =>
            {
                if (touchClass.MaxTop > 0)
                {
                    SL_Bar.ProgressValue = SL_Bar.Maximum * (-Canvas.GetTop(STK_Containner) / touchClass.MaxTop);
                }
            };//确保鼠标松开后滑竿也从新定位
            #endregion
            #region  功能：点击界面出现控制条，再点一下消失
            LayoutRoot.PreviewMouseDown += new MouseButtonEventHandler((sender, e) =>
            {
                startPoint = e.GetPosition(LayoutRoot);
            });
            LayoutRoot.PreviewMouseUp += new MouseButtonEventHandler((sender, e) =>
            {
                if (GetDistence(startPoint, e.GetPosition(LayoutRoot)) < Bound_ShowConsole)
                {
                    GD_Console.Visibility = (GD_Console.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
                }
            });
            #endregion
            #region 按钮事件
            BTN_Back.Click += new RoutedEventHandler((sender, e) =>
            {
                Back();
            });
            this.BTN_SysBegin.Click += new RoutedEventHandler((sender, e) =>
            {
                BeginSpeech();
            });
            BTN_Pause.Click += (sender, e) =>
            {
                PauseSpeech();
            };
            BTN_Stop.Click += (sender, e) =>
            {
                StopSpeech();
            };
            BTN_Font.Click+=(sender,e)=>
            {
                SwitchFont();
                (App.Current as App).SaveSettingValue("FontSize", ContentFontSize);
            };
            #endregion
            App.ViewModel.LoadContentCompleted += new ViewModel.LoadContentCompletedEventHandler(ViewModel_LoadContentCompleted);
        }

       
        #region 界面操作
        Point startPoint = default(Point);
        DoubleAnimation ANI_Rotate;
        double Bound_ShowConsole = 3;
        private double GetDistence(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        public delegate void CloseEventHandler();
        public event CloseEventHandler Closed;
        private void Back()
        {
            touchClass.ResetBack(true);
            BookInfo.Content = null;
            BookInfo = null;
            TB_Main.Text = "";
            if (ssz != null)
            {
                ssz.Dispose();
                ssz = null;
            }
            if (Closed != null) { Closed(); }
        }
        private void GoToLoadState()
        {
            EP_Buffering.Visibility = Visibility.Visible;
            RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, ANI_Rotate);
        }
        private void GoToDisplayState()
        {
            EP_Buffering.Visibility = Visibility.Collapsed;
            RTF_Buffering.BeginAnimation(RotateTransform.AngleProperty, null);
        }
        #endregion
        #region 加载图书模块
        private BookInfo bookInfo;
        public BookInfo BookInfo
        {
            get { return bookInfo; }
            set { bookInfo = value; touchClass.ResetBack(true); }
        }
        public void RefreshText()
        {
            if (BookInfo== null){return;}
            if (BookInfo.Content == null)
            {
                App.ViewModel.BegionLoadContentWorker(BookInfo);
                GoToLoadState();
            }
            else
            {
                TB_Main.Text = BookInfo.Content;
            }

        }
        void ViewModel_LoadContentCompleted(string content)
        {
            BookInfo.Content = content;
            GoToDisplayState();
            TB_Main.Text = BookInfo.Content;
        }
        #endregion
        #region 朗读功能模块
        int rindex = 0;
        System.Speech.Synthesis.SpeechSynthesizer ssz;
        private void Read()
        {
            if (ssz == null)
            {
                ssz = new System.Speech.Synthesis.SpeechSynthesizer();
                ssz.SpeakCompleted += (sender, e) => 
                {
                    if (rindex + 1 < BookInfo.Sentenses.Length)
                    {
                        rindex++;
                        ssz.SpeakAsync(BookInfo.Sentenses[rindex]);
                    }
                    else
                    {
                        ssz.Pause();
                    }
                };
                ssz.Volume = 100;
            }
            if (ssz.State == System.Speech.Synthesis.SynthesizerState.Paused)
            {
                ssz.Resume();
                return;
            }
            if (ssz.State == System.Speech.Synthesis.SynthesizerState.Speaking)
            {
                return;
            }
            if (BookInfo.Sentenses != null && BookInfo.Sentenses.Length > 0)
            {
                ssz.SpeakAsync(BookInfo.Sentenses[rindex]);
            }
        }
        private void StopSpeech()
        {
            if ((App.Current as App).IsEnableSpeech)
            {
                if (ssz != null)
                {
                    ssz.Dispose();
                    ssz = null;
                    BTN_Pause.Visibility = Visibility.Collapsed;
                    BTN_SysBegin.Visibility = Visibility.Visible;
                    rindex = 0;
                }
            }
            else
            {
                new MainPanel.PopOk().ShowDialog("朗读功能暂不提供！");
            }
        }
        private void PauseSpeech()
        {
            if ((App.Current as App).IsEnableSpeech)
            {
                if (ssz != null)
                {
                    ssz.Pause();
                    BTN_Pause.Visibility = Visibility.Collapsed;
                    BTN_SysBegin.Visibility = Visibility.Visible;
                }
            }
            else
            {
                new MainPanel.PopOk().ShowDialog("朗读功能暂不提供！");
            }
        }
        private void BeginSpeech()
        {
            if ((App.Current as App).IsEnableSpeech)
            {
                Read();
                BTN_Pause.Visibility = Visibility.Visible;
                BTN_SysBegin.Visibility = Visibility.Collapsed;
            }
            else
            {
                new MainPanel.PopOk().ShowDialog("朗读功能暂不提供！");
            }
        }
        #endregion
        #region 字体模块
        public double ContentFontSize
        {
            set { TB_Main.FontSize = value; }
            get { return TB_Main.FontSize; }
        }
        private int currentIndexInFontCollection = 3;
        private List<double> fontCollection = new List<double>() { 15, 17, 19, 21, 23, 25, 27, 29, 31 };
        private void SwitchFont()
        {
            currentIndexInFontCollection = (currentIndexInFontCollection + 1) % fontCollection.Count;
            ContentFontSize = fontCollection[currentIndexInFontCollection];
        } 
        #endregion
    }
}