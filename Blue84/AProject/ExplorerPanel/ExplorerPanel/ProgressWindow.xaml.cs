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
	/// Interaction logic for ProgressWindow.xaml
	/// </summary>
	public partial class ProgressWindow : Window
	{
		public ProgressWindow()
		{
			this.InitializeComponent();
		}
        public ProgressWindow(bool isCopy)
        {
            this.InitializeComponent();
            IsCopy = IsCopy;
            BTN_CancelCD.Click += new RoutedEventHandler(BTN_CancelCD_Click);
        }
        private bool isCopy = true;
        public bool IsCopy
        {
            get { return isCopy; }
            set
            {
                if (value)
                {
                    App.ViewModel.CopyProgressChanged += new ViewModel.CopyProgressChangedEventHandler(ViewModel_CopyProgressChanged);
                    App.ViewModel.CopyCompleted += new ViewModel.CopyCompletedEventHandler(ViewModel_CopyCompleted);
                    TBK_Content.Text = "正在复制....";
                }
                else
                {
                    App.ViewModel.DeleteCompleted += new ViewModel.DeleteCompletedEventHandler(ViewModel_DeleteCompleted);
                    App.ViewModel.DeleteProgressChanged += new ViewModel.DeleteProgressChangedEventHandler(ViewModel_DeleteProgressChanged);
                    TBK_Content.Text = "正在删除....";
                }
                isCopy = value; 
            }
        }
        public void BeginWork(FileClass fclass)
        {
            App.ViewModel.BeginCopy(fclass);
        }
        public void BeginWork()
        {
            App.ViewModel.BeginDelete();
        }
        #region 复制进度显示
        void ViewModel_CopyCompleted()
        {
            PopOk pop = new PopOk();
            pop.ShowDialog("复制结束 ! ");
            App.ViewModel.CopyCompleted -= ViewModel_CopyCompleted;
            App.ViewModel.CopyProgressChanged -= ViewModel_CopyProgressChanged;
            this.Close();
        }

        void ViewModel_CopyProgressChanged(int percentage)
        {
            SL_Progress.Value = SL_Progress.Maximum * (percentage / 100.0);
        }
        #endregion
        #region 删除进度显示
        void ViewModel_DeleteProgressChanged(int percentage)
        {
            SL_Progress.Value = SL_Progress.Maximum * (percentage / 100.0);
        }

        void ViewModel_DeleteCompleted()
        {
            PopOk pop = new PopOk();
            pop.ShowDialog("删除结束 ! ");
            App.ViewModel.DeleteCompleted -= ViewModel_DeleteCompleted;
            App.ViewModel.DeleteProgressChanged -= ViewModel_DeleteProgressChanged;
            this.Close();

        }
        void BTN_CancelCD_Click(object sender, RoutedEventArgs e)
        {
            if (IsCopy)
            {
                App.ViewModel.CancelCopy();
            }
            else
            {
                App.ViewModel.CancelDelete();
            }
        }
        #endregion
    }
}