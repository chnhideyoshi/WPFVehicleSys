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
	/// Interaction logic for SwitchShow.xaml
	/// </summary>
	public partial class SwitchShow : UserControl
	{
		public SwitchShow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {

        }
        private void InitEventHandler()
        {
            UC_CoverFlow.ImageButtonClicked += (sender, videoInfo, videoInfoList) =>
            {
                if (ItemClick != null)
                {
                    ItemClick(videoInfo, AllVideoInfoList);
                }
            };
            UC_CoverFlow.IndexChanged += (list, index) =>
            {
                if (list.Count <= index || index < 0) { return; }
                Title = list[index].Name;
            };
            Loaded += (sender, e) => { InitData(); };
        }
        public delegate void ItemClickEventHandler(VideoInfo videoInfo, List<VideoInfo> videoInfoList);
        public event ItemClickEventHandler ItemClick; 
        #endregion
        #region 数据
        public string Title
        {
            get { return TBK_Title.Text; }
            set { TBK_Title.Text = value; }
        }
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
            if (App.ViewModel.AllVideoInfoList == null)
            {
                App.ViewModel.FileSystemWorkCompleted += () =>
                    {
                        AllVideoInfoList =App.ViewModel.AllVideoInfoList;
                    };
                App.ViewModel.RunFileSystemWorker();
            }
            else
            {
                AllVideoInfoList = App.ViewModel.AllVideoInfoList;
            }
        }
        private void LoadControls()
        {
            UC_CoverFlow.VideoInfoList = allVideoInfoList;
            Title = AllVideoInfoList[0].Name;
        }
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
            AllVideoInfoList = App.ViewModel.GetAllVideoInfoList();
        }
        #endregion
    }
}