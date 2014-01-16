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

namespace MusicPlayer.View
{
	/// <summary>
	/// Interaction logic for UpBan.xaml
	/// </summary>
	public partial class UpBan : UserControl
	{
		public UpBan()
		{
			this.InitializeComponent();
            InitTime();
        }
        #region time
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
        private void InitTime()
        {
            TBK_Time.Text = DateTime.Now.GetDateTimeFormats('f')[0].ToString();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += (sender, e) =>
            {
                TBK_Time.Text = DateTime.Now.GetDateTimeFormats('f')[0].ToString();
            };
            timer.Start();
        }
        #endregion
	}
}