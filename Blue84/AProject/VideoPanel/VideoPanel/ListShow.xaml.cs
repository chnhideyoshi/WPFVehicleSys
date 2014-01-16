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
	/// Interaction logic for ListShow2.xaml
	/// </summary>
	public partial class ListShow : UserControl
	{
		public ListShow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
            InitData(); 
		}
        #region 初始化
        ManipTest.TouchMouseWithRebound touchClass = new ManipTest.TouchMouseWithRebound();
        private void InitEventHandler()
        {
            #region 滑竿控制
            this.CA_ExpSL.PreviewMouseMove += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        double rate = e.GetPosition(CA_ExpSL).Y / CA_ExpSL.ActualHeight;
                        touchClass.ResetBack(false);
                        SL_Down.Value = rate * SL_Down.Maximum;
                        Canvas.SetTop(WP_Main, -(touchClass.MaxTop) * rate);
                    }
                };
            touchClass.EndOperation += () =>
             {
                 if (touchClass.MaxTop > 0)
                 {
                     SL_Down.Value = SL_Down.Maximum * (-Canvas.GetTop(WP_Main) / touchClass.MaxTop);
                 }
             }; 
            #endregion
            App.ViewModel.LoadDetailsProgressChanged += new ViewModel.LoadDetailsProgressChangedEventHandler(ViewModel_LoadDetailsProgressChanged);
            this.Loaded += (sender, e) => 
            {
                InitData(); 
            };
        }
        private void Init()
        {
            try
            {
                touchClass.InitTouchListOperation(CA_Main, WP_Main);
            }
            catch { return; }
        }
        #endregion
        #region 界面逻辑
        public bool IsNowVisible
        {
            get 
            {
                ContentControl cc = this.Parent as ContentControl;
                if (cc == null) { return false; }
                return cc.Visibility == Visibility.Visible;
            }
        }
        private double? recordPos;
        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (recordPos.HasValue && Math.Abs(Canvas.GetTop(WP_Main) - recordPos.Value) <= 1)
            {
                if (ItemClick != null)
                {
                    ItemClick((sender as Button).Tag as VideoInfo, AllVideoInfoList);
                }
            }
        } 
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            recordPos = Canvas.GetTop(WP_Main);
        }
        public delegate void ItemClickEventHandler(VideoInfo videoInfo, List<VideoInfo> videoInfoList);
        public event ItemClickEventHandler ItemClick; 
        #endregion
        #region 数据加载
        List<VideoInfo> allVideoInfoList;
        public List<VideoInfo> AllVideoInfoList
        {
            get { return allVideoInfoList; }
            set
            {
                allVideoInfoList = value;
                if (value.Count != 0)
                {
                     LoadControls();
                }
            }
        }
        private void InitData()
        {
            App.ViewModel.FileSystemWorkCompleted += () =>
            {
                AllVideoInfoList = App.ViewModel.GetAllVideoInfoList();
            };
            App.ViewModel.RunFileSystemWorker();
        }
        private void ViewModel_LoadDetailsProgressChanged(VideoInfo pi)
        {
            RefreshControlBackground(pi);
        }
        private void LoadControls()
        {
            WP_Main.Children.Clear();
            for (int i = 0; i < AllVideoInfoList.Count; i++)
            {
                Button b = new Button();
                b.Background = new ImageBrush();
                b.Style = this.FindResource("ImageButton") as Style;
                b.Content = AllVideoInfoList[i].Name;
                b.Tag = AllVideoInfoList[i];
                b.PreviewMouseDown += new MouseButtonEventHandler(Button_PreviewMouseDown);
                b.PreviewMouseUp += new MouseButtonEventHandler(Button_PreviewMouseUp);
                WP_Main.Children.Add(b);
            }
            App.ViewModel.AddWorkingItem(AllVideoInfoList,0,AllVideoInfoList.Count-1);
            App.ViewModel.RunDetailWorker();
        }
        private void RefreshControlBackground(VideoInfo videoInfo)
        {
            if (!this.IsVisible) { return; }
            for (int i = 0; i < WP_Main.Children.Count; i++)
            {
                if (((WP_Main.Children[i] as Button).Tag as VideoInfo).AbsolutePath == videoInfo.AbsolutePath)
                {
                    if (videoInfo.ThumbImage != null)
                    {
                        ((WP_Main.Children[i] as Button).Background as ImageBrush).ImageSource = videoInfo.ThumbImage;
                    }
                    else
                    {
                        ((WP_Main.Children[i] as Button).Background as ImageBrush).ImageSource = this.FindResource("BI_DefaultBack") as BitmapImage;
                    }
                }
            }
        }
        #endregion
        #region 测试界面的属性
        bool isDesign = false;
        public bool IsDesign
        {
            get { return isDesign; }
            set
            {
                isDesign = value;
                if (value)
                {
                    LoadTestData();
                }
            }
        }
        private void LoadTestData()
        {
            AllVideoInfoList = App.ViewModel.GetAllVideoInfoList();
        }
        #endregion
        
    }
}