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

namespace PictureBrowserPanel
{
	/// <summary>
	/// Interaction logic for ViewConsole.xaml
	/// </summary>
	public partial class ViewConsole : UserControl
	{
        public ViewConsole()
        {
            // Required to initialize variables
            InitializeComponent();
            InitEventHandler();
        }

        #region 初始化
        private void InitEventHandler()
        {
            this.BTN_Back.Click += new RoutedEventHandler(BTN_Back_Click);
            this.BTN_ReSize.Click += new RoutedEventHandler(BTN_ReSize_Click);
            this.BTN_GoForward.Click += new RoutedEventHandler(BTN_GoFoward_Click);
            this.BTN_ReturnLast.Click += new RoutedEventHandler(BTN_ReturnLast_Click);
            this.BTN_RotateC.Click += new RoutedEventHandler(BTN_RotateC_Click);
            this.BTN_RotateUC.Click += new RoutedEventHandler(BTN_RotateUC_Click);
        } 
        #endregion
        int angel = 0;

        public delegate void BackEventHandler(object sender, EventArgs e);
        public delegate void GoForwardEventHandler(object sender, EventArgs e);
        public delegate void ReturnLast(object sender, EventArgs e);
        public delegate void RotateUCEventHandler(object sender,int angle);
        public delegate void RotateCEventHandler(object sender,int angle);
        public delegate void ReSizeEventHandler(object sender, EventArgs e);

        public event BackEventHandler BackButtonClicked;
        public event ReSizeEventHandler ReSizeButtonClicked;
        public event ReturnLast ReturnLastButtonClicked;
        public event GoForwardEventHandler GoForwardButtonClicked;
        public event RotateUCEventHandler RotateUC;
        public event RotateCEventHandler RotateC;

        private void BTN_Back_Click(object sender, RoutedEventArgs e)
        {
            if (BackButtonClicked != null)
                BackButtonClicked(sender, e);
        }
        private void BTN_ReSize_Click(object sender, RoutedEventArgs e)
        {
            if (ReSizeButtonClicked != null)
                ReSizeButtonClicked(sender, e);
        }
        private void BTN_GoFoward_Click(object sender, RoutedEventArgs e)
        {
            if (GoForwardButtonClicked != null)
                GoForwardButtonClicked(sender, e);
        }
        private void BTN_ReturnLast_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnLastButtonClicked != null)
                ReturnLastButtonClicked(sender, e);
        }
        private void BTN_RotateUC_Click(object sender, RoutedEventArgs e)
        {
            if (RotateUC != null)
            {
                RotateUC(sender, angel);
            }
        }
        private void BTN_RotateC_Click(object sender, RoutedEventArgs e)
        {
            if (RotateC != null)
            {
                RotateC(sender, angel);
            }
        }
        
	}
}