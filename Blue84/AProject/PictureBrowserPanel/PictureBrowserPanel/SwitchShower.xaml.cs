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

namespace PictureBrowserPanel
{
	/// <summary>
	/// Interaction logic for SwitchShower.xaml
	/// </summary>
	public partial class SwitchShower : UserControl
	{
		public SwitchShower()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            Title = "";
        }   
        private void InitEventHandler()
        {
            UC_CoverFlow.ImageButtonClicked += new MyCoverFlow.ImageButtonClickEventHandler(UC_CoverFlow_ImageButtonClicked);
            BTN_SwitchLeft.Click += (sende, e) => 
            {
                if (allPictureInfoMatrix.Count == 0) { return; }
                StartIndex = (StartIndex + 1) % allPictureInfoMatrix.Count;
                SwitchDirectoryData();
            };
            this.BTN_SwitchRight.Click += (sende, e) =>
            {
                if (allPictureInfoMatrix.Count == 0) { return; }
                StartIndex = (StartIndex - 1 + allPictureInfoMatrix.Count) % allPictureInfoMatrix.Count;
                SwitchDirectoryData();
            };
            this.Loaded += (sender, e) => { InitData(); };
        }   
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
        public string Title
        {
            get { return TBK_title.Text; }
            set { TBK_title.Text = value; }
        }//目录名显示属性
        private void InitData()
        {
            AllPictureInfoMatrix = App.ViewModel.GetAllPictureInfoMatrix();
        }//获取数据
        public void LoadControls()
        {
            if (allPictureInfoMatrix.Count != 0)
            {
                SwitchDirectoryData();
            }
            else
            {
                GoToNonPictureState();
            }
        }//加载控件
        private void GoToNonPictureState()
        {
            //选择性实现
        }//当发现无图片可读的时候采取的操作
        private void SwitchDirectoryData()
        {
            UC_CoverFlow.PictureInfoList = allPictureInfoMatrix[StartIndex];
            Title = allPictureInfoMatrix[StartIndex][0].DirectoryName;
        }
        #endregion
        #region 界面逻辑
        public int StartIndex = 0;
        
        void UC_CoverFlow_ImageButtonClicked(object sender, PictureInfo p, List<PictureInfo> list)
        {
            if (ImageButtonClicked != null)
            {
                ImageButtonClicked(sender, p, list);
            }
        }
        public delegate void ImageButtonClickEventHandler(object sender, PictureInfo p, List<PictureInfo> list);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        #endregion
        #region 测试
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
            AllPictureInfoMatrix = App.ViewModel.GetAllPictureInfoMatrix();
        }
        #endregion

    }
}