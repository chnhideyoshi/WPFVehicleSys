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

namespace Settings
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
            InIt();
            InitEventHandler();
		}
        #region panels
        _GControl UC_3GControl = null;
        TimeControl UC_TimeControl = null;
        DTV UC_DTVControl = null;
        GPSControl UC_GPSControl = null;
        ScreenControl UC_ScreenControl = null;
        SoundControl UC_SoundControl = null;
        BlueToothControl UC_BlueToothControl = null;
        #endregion
        private void InitEventHandler()
        {
            this.BTN_3G.Click += (sender, e) => { ShowSubPanel(UC_3GControl,typeof(_GControl)); };
            this.BTN_Date.Click += (sender, e) => { ShowSubPanel(UC_TimeControl, typeof(TimeControl)); };
            this.BTN_DTV.Click += (sender, e) => { ShowSubPanel(UC_DTVControl, typeof(DTV)); };
            this.BTN_gps.Click += (sender, e) => { ShowSubPanel(UC_GPSControl, typeof(GPSControl)); };
            this.BTN_Screen.Click += (sender, e) => { ShowSubPanel(UC_ScreenControl, typeof(ScreenControl)); };
            this.BTN_Sound.Click += (sender, e) => { ShowSubPanel(UC_SoundControl, typeof(SoundControl)); };
            this.BTN_BlueTooth.Click += (sender, e) => { ShowSubPanel(UC_BlueToothControl, typeof(BlueToothControl)); };

            this.BTN_Back.Click += (sender, e) => 
            {
                if (!isInSub)
                {
                    App.Current.Shutdown();
                }
                else
                {
                    GoToMainMenu();
                }
            };
            this.BTN_Confirm.Click += (sender, e) =>
            {
                SendMessages();
            };
        }

     
       
        #region private
        bool isInSub = false;
        #endregion
        private void InIt()
        {
                Topmost = (App.Current as App).IsTopMost;
        }
        public void ShowSubPanel(UserControl uc,Type t)
        {
            if (uc == null)
            {
                uc = (UserControl)Activator.CreateInstance(t);
            }
            CC_SubHost.Content = uc;
            CC_SubHost.Visibility = Visibility.Visible;
            CA_Main.Visibility = Visibility.Collapsed;
            BTN_Confirm.Visibility = Visibility.Visible;
            BTN_Back.Tag = "返回";
            isInSub = true;
        }
        private void GoToMainMenu()
        {
            CC_SubHost.Visibility = Visibility.Collapsed ;
            CA_Main.Visibility = Visibility.Visible;
            BTN_Back.Tag = "退出";
            BTN_Confirm.Visibility = Visibility.Collapsed;
            isInSub = false;
        }
        private void SendMessages()
        {
            if (CC_SubHost.Content as TimeControl != null) { (CC_SubHost.Content as TimeControl).Confirm(); return; };
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
            //if(xx!=null){xx.Confirm();return; }
        }

	}
}