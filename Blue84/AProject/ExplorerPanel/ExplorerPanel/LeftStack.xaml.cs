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
	/// Interaction logic for LeftStack.xaml
	/// </summary>
	public partial class LeftStack : UserControl
	{
		public LeftStack()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            STK_Main.Children.Clear();
        }
        private void InitEventHandler()
        {
            BTN_Down.Click += new RoutedEventHandler(BTN_Down_Click);
            BTN_Up.Click += new RoutedEventHandler(BTN_Up_Click);
            BTN_Root.Click += (sender, e) =>
            {
                if (RootButtonClicked != null)
                    RootButtonClicked();
            };
        } 
        #endregion
        #region 界面逻辑
        private double UnitHeight
        {
            get { return (STK_MainOut.Children[0] as Button).Height; }
        }
        private void BTN_Up_Click(object sender, RoutedEventArgs e)
        {
            if (STK_Main.Children.Count > 2)
            {
                double top = Canvas.GetTop(STK_MainOut);
                if (IsInRange(top + UnitHeight))
                {
                    Canvas.SetTop(STK_MainOut, top + UnitHeight);
                }
            }
            else
            {
                Canvas.SetTop(STK_MainOut, 0);
            }
        }
        private void BTN_Down_Click(object sender, RoutedEventArgs e)
        {
            if (STK_Main.Children.Count > 2)
            {
                double top = Canvas.GetTop(STK_MainOut);
                if (IsInRange(top - UnitHeight))
                {
                    Canvas.SetTop(STK_MainOut, top - UnitHeight);
                }
            }
            else
            {
                Canvas.SetTop(STK_MainOut,0);
            }
        }
        private bool IsInRange(double position)
        {
            return position <= 0 && position >= -(STK_MainOut.ActualHeight - LayoutRoot.ActualHeight);
        }
        private void ReLayout()
        {
            if (STK_Main.Children.Count > 2)
            {
                Canvas.SetTop(STK_MainOut, -UnitHeight * (this.STK_Main.Children.Count - 2));
            }
            else
            {
                Canvas.SetTop(STK_MainOut, 0);
            }
        }
        #endregion
        #region 功能逻辑
        public void Push(string path)
        {
            Button b = new Button();
            b.Click += new RoutedEventHandler(DirButton_Click);
            b.Style = this.FindResource("ButtonStyleDir") as Style;
            b.Content = ViewModel.GetDirectoryName(path);
            b.Tag = path;
            STK_Main.Children.Add(b);
            ReLayout();
        }
        void DirButton_Click(object sender, RoutedEventArgs e)
        {
            if (DirButtonClicked != null)
            {
                Pop(sender);
                DirButtonClicked(sender, (sender as Button).Tag as string);
            }
        }
        private void Pop(object sender)
        {
            Button b = sender as Button;
            int index = STK_Main.Children.IndexOf(b);
            if (index != -1)
            {
                STK_Main.Children.RemoveRange(index + 1,STK_Main.Children.Count-index-1);
            }
            ReLayout();
        }
        public delegate void DirButtonClickEventHandler(object sender, string path);
        public event DirButtonClickEventHandler DirButtonClicked;
        public delegate void RootButtonClickEventHandler();
        public event RootButtonClickEventHandler RootButtonClicked;
        public void Clear()
        {
            STK_Main.Children.Clear();
        }
        #endregion
    }
}