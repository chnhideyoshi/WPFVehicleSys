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

namespace SystemHelp
{
	/// <summary>
	/// Interaction logic for MainList.xaml
	/// </summary>
	public partial class MainList : UserControl
	{
		public MainList()
		{
			this.InitializeComponent();
            InitEventHandler();
		}
        private void InitEventHandler()
        {
            this.BTN_Book.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Book); } };
            this.BTN_Explorer.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Explorer); } };
            this.BTN_FM.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.FM); } };
            this.BTN_GPS.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.GPS); } };
            this.BTN_Music.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Music); } };
            this.BTN_Picture.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Picture); } };
            this.BTN_Video.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Video); } };
            this.BTN_Product.Click += (sender, e) => { if (ButtonClick != null) { ButtonClick(Modules.Product); } };
        }
        public delegate void ButtonClickEventHandler(Modules module);
        public event ButtonClickEventHandler ButtonClick;
	}
}