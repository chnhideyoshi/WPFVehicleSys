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
	/// Interaction logic for HorizontalScrollPanel.xaml
	/// </summary>
	public partial class HorizontalScrollPanel : UserControl
	{
		public HorizontalScrollPanel()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            ANI_ForlderLeft = this.FindResource("FolderLeft") as DoubleAnimation;
            ANI_ForlderRight = this.FindResource("FolderRight") as DoubleAnimation;
        }
        private void InitEventHandler()
        {
            App.ViewModel.LoadDetailsProgressChanged += new ViewModel.LoadDetailsProgressChangedEventHandler(ViewModel_LoadDetailsProgressChanged);
            BTN_Left1st.MouseDown += (sender, e) =>
            {
                if (!isInAnime)
                {
                    SwitchLeft();
                    isInAnime = true;
                    STK_Main.BeginAnimation(Canvas.LeftProperty, ANI_ForlderLeft);
                }
            };
            BTN_Right1st.MouseDown += (sender, e) =>
            {
                if (!isInAnime)
                {
                    isInAnime = true;
                    STK_Main.BeginAnimation(Canvas.LeftProperty, ANI_ForlderRight);
                }
            };
            ANI_ForlderLeft.Completed += (sender, e) =>
            {
                isInAnime = false;
            };
            ANI_ForlderRight.Completed += (sender, e) =>
            {
                SwitchRight();
                isInAnime = false;
            };
        }
        #endregion
        #region 界面逻辑
        public static readonly int ButtonMaxCount = 12;
        DoubleAnimation ANI_ForlderLeft;
        DoubleAnimation ANI_ForlderRight;
        bool isInAnime = false;
        Point recordPos;
        private double GetDistence(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        private void Control_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            recordPos = e.GetPosition(CA_Main);
        }
        private void Control_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (recordPos != null && GetDistence(recordPos, e.GetPosition(CA_Main)) <= 4)
            {
                ImageButton_Click(sender, e);
            }
        }
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImageButtonClicked != null)
            {
                ImageButtonClicked(sender, (sender as Control).Tag as PictureInfo, this.pictureList);
            }
        }
        private void SwitchRight()
        {
            startIndex = (startIndex + ButtonMaxCount / 2) % pictureList.Count;
            FillImageByIndex();
        }
        private void SwitchLeft()
        {
            startIndex = (startIndex - ButtonMaxCount / 2 + PictureList.Count * 13) % PictureList.Count;
            FillImageByIndex();
        }
        private void FillImageByIndex()
        {
            for (int i = 0; i < STK_Main.Children.Count; i++)
            {
                Control c = STK_Main.Children[i] as Control;
                int targetIndex = (startIndex + i) % pictureList.Count;
                if (c.Tag as PictureInfo != null)
                {
                    (c.Foreground as ImageBrush).ImageSource = null;
                    DisposeImage(c.Tag as PictureInfo);
                    c.Tag = null;
                }
                c.Tag = PictureList[targetIndex];
                if (pictureList[targetIndex].ThumbImage == null)
                {
                    (c.Foreground as ImageBrush).ImageSource = null;
                    App.ViewModel.AddWorkingItem(PictureList[targetIndex]);
                }
                else
                {
                    (c.Foreground as ImageBrush).ImageSource = pictureList[targetIndex].ThumbImage;
                }
            }
            App.ViewModel.RunDetailWorker();
        }
        private void DisposeImage(PictureInfo pictureInfo)
        {
            if (pictureInfo.ThumbImage != null)
            {
                if (pictureInfo.ThumbImage.StreamSource != null)
                {
                    pictureInfo.ThumbImage.StreamSource.Dispose();
                }
                pictureInfo.ThumbImage = null;
            }
        }
        public delegate void ImageButtonClickEventHandler(object sender, PictureInfo p, List<PictureInfo> list);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        #endregion
        #region 数据逻辑
        int startIndex = 0;
        private List<PictureInfo> pictureList;
        public List<PictureInfo> PictureList
        {
            get { return pictureList; }
            set
            {
                pictureList = value;
                LoadControl();
            }
        }
      
        private void ViewModel_LoadDetailsProgressChanged(PictureInfo pictureInfo)
        {
            RefreshButtonBackground(pictureInfo);
        }
        private void LoadControl()
        {
            STK_Main.Children.Clear();
            startIndex = 0;
            for (int i = 0; i < ButtonMaxCount; i++)
            {
                Control b = new Control();
                b.Foreground = new ImageBrush();
                b.Style = this.FindResource("ImageControlStyle1") as Style;
                b.PreviewMouseDown += new MouseButtonEventHandler(Control_PreviewMouseDown);
                b.PreviewMouseUp += new MouseButtonEventHandler(Control_PreviewMouseUp);
                STK_Main.Children.Add(b);
            }
            FillImageByIndex();
        }
        public void RefreshButtonBackground(PictureInfo pictureInfo)
        {
            for (int i = 0; i < STK_Main.Children.Count; i++)
            {
                Control b = STK_Main.Children[i] as Control;
                if ((b.Tag as PictureInfo).AbsolutePath == pictureInfo.AbsolutePath)
                {
                    (b.Foreground as ImageBrush).ImageSource = pictureInfo.ThumbImage;
                }
            }
        } 
        #endregion

    }
}