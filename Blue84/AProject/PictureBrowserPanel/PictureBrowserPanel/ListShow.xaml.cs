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
	/// Interaction logic for ListShow.xaml
	/// </summary>
	public partial class ListShow : UserControl
	{
        public ListShow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            ANI_SwitchUpDown = new DoubleAnimation()
            {
                FillBehavior = FillBehavior.HoldEnd,
                By = 100,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            };
        }
        private void InitEventHandler()
        { 
            this.Loaded += (sender, e) => { InitData(); };
            this.BTN_DirDown.Click += (sender, e) => { SwitchDirs(true); }; ;
            this.BTN_DirUp.Click += (sender, e) => { SwitchDirs(false); };
            ANI_SwitchUpDown.Completed += (sender, e) => { isInAnime = false; };
            App.ViewModel.FileSystemWorkCompleted += new ViewModel.FileSystemWorkCompletedEventHandler(ViewModel_FileSystemWorkCompleted);
        }
        #endregion
        #region 界面逻辑
        Storyboard STB_SwitchUpRebound;//反弹动画1
        Storyboard STB_SwitchDownRebound;//反弹动画2
        DoubleAnimation ANI_SwitchUpDown;//上下转移动画
        bool isInAnime = false;
        private void InitStoryboard()
        {
            if (STB_SwitchDownRebound == null)
            {
                double maxtop = STK_MainPanel.ActualHeight - CA_Main.ActualHeight;
                STB_SwitchDownRebound = this.FindResource("STB_SwitchDownRebound") as Storyboard;
                STB_SwitchDownRebound.Completed += (sender, e) => { isInAnime = false; };
                DoubleAnimationUsingKeyFrames DANIF= STB_SwitchDownRebound.Children[0] as DoubleAnimationUsingKeyFrames;
                if (maxtop >= 0)
                {
                    DANIF.KeyFrames[0].Value = -maxtop;
                    DANIF.KeyFrames[1].Value =- maxtop - 12;
                    DANIF.KeyFrames[2].Value = -maxtop - 15;
                    DANIF.KeyFrames[3].Value = -maxtop - 12;
                    DANIF.KeyFrames[4].Value = -maxtop;
                }
            }
            if (STB_SwitchUpRebound == null)
            {
                STB_SwitchUpRebound = this.FindResource("STB_SwitchUpRebound") as Storyboard;
                STB_SwitchUpRebound.Completed += (sender, e) => { isInAnime = false; };
            }
        }//初始化反弹动画
        private void GoToNonPictureState()
        {
            BTN_DirDown.Visibility = Visibility.Collapsed;
            BTN_DirUp.Visibility = Visibility.Collapsed;
        }//界面状态跳转到没有图片的状态
        private void SwitchDirs(bool isUp)
        {
            if (isInAnime) { return; }
            if(CA_Main.ActualHeight>=STK_MainPanel.ActualHeight){return;}
            InitStoryboard();
            if (isUp)
            {
                if (Canvas.GetTop(STK_MainPanel) - 100 < -(STK_MainPanel.ActualHeight - CA_Main.ActualHeight))
                {
                    if (STB_SwitchDownRebound != null)
                        isInAnime = true;
                        STB_SwitchDownRebound.Begin();
                }
                else
                {
                    ANI_SwitchUpDown.By = -100;
                    isInAnime = true;
                    STK_MainPanel.BeginAnimation(Canvas.TopProperty, ANI_SwitchUpDown);
                }
            }
            else
            {
                if (Canvas.GetTop(STK_MainPanel) + 100 > 0)
                {
                    if (STB_SwitchUpRebound != null)
                        isInAnime = true;
                        STB_SwitchUpRebound.Begin();
                }
                else
                {
                    ANI_SwitchUpDown.By = 100;
                    isInAnime = true;
                    STK_MainPanel.BeginAnimation(Canvas.TopProperty, ANI_SwitchUpDown);
                }
            }
        }//点击上下按钮执行的操作
        private void HorizontalScrollPanel_ImageButtonClicked(object sender, PictureInfo pictureInfo, List<PictureInfo> pictureList)
        {
            if (ImageButtonClicked != null)
            {
                ImageButtonClicked(sender, pictureInfo, pictureList);
            }
        }
        public delegate void ImageButtonClickEventHandler(object sender, PictureInfo pictureInfo, List<PictureInfo> pictureList);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        #endregion
        #region 数据逻辑
        private List<List<PictureInfo>> allPictureInfoMatrix;
        public List<List<PictureInfo>> AllPictureInfoMatrix
        {
            get { return allPictureInfoMatrix; }
            set
            {
                allPictureInfoMatrix = value;
                LoadControls();
            }
        }
        void ViewModel_FileSystemWorkCompleted()
        {
            AllPictureInfoMatrix = App.ViewModel.GetAllPictureInfoMatrix();
        }
        private void InitData()
        {
            App.ViewModel.RunFileSystemWorker();
        }
        private void LoadControls()
        {
            if (allPictureInfoMatrix.Count != 0)
            {
                STK_MainPanel.Children.Clear();
                for (int i = 0; i < allPictureInfoMatrix.Count; i++)
                {
                    HorizontalScrollPanel hsp = new HorizontalScrollPanel();
                    hsp.ImageButtonClicked += new HorizontalScrollPanel.ImageButtonClickEventHandler(HorizontalScrollPanel_ImageButtonClicked);
                    hsp.PictureList = allPictureInfoMatrix[i];
                    STK_MainPanel.Children.Add(hsp);
                }
            }
            else
            {
                GoToNonPictureState();
            }
        } 
        #endregion
        #region 测试数据
        bool isDesign=false;
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
            AllPictureInfoMatrix = App.ViewModel.GetAllPictureInfoMatrix();
        }
        #endregion
       
	}
}