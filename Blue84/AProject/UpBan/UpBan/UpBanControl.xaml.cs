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

namespace UpBan
{
	/// <summary>
	/// Interaction logic for UpBanControl.xaml
	/// </summary>
	public partial class UpBanControl : UserControl
	{
		public UpBanControl()
		{
			this.InitializeComponent();
            InitTimer();
            GoToDefaultState();
        }

        private void GoToDefaultState()
        {
            HideMuteIcon();
        }
        #region 定时器设置
        public int RefreshSeconds = 1;
        private int tick = 0;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
        private void InitTimer()
        {
            TBK_Time.Text = DateTime.Now.GetDateTimeFormats('f')[0].ToString();
            timer.Interval = TimeSpan.FromSeconds(RefreshSeconds);
            timer.Tick += (sender, e) =>
            {
                if (tick % TimeCheckInterval == 0)
                {
                    SetTime();
                }
                if (tick % USBCheckInterval == 0)
                {
                    SetUSBState();
                }
                if (tick % VolumeCheckInterval == 0)
                {
                    SetMuteState();
                }
                if (tick % _3GCheckInterval == 0)
                {
                    Set3GState();
                }
                if (tick % BlueToothCheckInterval == 0)
                {
                    SetBlueToothState();
                }
                if (tick % BatteryCheckInterval == 0)
                {
                    SetBatteryValue();
                }
                tick++;
            };
            timer.Start();
        } 
        #endregion
        #region 时间设置
        private int TimeCheckInterval=5;
        private void SetTime()
        {
            TBK_Time.Text = DateTime.Now.GetDateTimeFormats('f')[0].ToString();
        }
        #endregion
        #region USB状态
        private int USBCheckInterval = 5;
        private void ShowUSBIcon()
        {
            GD_USB.Visibility = Visibility.Visible;
        }
        private void HideUSBIcon()
        {
            GD_USB.Visibility = Visibility.Collapsed;
        }
        private void SetUSBState()
        {
            if (LocalMethod.GetUSBExistence())
            {
                ShowUSBIcon();
            }
            else
            {
                HideUSBIcon();
            }
        }
        #endregion
        #region 电池状态
        private int BatteryCheckInterval = 5;
        private void SetBatteryValue()
        {
            double maxium = Rec_Battery.MaxWidth;
            double percentage = LocalMethod.GetBatteryPercentage();
            Rec_Battery.Width = maxium * percentage;
        }
        #endregion
        #region 蓝牙状态
        private int BlueToothCheckInterval = 5;
        private void ShowBlueToothIcon()
        {
            GD_BlueTooth.Visibility = Visibility.Visible;
        }
        private void HideBlueToothIcon()
        {
            GD_BlueTooth.Visibility = Visibility.Collapsed;
        }
        private void SetBlueToothState()
        {
            if (LocalMethod.GetBlueToothExistence())
            {
                ShowBlueToothIcon();
            }
            else
            {
                HideBlueToothIcon();
            }
        }
        #endregion
        #region 3G状态
        private int _3GCheckInterval = 5;
        private void Show3GIcon()
        {
            GD_3G.Visibility = Visibility.Visible;
        }
        private void Hide3GIcon()
        {
            GD_3G.Visibility = Visibility.Collapsed;
        }
        private void Set3GState()
        {
            if (LocalMethod.Get3GExistence())
            {
                Show3GIcon();
            }
            else
            {
                Hide3GIcon();
            }
        }
        #endregion
        #region 声音状态
        private int VolumeCheckInterval = 5;
        private void ShowMuteIcon()
        {
            GD_Mute.Visibility = Visibility.Visible;
        }
        private void HideMuteIcon()
        {
            GD_Mute.Visibility = Visibility.Collapsed;
        }
        private void SetMuteState()
        {
            if (LocalMethod.GetMuteExistence())
            {
                ShowMuteIcon();
            }
            else
            {
                HideMuteIcon();
            }
        }
        #endregion
    }
}