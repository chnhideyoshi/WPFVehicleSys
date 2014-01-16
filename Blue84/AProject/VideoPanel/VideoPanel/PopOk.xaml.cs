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

namespace PopWindowsLib
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
            BTN_Import.Click += new RoutedEventHandler(BTN_Import_Click);
            BTN_Refresh.Click += new RoutedEventHandler(BTN_Refresh_Click);
            OverLayBrush = Color.FromArgb(0xB3, 0x00, 0x00, 0x00);
        }

        void BTN_Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (RefreshButtonClicked != null)
                RefreshButtonClicked();
        }

        void BTN_Import_Click(object sender, RoutedEventArgs e)
        {
            if (ImportButtonClicked != null)
                ImportButtonClicked();
        }
        void BTN_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (OkButtonClicked != null)
                OkButtonClicked();
            DialogResult = false;
        }
        public delegate void OkButtonClickedEventHandler();
        public event OkButtonClickedEventHandler OkButtonClicked;

        public delegate void RefreshButtonClickedEventHandler();
        public event RefreshButtonClickedEventHandler RefreshButtonClicked;

        public delegate void ImportButtonClickedEventHandler();
        public event ImportButtonClickedEventHandler ImportButtonClicked;
        public string Text
        {
            set { TBK_Content.Text = value; }
            get { return TBK_Content.Text; }
        }
        public Color OverLayBrush
        {
            get;
            set;
        }
        public bool IsShowGrid
        {
            get { return CC_Main.Visibility == Visibility.Visible; }
            set 
            {
                if (value)
                {
                    CC_Main.Visibility = Visibility.Visible;
                    CC_Main2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CC_Main.Visibility = Visibility.Collapsed;
                    CC_Main2.Visibility = Visibility.Visible;
                }
            }
        }
	}
}