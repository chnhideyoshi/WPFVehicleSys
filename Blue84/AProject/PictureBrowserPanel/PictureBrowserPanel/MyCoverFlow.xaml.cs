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

namespace PictureBrowserPanel
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
            this.MouseDown += (sender, e) =>
            {
                if (isInAnime) { return; }
                if (e.GetPosition(this).X > 500)
                {
                    if (StartIndex < PictureInfoList.Count - 1)
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

            };
            for (int i = 0; i < controlList.Count; i++)
            {
                controlList[i].MouseDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    int indexs = controlList.IndexOf(sender as Control);
                    if (indexs != -1 && ImageButtonClicked != null)
                    {
                        ImageButtonClicked(sender, pictureInfoList[StartIndex], pictureInfoList);
                    }
                };
            }
            STB_SwitchLeft.Completed += new EventHandler(STB_SwitchLeft_Completed);
            STB_SwitchRight.Completed += new EventHandler(STB_SwitchRight_Completed);
            App.ViewModel.LoadDetailsProgressChanged += (vi) => { RefreshControlBackground(vi); };
            App.ViewModel.DetailWorkCompleted += () =>
            {
                this.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Visible;
            };
        } 
        #endregion
        #region 界面逻辑
        bool isInAnime=false;
        Storyboard STB_SwitchLeft;
        Storyboard STB_SwitchRight;
        private int startIndex = 0;
        private readonly int range = 11;
        private List<Control> controlList = new List<Control>(11);
        public int StartIndex
        {
            get { return startIndex; }
            set
            {
                if (value >= 0 && value <= pictureInfoList.Count - 1)
                {
                    startIndex = value;
                    FillImageByStartIndexAndRange();
                }
            }
        }
        private void STB_SwitchLeft_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex++;
        }
        private void STB_SwitchRight_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex--;
        }
        public delegate void ImageButtonClickEventHandler(object sender, PictureInfo pictureInfo, List<PictureInfo> pictureInfoList);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        #endregion
        #region 数据逻辑
        private List<PictureInfo> pictureInfoList;
        public List<PictureInfo> PictureInfoList
        {
            get { return pictureInfoList; }
            set
            {
                pictureInfoList = value;
                if (pictureInfoList.Count >= 11)
                {
                    StartIndex = 5;
                }
                else
                {
                    StartIndex = PictureInfoList.Count / 2;
                }
            }
        }
        private void FillImageByStartIndexAndRange()
        {
            for (int i = -range / 2; i <= range / 2; i++)
            {
                if (i + StartIndex >= 0 && i + StartIndex <= PictureInfoList.Count - 1)
                {
                    if (PictureInfoList[i + StartIndex].ThumbImage == null)
                    {
                        App.ViewModel.AddWorkingItem(PictureInfoList[i + StartIndex]);
                    }
                    else
                    {
                        controlList[range / 2 + i].Tag = PictureInfoList[i + StartIndex];
                        (controlList[range / 2 + i].Background as ImageBrush).ImageSource = PictureInfoList[i + StartIndex].ThumbImage;
                    }
                    controlList[range / 2 + i].Visibility = Visibility.Visible;
                }
                else
                {
                    controlList[range / 2 + i].Tag = null;
                    (controlList[range / 2 + i].Background as ImageBrush).ImageSource = null;
                    controlList[range / 2 + i].Visibility = Visibility.Hidden;
                }
            }
            App.ViewModel.RunDetailWorker();
        }
        private void RefreshControlBackground(PictureInfo pictureInfo)
        {
            try
            {
                for (int i = 0; i < controlList.Count; i++)
                {
                    PictureInfo v = controlList[i].Tag as PictureInfo;
                    if (v == null) { return; }
                    if (v.AbsolutePath == pictureInfo.AbsolutePath)
                    {
                        (controlList[i].Background as ImageBrush).ImageSource = pictureInfo.ThumbImage;
                    }
                }
            }
            catch { return; }
        } 
        #endregion
	}
}