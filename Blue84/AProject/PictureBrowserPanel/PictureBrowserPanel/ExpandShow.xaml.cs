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

namespace PictureBrowserPanel
{
	/// <summary>
	/// Interaction logic for ExpandShow.xaml
	/// </summary>
	public partial class ExpandShow : UserControl
    {
        public ExpandShow()
        {
            InitializeComponent();
            Init();
            InitEventHandler();
        }
        #region 初始化
        private void Init()
        {
            #region
            STB_ImageSwitchBackward = this.FindResource("STB_ImageSwitchBackward2") as Storyboard;
            STB_ImageSwitchForward = this.FindResource("STB_ImageSwitchForward2") as Storyboard;
            STB_ImageSwitchBackwardBack = this.FindResource("STB_ImageSwitchBackward2Back") as Storyboard;
            STB_ImageSwitchForwardBack = this.FindResource("STB_ImageSwitchForward2Back") as Storyboard;
            STB_RotateUC90 = this.FindResource("STB_RotateUC90") as Storyboard;
            STB_RotateUC180 = this.FindResource("STB_RotateUC180") as Storyboard;
            STB_RotateUC270 = this.FindResource("STB_RotateUC270") as Storyboard;
            STB_RotateUC360 = this.FindResource("STB_RotateUC360") as Storyboard;
            STB_RotateUC90.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateUC180.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateUC270.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateUC360.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateUC90.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 90; });
            STB_RotateUC180.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 180; });
            STB_RotateUC270.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 270; });
            STB_RotateUC360.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 0; });
            STB_RotateC90 = this.FindResource("STB_RotateC90") as Storyboard;
            STB_RotateC180 = this.FindResource("STB_RotateC180") as Storyboard;
            STB_RotateC270 = this.FindResource("STB_RotateC270") as Storyboard;
            STB_RotateC360 = this.FindResource("STB_RotateC360") as Storyboard;
            STB_RotateC90.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateC180.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateC270.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateC360.FillBehavior = FillBehavior.HoldEnd;
            STB_RotateC90.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 270; });
            STB_RotateC180.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 180; });
            STB_RotateC270.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 90; });
            STB_RotateC360.Completed += new EventHandler((sender, e) => { isInAnime = false; angel = 0; });
            STB_ImageSwitchBackward.FillBehavior = FillBehavior.Stop;
            STB_ImageSwitchForward.FillBehavior = FillBehavior.Stop;
            STB_ImageSwitchBackwardBack.FillBehavior = FillBehavior.Stop;
            STB_ImageSwitchForwardBack.FillBehavior = FillBehavior.Stop;
            this.UC_ViewConsole.Visibility = Visibility.Collapsed;
            MainShow.Fill = new ImageBrush() { Stretch = streches[strechIndex] };
            LastShow.Fill = new ImageBrush() { Stretch = streches[strechIndex] };
            NextShow.Fill = new ImageBrush() { Stretch = streches[strechIndex] };
            #endregion
        }
        private void InitEventHandler()
        {
            App.ViewModel.ViewWorkCompleted += new ViewModel.ViewWorkCompletedEventHandler(ViewModel_ViewWorkCompleted);
            #region 鼠标触摸操作
            CA_ControlCanvas.PreviewMouseDown += new MouseButtonEventHandler(CA_ControlCanvas_PreviewMouseDown);
            CA_ControlCanvas.PreviewMouseUp +=new MouseButtonEventHandler(CA_ControlCanvas_PreviewMouseUp);
            CA_ControlCanvas.PreviewMouseMove += new MouseEventHandler(CA_ControlCanvas_PreviewMouseMove);
            STB_ImageSwitchBackward.Completed += new EventHandler(STB_ImageSwitchBackward_Completed);
            STB_ImageSwitchForward.Completed += new EventHandler(STB_ImageSwitchForward_Completed);
            STB_ImageSwitchBackwardBack.Completed += new EventHandler((sender, e) =>
            {
                ShowImage();
            });
            STB_ImageSwitchForwardBack.Completed += new EventHandler((sender, e) =>
            {
                ShowImage();
            }); 
            #endregion
            #region 按钮事件
            this.UC_ViewConsole.ReturnLastButtonClicked += new ViewConsole.ReturnLast((sender, e) =>
               {
                   if (!isInAnime)
                   { ResetRotateState(); STB_ImageSwitchBackward.Begin(); isInAnime = true; }
               });
            this.UC_ViewConsole.GoForwardButtonClicked += new ViewConsole.GoForwardEventHandler((sender, e) =>
            {
                if (!isInAnime)
                { ResetRotateState(); STB_ImageSwitchForward.Begin(); isInAnime = true; }
            });
            UC_ViewConsole.BackButtonClicked += new ViewConsole.BackEventHandler((sender, e) =>
            {
                this.DisposeAllImage();
                this.PictureInfoList = null;
                this.CurrentIndex = 0;
                if (Closed != null) { Closed(); }
            });
            UC_ViewConsole.RotateUC += new ViewConsole.RotateUCEventHandler(UC_ViewConsole_RotateUC);
            UC_ViewConsole.RotateC += new ViewConsole.RotateCEventHandler(UC_ViewConsole_RotateC);
            UC_ViewConsole.ReSizeButtonClicked += new ViewConsole.ReSizeEventHandler(UC_ViewConsole_ReSizeButtonClicked); 
            #endregion
        }
        #endregion
        #region 鼠标触摸操作
     
        Storyboard STB_ImageSwitchBackward;
        Storyboard STB_ImageSwitchForward;
        Storyboard STB_ImageSwitchBackwardBack;
        Storyboard STB_ImageSwitchForwardBack;
        bool beginDrag = false;
        Point startPoint;
        Point endPoint;
        double Bound_deltaX = 800 / 3;
        private void SetLeftLine(double deltaX)
        {
            MainShow.RenderTransformOrigin = new Point(1, 0.5);
            STF_MainShow.ScaleX = (Width - deltaX) / Width;
            STF_LastShow.ScaleX = deltaX / Width;
        }
        private void SetRightLine(double deltaX)
        {
            deltaX = -deltaX;
            MainShow.RenderTransformOrigin = new Point(0, 0.5);
            STF_MainShow.ScaleX = (Width - deltaX) / Width;
            STF_NextShow.ScaleX = deltaX / Width;
        }
        private void STB_ImageSwitchForward_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            CurrentIndex = (CurrentIndex + 1) % PictureInfoList.Count;
            DisposeImage();
            ShowImage();
        }
        private void STB_ImageSwitchBackward_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            CurrentIndex = (CurrentIndex - 1 + PictureInfoList.Count) % PictureInfoList.Count;
            DisposeImage();
            ShowImage();
        }
        private void CA_ControlCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (beginDrag && e.LeftButton == MouseButtonState.Pressed)
            {
                ResetRotateState();
                endPoint = e.GetPosition(sender as Canvas);
                double deltaX = endPoint.X - startPoint.X;
                if (deltaX > 0)
                {
                    SetLeftLine(deltaX);
                }
                else
                {
                    SetRightLine(deltaX);
                }
            }
        }
        private void CA_ControlCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (beginDrag)
            {
                beginDrag = false;
                endPoint = e.GetPosition(sender as Canvas);
                double deltaX = endPoint.X - startPoint.X;
                if (Math.Abs(deltaX) < 5)
                {
                    this.UC_ViewConsole.Visibility = (UC_ViewConsole.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
                }
                else
                {
                    if (Math.Abs(deltaX) > Bound_deltaX)
                    {
                        if (deltaX > 0)
                        {
                            STB_ImageSwitchBackward.Begin();
                        }
                        else
                        {
                            STB_ImageSwitchForward.Begin();
                        }
                    }
                    else
                    {
                        if (deltaX > 0)
                        {
                            STB_ImageSwitchBackwardBack.Begin();

                        }
                        else
                        {
                            STB_ImageSwitchForwardBack.Begin();
                        }
                    }
                }
            }
        }
        private void CA_ControlCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                beginDrag = true;
                startPoint = e.GetPosition(sender as Canvas);
            }
        }
        #endregion
        #region 图片显示
        bool isInAnime = false;
        public int CurrentIndex = 0;
        bool[] waitFlag = new bool[3] { false, false, false };
        private bool IsNearCurrentIndex(int i)
        {
            if (i == CurrentIndex - 1 || i == CurrentIndex || i == CurrentIndex + 1)
            { return true; }
            else
            {
                if (CurrentIndex == PictureInfoList.Count - 1 && i == 0)
                {
                    return true;
                }
                if (CurrentIndex == 0 && i == PictureInfoList.Count - 1)
                {
                    return true;
                }
            }
            return false;
        }
        private void GetImageAsync(PictureInfo pi1,PictureInfo pi2,PictureInfo pi3)
        {
            if (pi1.Image == null)
            {
                App.ViewModel.AddViewWorkItem(pi1);
                waitFlag[0] = true;
            }
            else
            {
                (LastShow.Fill as ImageBrush).ImageSource = null;
                (LastShow.Fill as ImageBrush).ImageSource = PictureInfoList[(CurrentIndex - 1 + PictureInfoList.Count) % PictureInfoList.Count].Image;
            }
            if (pi2.Image == null)
            {
                App.ViewModel.AddViewWorkItem(pi2);
                waitFlag[1] = true;
            }
            else
            {
                (MainShow.Fill as ImageBrush).ImageSource = null;
                (MainShow.Fill as ImageBrush).ImageSource = PictureInfoList[CurrentIndex].Image;
            }
            if (pi3.Image == null)
            {
                App.ViewModel.AddViewWorkItem(pi3);
                waitFlag[2] = true;
            }
            else
            {
                (NextShow.Fill as ImageBrush).ImageSource = null;
                (NextShow.Fill as ImageBrush).ImageSource = PictureInfoList[(CurrentIndex + 1) % PictureInfoList.Count].Image;
            }
            App.ViewModel.RunViewWorker();
        }
        private void ViewModel_ViewWorkCompleted()
        {
            if (waitFlag[1])
            {
                (MainShow.Fill as ImageBrush).ImageSource = null;
                (MainShow.Fill as ImageBrush).ImageSource = PictureInfoList[CurrentIndex].Image;
            }
            if (waitFlag[0])
            {
                (LastShow.Fill as ImageBrush).ImageSource = null;
                (LastShow.Fill as ImageBrush).ImageSource = PictureInfoList[(CurrentIndex - 1 + PictureInfoList.Count) % PictureInfoList.Count].Image;
            }
            if (waitFlag[2])
            {
                (NextShow.Fill as ImageBrush).ImageSource = null;
                (NextShow.Fill as ImageBrush).ImageSource = PictureInfoList[(CurrentIndex + 1) % PictureInfoList.Count].Image;
            }
        } 
        public void ShowImage()
        {
            SetLeftLine(0);
            SetRightLine(0);
            GetImageAsync(PictureInfoList[(CurrentIndex - 1 + PictureInfoList.Count) % PictureInfoList.Count], PictureInfoList[CurrentIndex], PictureInfoList[(CurrentIndex + 1) % PictureInfoList.Count]);
        }
        private void DisposeImage()
        {
            for (int i = 0; i < PictureInfoList.Count; i++)
            {
                if (!IsNearCurrentIndex(i) && PictureInfoList[i].Image != null)
                {
                    if (PictureInfoList[i].Image.StreamSource != null)
                    {
                        PictureInfoList[i].Image.StreamSource.Dispose();
                    }
                    PictureInfoList[i].Image = null;
                }
            }
        }
        private void DisposeAllImage()
        {
            (MainShow.Fill as ImageBrush).ImageSource = null;
            (LastShow.Fill as ImageBrush).ImageSource = null;
            (NextShow.Fill as ImageBrush).ImageSource = null;
            for (int i = 0; i < PictureInfoList.Count; i++)
            {
                if (PictureInfoList[i].Image != null)
                {
                    if (PictureInfoList[i].Image.StreamSource != null)
                    {
                        PictureInfoList[i].Image.StreamSource.Dispose();
                    }
                    PictureInfoList[i].Image = null;
                }
            }
        }
        #endregion
        #region 图片显示模式
        int strechIndex = 0;
        List<Stretch> streches = new List<Stretch>(3) { Stretch.None, Stretch.Uniform, Stretch.Fill };
        private void UC_ViewConsole_ReSizeButtonClicked(object sender, EventArgs e)
        {
            strechIndex = (strechIndex + 1) % streches.Count;
            (MainShow.Fill as ImageBrush).Stretch = streches[strechIndex];
            (LastShow.Fill as ImageBrush).Stretch = streches[strechIndex];
            (NextShow.Fill as ImageBrush).Stretch = streches[strechIndex];
        }
        #endregion
        #region 图片旋转
        int angel = 0;
        Storyboard STB_RotateUC90;
        Storyboard STB_RotateUC180;
        Storyboard STB_RotateUC270;
        Storyboard STB_RotateUC360;
        Storyboard STB_RotateC90;
        Storyboard STB_RotateC180;
        Storyboard STB_RotateC270;
        Storyboard STB_RotateC360;
        private void ResetRotateState()
        {
            STB_RotateUC90.Remove();
            STB_RotateUC180.Remove();
            STB_RotateUC270.Remove();
            STB_RotateUC360.Remove();
            STB_RotateC90.Remove();
            STB_RotateC180.Remove();
            STB_RotateC270.Remove();
            STB_RotateC360.Remove();
            angel = 0;
        }
        private void UC_ViewConsole_RotateUC(object sender, int angle)
        {
            if (isInAnime) { return; }
            switch (this.angel)
            {
                case 0: { isInAnime = true; STB_RotateUC90.Begin(); } break;
                case 90: { isInAnime = true; STB_RotateUC180.Begin(); } break;
                case 180: { isInAnime = true; STB_RotateUC270.Begin(); } break;
                case 270: { isInAnime = true; STB_RotateUC360.Begin(); } break;
                default: return;
            }
        }
        private void UC_ViewConsole_RotateC(object sender, int angle)
        {
            if (isInAnime) { return; }
            switch (this.angel)
            {
                case 0: { isInAnime = true; STB_RotateC90.Begin(); } break;
                case 90: { isInAnime = true; STB_RotateC360.Begin(); } break;
                case 180: { isInAnime = true; STB_RotateC270.Begin(); } break;
                case 270: { isInAnime = true; STB_RotateC180.Begin(); } break;
                default: return;
            }
        }
        #endregion 
        private List<PictureInfo> pictureInfoList;
        public List<PictureInfo> PictureInfoList
        {
            get { return pictureInfoList; }
            set { pictureInfoList = value; }
        }
        public delegate void CloseEventHandler();
        public event CloseEventHandler Closed;
    }
}