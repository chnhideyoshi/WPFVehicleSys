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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace SystemHelp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
			// Insert code required on object creation below this point.
        }
        private void Init()
        {
            STB_ReturnToList = this.FindResource("STB_ReturnToList") as Storyboard;
            STB_ReturnToList.Completed += (sender, e) => { isInAnime = false; };
            STB_SwitchToSub = this.FindResource("STB_SwitchToSub") as Storyboard;
            STB_SwitchToSub.Completed += (sender, e) => { isInAnime = false; };
            this.Topmost = (App.Current as App).IsTopMost;
            this.ShowInTaskbar = false;
        }
        private void InitEventHandler()
        {
            UC_MainList.ButtonClick += (module) => { LoadData(module); GoToSubPanel(module); };
            BTN_BackHome.Click += (sender, e) => 
            {
                if (IsInSubPanel)
                {
                    GoToListState();
                }
                else
                {
                    App.Current.Shutdown();
                }
            };
        }
        #region uilogic
        Storyboard STB_SwitchToSub;
        Storyboard STB_ReturnToList;
        bool isInAnime=false;
        public bool IsInSubPanel
        {
            get { return Canvas.GetLeft(grid) < -400; }
        }
        private void GoToSubPanel(Modules module)
        {
            if (isInAnime) { return; }
            BTN_BackHome.Content = "返回";
            LoadData(module);
            isInAnime = true;
            STB_SwitchToSub.Begin();
        }
        private void GoToListState()
        {
            if (isInAnime) { return; }
            BTN_BackHome.Content = "退出";
            isInAnime = true;
            STB_ReturnToList.Begin();
        }
        #endregion
        #region datalogic
        private void LoadData(Modules module)
        {
            string xaml = null;
            UC_MainShow.Title = App.ViewModel.GetTitle(module);
            switch (module)
            {
                case Modules.Book: { xaml = App.ViewModel.GetContent("Book"); } break;
                case Modules.Music: { xaml = App.ViewModel.GetContent("Music"); } break;
                case Modules.Video: { xaml = App.ViewModel.GetContent("Video"); } break;
                case Modules.Picture: { xaml = App.ViewModel.GetContent("Picture"); } break;
                case Modules.Product: { xaml = App.ViewModel.GetContent("Product"); } break;
                case Modules.GPS: { xaml = App.ViewModel.GetContent("GPS"); } break;
                case Modules.FM: { xaml = App.ViewModel.GetContent("FM"); } break;
                case Modules.Explorer: { xaml = App.ViewModel.GetContent("Explorer"); } break;
                default: break;
            }
            if (xaml == null) { return; }
            try
            {
                UC_MainShow.HelpContentText = xaml;
            }
            catch { return; }
        }
        //private void LoadData(Modules module)
        //{
        //    FlowDocument fd = null;
        //    switch (module)
        //    {
        //        case Modules.Book: { fd = App.ViewModel.GetControlObject("Book"); } break;
        //        case Modules.Music: { fd = App.ViewModel.GetControlObject("Music"); } break;
        //        case Modules.Video: { fd = App.ViewModel.GetControlObject("Video"); } break;
        //        case Modules.Picture: { fd = App.ViewModel.GetControlObject("Picture"); } break;
        //        case Modules.Product: { fd = App.ViewModel.GetControlObject("Product"); } break;
        //        case Modules.GPS: { fd = App.ViewModel.GetControlObject("GPS"); } break;
        //        case Modules.FM: { fd = App.ViewModel.GetControlObject("FM"); } break;
        //        case Modules.Explorer: { fd = App.ViewModel.GetControlObject("Explorer"); } break;
        //        default: break;
        //    }
        //    if (fd == null) { return; }
        //    UC_MainShow.HelpContent = fd;
        //}
        #endregion
    }
}