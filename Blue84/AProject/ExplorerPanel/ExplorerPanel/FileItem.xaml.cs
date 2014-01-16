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

namespace ExplorerPanel
{
	/// <summary>
	/// Interaction logic for FileItem.xaml
	/// </summary>
	public partial class FileItem : UserControl
	{
		public FileItem()
		{
			this.InitializeComponent();
            Init();
            InitEvnetHandler();
		}

        private void Init()
        {
            IsFile = true;
            Info = null;
        }

        private void InitEvnetHandler()
        {
            this.REC_cLICK.MouseDown += new MouseButtonEventHandler(REC_cLICK_MouseDown);
            this.REC_cLICK.MouseUp += new MouseButtonEventHandler(REC_cLICK_MouseUp);
        }
        #region 属性
        public string Info { get; set; }
        public bool IsFile { get; set; }
        public string FileName
        {
            get { return TBK_FileName.Text; }
            set { TBK_FileName.Text = value; }
        }
        private FileClass kind;
        public FileClass HeadImage
        {
            get { return kind; }
            set
            {
                kind = value;
                SetHeader(value);
            }
        }
        public bool? IsChecked
        {
            get { return CKB_Check.IsChecked; }
            set { CKB_Check.IsChecked = value; }
        }
        #endregion
        #region 
        bool isTouched = false;
        public void ClearFlag()
        {
            isTouched = false;
            REC_hover.Visibility = Visibility.Hidden;
        }
        private void REC_cLICK_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isTouched && Clicked != null)
            {
                Clicked(this, Info, IsFile);
            }
            ClearFlag();
        }
        private void REC_cLICK_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isTouched = true;
            REC_hover.Visibility = Visibility.Visible;
            if (ItemReady != null)
            {
                ItemReady(this);
            }
        }
        private void SetHeader(FileClass value)
        {
            for (int i = 0; i < IMG_HeadImage.Children.Count; i++)
            {
                IMG_HeadImage.Children[i].Visibility = Visibility.Hidden;
            }
            switch (value)
            {
                case FileClass.Folder: { PH_Dir.Visibility = Visibility.Visible; return; }
                case FileClass.Music: { PH_Music.Visibility = Visibility.Visible; return; }
                case FileClass.Picture: { PH_Img.Visibility = Visibility.Visible; return; }
                case FileClass.Video: { PH_Video.Visibility = Visibility.Visible; return; }
                case FileClass.File: { PH_File.Visibility = Visibility.Visible; return; }
                case FileClass.Driver: { this.PH_Disk.Visibility = Visibility.Visible; return; }
                default: return;
            }
        }
        #endregion
        public delegate void ClickEventHandler(object sender, string info, bool isFile);
        public event ClickEventHandler Clicked;
        public delegate void ItemReadyEventHandler(FileItem sender);
        public event ItemReadyEventHandler ItemReady;
	}
   
}