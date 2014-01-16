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

namespace ExplorerPanel
{
	/// <summary>
	/// Interaction logic for ItemWindow.xaml
	/// </summary>
	public partial class ItemWindow : Window
	{
		public ItemWindow()
		{
			this.InitializeComponent();
			// Insert code required on object creation below this point.
            BTN_ImageIm.Click += new RoutedEventHandler(BTN_ImageIm_Click);
            BTN_MusicIm.Click += new RoutedEventHandler(BTN_MusicIm_Click);
            BTN_VideoIm.Click += new RoutedEventHandler(BTN_VideoIm_Click);
            BTN_BookIm.Click += new RoutedEventHandler(BTN_BookIm_Click);
            BTN_Ok.Click += new RoutedEventHandler(BTN_Ok_Click);
            //Topmost = true;
        }
        private bool isIncludeMode = false;

        public bool IsIncludeMode
        {
            get { return isIncludeMode; }
            set 
            { 
                isIncludeMode = value;
                if (value)
                {
                    this.BTN_BookIm.Content = "将选中项包含到图书列表";
                    this.BTN_ImageIm.Content = "将选中项包含到图片列表";
                    this.BTN_MusicIm.Content = "将选中项包含到音乐列表";
                    this.BTN_VideoIm.Content = "将选中项包含到视频列表";
                }
            }
        }
        void BTN_BookIm_Click(object sender, RoutedEventArgs e)
        {
            if (ImportBook != null)
                ImportBook();
            this.DialogResult = true;
        }

        void BTN_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void BTN_VideoIm_Click(object sender, RoutedEventArgs e)
        {
            if (ImportVideo != null)
                ImportVideo();
            this.DialogResult = true;
        }

        void BTN_MusicIm_Click(object sender, RoutedEventArgs e)
        {
            if (ImportMusic != null)
                ImportMusic();
            this.DialogResult = true;
        }

        void BTN_ImageIm_Click(object sender, RoutedEventArgs e)
        {
            if (ImportImage != null)
                ImportImage();
            this.DialogResult = true;
        }
        public delegate void ImportImageEventHandler();
        public event ImportImageEventHandler ImportImage;
        public delegate void ImportVideoEventHandler();
        public event ImportVideoEventHandler ImportVideo;
        public delegate void ImportMusicEventHandler();
        public event ImportMusicEventHandler ImportMusic;
        public delegate void ImportBookEventHandler();
        public event ImportBookEventHandler ImportBook;
    }
}