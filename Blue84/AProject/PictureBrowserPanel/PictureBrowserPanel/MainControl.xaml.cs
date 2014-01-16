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
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl
	{
        public MainControl()
		{
			InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            STB_ExpIn = this.FindResource("STB_ExpIn") as Storyboard;
            STB_ExpIn.Completed += (sender, e) => { isInAnime = false; };
            STB_ExpIn.FillBehavior = FillBehavior.HoldEnd;
            STB_ExpOut = this.FindResource("STB_ExpOut") as Storyboard;
            STB_ExpIn.FillBehavior = FillBehavior.HoldEnd;
            STB_ExpOut.Completed += (sender, e) =>
            {
                CC_MainHolder.Visibility = Visibility.Collapsed;
                isInAnime = false;
            };
            CreateListShow();
        }
        private void InitEventHandler()
        {
            BTN_BackHome.Click += (sender, e) =>
            {
                App.Current.Shutdown();
            };
            this.CKB_Switch.Click += (sender, e) =>
            {
                if (isList)
                {
                    CreateSwicthShow();
                    isList = false;
                }
                else
                {
                    CreateListShow();
                    isList = true;
                }
            };
            BTN_Switch.Click += (sender, e) =>
            {
                ShowWindow();
            };
        } 
        #endregion

        #region 界面逻辑
        Storyboard STB_ExpOut;
        Storyboard STB_ExpIn;

        ListShow UC_ListShow = null;
        SwitchShower UC_SwicthShow = null;
        bool isInAnime = false;
        bool isList = true;

        private void GoToListState()
        {
            CC_ListShower.Visibility = Visibility.Visible;
            CC_MainHolder.Visibility = Visibility.Collapsed;
            CC_SwitchShower.Visibility = Visibility.Collapsed;
        }
        private void GoToSwitchState()
        {
            CC_ListShower.Visibility = Visibility.Collapsed;
            CC_MainHolder.Visibility = Visibility.Collapsed;
            CC_SwitchShower.Visibility = Visibility.Visible;
        }
        private void GoToExpanderState()
        {
            CC_MainHolder.Opacity = 0;
            CC_MainHolder.Visibility = Visibility.Visible;
            isInAnime = true;
            STB_ExpIn.Begin();
        }

        private void CreateSwicthShow()
        {
            if (this.CC_SwitchShower.Content == null || CC_SwitchShower.Content as SwitchShower == null)
            {
                UC_SwicthShow = new SwitchShower();
                UC_SwicthShow.ImageButtonClicked += UC_ImageButtonClicked;
                CC_SwitchShower.Content = UC_SwicthShow;
                GoToSwitchState();
            }
            else
            {
                GoToSwitchState();
            }
        }
        private void CreateListShow()
        {
            if (this.CC_ListShower.Content == null || CC_ListShower.Content as ListShow == null)
            {
                UC_ListShow = new ListShow();
                UC_ListShow.ImageButtonClicked += UC_ImageButtonClicked;
                this.CC_ListShower.Content = UC_ListShow;
                GoToListState();
            }
            else
            {
                GoToListState();
            }
        }
        private void CreateExpanderShow(PictureInfo pictrueInfo, List<PictureInfo> pictureList)
        {
            ExpandShow eps = null;
            if (this.CC_MainHolder.Content == null || CC_MainHolder.Content as ExpandShow == null)
            {
                eps = new ExpandShow();
                eps.Closed += () =>
                {
                    isInAnime = true;
                    STB_ExpOut.Begin();
                    //CC_MainHolder.Visibility = Visibility.Collapsed;
                };
                CC_MainHolder.Content = eps;
            }
            else
            {
                eps = CC_MainHolder.Content as ExpandShow;
            }
            eps.PictureInfoList = pictureList;
            eps.CurrentIndex = pictureList.IndexOf(pictrueInfo);
            eps.ShowImage();
            GoToExpanderState();
        } 

        private void ShowWindow()
        {
            PopWindowsLib.PopOk pop = new PopWindowsLib.PopOk();
            pop.IsShowGrid = false;
            pop.Text = "扩展功能暂未提供";
            pop.ShowDialog();
        }

        private void UC_ImageButtonClicked(object sender, PictureInfo pictrueInfo, List<PictureInfo> pictureList)
        {
            if (isInAnime) { return; }
            CreateExpanderShow(pictrueInfo, pictureList);
        }
        
        #endregion
        


    }
}