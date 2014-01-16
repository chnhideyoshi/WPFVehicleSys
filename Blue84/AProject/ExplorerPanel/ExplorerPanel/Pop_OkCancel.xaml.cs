using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ExplorerPanel
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Pop_OkCancel : Window
    {
       
        public Pop_OkCancel()
        {
            InitializeComponent();
            BTN_Cancel.Click += new RoutedEventHandler(BTN_Cancel_Click);
            BTN_Ok.Click += new RoutedEventHandler(BTN_Ok_Click);
            OverLayBrush = Color.FromArgb(0xB3, 0x00, 0x00, 0x00);
            //Topmost = true;
        }
        void BTN_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (OkButtonClicked != null)
                OkButtonClicked();
            DialogResult = true;
        }

        void BTN_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked();
            DialogResult = false;
        }
        public new string Content
        {
            get { return TBK_Content.Text; }
            set { TBK_Content.Text = value; }
        }
        public delegate void OkButtonClickedEventHandler();
        public event OkButtonClickedEventHandler OkButtonClicked;

        public delegate void CancelButtonClickedEventHandler();
        public event CancelButtonClickedEventHandler CancelButtonClicked;

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
