using System;
using System.Collections.Generic;
using System.Linq;
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

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for MyCoverFlow.xaml
	/// </summary>
	public partial class MyCoverFlow : UserControl
	{
		public MyCoverFlow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            STB_SwitchLeft = this.FindResource("STB_SwitchLeft") as Storyboard;
            STB_SwitchRight = this.FindResource("STB_SwitchRight") as Storyboard;
            #region control
            controlList.Add(CT_L5);
            controlList.Add(CT_L4);
            controlList.Add(CT_L3);
            controlList.Add(CT_L2);
            controlList.Add(CT_L1);
            controlList.Add(CT_C);
            controlList.Add(CT_R1);
            controlList.Add(CT_R2);
            controlList.Add(CT_R3);
            controlList.Add(CT_R4);
            controlList.Add(CT_R5);
            for (int i = 0; i < controlList.Count; i++)
                controlList[i].Background = new ImageBrush();
            #endregion
        }
        private void InitEventHandler()
        {
            this.MouseDown += new MouseButtonEventHandler(MyCoverFlow_MouseDown);
            for (int i = 0; i < controlList.Count; i++)
            {
                controlList[i].MouseDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    int indexs = controlList.IndexOf(sender as Control);
                    if (indexs != -1 && ImageButtonClicked != null)
                    {
                        ImageButtonClicked(sender, VideoInfoList[StartIndex], VideoInfoList);
                    }
                };
            }
            STB_SwitchLeft.Completed += new EventHandler(STB_SwitchLeft_Completed);
            STB_SwitchRight.Completed += new EventHandler(STB_SwitchRight_Completed);
            App.ViewModel.LoadDetailsProgressChanged += new ViewModel.LoadDetailsProgressChangedEventHandler(ViewModel_LoadDetailsProgressChanged);
        }
        #endregion
        #region 界面逻辑
        private void MyCoverFlow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isInAnime) { return; }
            if (e.GetPosition(this).X > 500)
            {
                if (StartIndex < VideoInfoList.Count - 1)
                {
                    isInAnime = true;
                    STB_SwitchLeft.Begin();
                }
            }
            else if (e.GetPosition(this).X < 300)
            {
                if (StartIndex > 0)
                {
                    isInAnime = true;
                    STB_SwitchRight.Begin();
                }
            }
        } 
        private void STB_SwitchLeft_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex++;
            if (IndexChanged != null)
            {
                IndexChanged(VideoInfoList, StartIndex);
            }
        }
        private void STB_SwitchRight_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex--;
            if (IndexChanged != null)
            {
                IndexChanged(VideoInfoList, StartIndex);
            }
        }
        bool isInAnime = false;
        Storyboard STB_SwitchLeft;
        Storyboard STB_SwitchRight;
        public delegate void ImageButtonClickEventHandler(object sender, VideoInfo videoInfo, List<VideoInfo> videoInfoList);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        public delegate void IndexChangedEventHandler(List<VideoInfo> videoInfoList, int newIndex);
        public event IndexChangedEventHandler IndexChanged;
        #endregion 
        #region 数据逻辑
        private readonly int controlNumber = 11; //
        private List<Control> controlList = new List<Control>(11);
        private int startIndex = 0;
        private List<VideoInfo> videoInfoList;
        public List<VideoInfo> VideoInfoList
        {
            get { return videoInfoList; }
            set
            {
                videoInfoList = value;
                if (videoInfoList.Count >= 11)
                {
                    StartIndex = 5;
                }
                else
                {
                    StartIndex = VideoInfoList.Count / 2;
                }
            }
        }
        public int StartIndex
        {
            get { return startIndex; }
            set
            {
                if (value >= 0 && value <= VideoInfoList.Count - 1)
                {
                    startIndex = value;
                    FillImageByStartIndexAndRange();
                }
            }
        }
        private void FillImageByStartIndexAndRange()
        {
            for (int i = -controlNumber / 2; i <= controlNumber / 2; i++)
            {
                if (i + StartIndex >= 0 && i + StartIndex <= VideoInfoList.Count - 1)
                {
                    if (VideoInfoList[i + StartIndex].ThumbImage == null)
                    {
                        (controlList[controlNumber / 2 + i].Background as ImageBrush).ImageSource = this.FindResource("BI_DefaultBack") as BitmapImage;
                    }
                    else
                    {
                        (controlList[controlNumber / 2 + i].Background as ImageBrush).ImageSource = null;
                        (controlList[controlNumber / 2 + i].Background as ImageBrush).ImageSource = VideoInfoList[i + StartIndex].ThumbImage;
                    }
                    controlList[controlNumber / 2 + i].Visibility = Visibility.Visible;
                }
                else
                {
                    controlList[controlNumber / 2 + i].Visibility = Visibility.Hidden;
                }
            }
            //App.ViewModel.RunDetailWorker();
        }
        private void ViewModel_LoadDetailsProgressChanged(VideoInfo videoInfo)
        {
            RefreshControlBackground(videoInfo);
        }
        private void RefreshControlBackground(VideoInfo videoInfo)
        {
            try
            {
                for (int i = 0; i < controlList.Count; i++)
                {
                    VideoInfo v = controlList[i].Tag as VideoInfo;
                    if (v.AbsolutePath == videoInfo.AbsolutePath || videoInfo.ThumbImage != null)
                    {
                        (controlList[i].Background as ImageBrush).ImageSource = videoInfo.ThumbImage;
                    }
                }
            }
            catch { return; }
        } 
        #endregion

      
    }
}