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

namespace MainPanel
{
	/// <summary>
	/// Interaction logic for PopOk.xaml
	/// </summary>
    public partial class PopOk : Window
	{
        public PopOk()
        {
            InitializeComponent();
            BTN_Ok.Click += new RoutedEventHandler(BTN_Ok_Click);
            OverLayBrush = Color.FromArgb(0xB3, 0x00, 0x00, 0x00);
        }
        void BTN_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (OkButtonClicked != null)
                OkButtonClicked();
            DialogResult = false;
        }

        public new string Content
        {
            get { return TBK_Content.Text; }
            set { TBK_Content.Text = value; }
        }
        public delegate void OkButtonClickedEventHandler();
        public event OkButtonClickedEventHandler OkButtonClicked;

        public bool? ShowDialog(string message)
        {
            Content = message;
            return ShowDialog();
        }
        public Color OverLayBrush
        {
            get;
            set;
        }
	}
}