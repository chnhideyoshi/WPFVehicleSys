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
	/// Interaction logic for MainShow.xaml
	/// </summary>
	public partial class MainShow : UserControl
	{
		public MainShow()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        private void Init()
        {
            try
            {
                touchClass.InitTouchListOperation(LayoutRoot, STK_Containner);
            }
            catch { return; }
        }
        ManipTest.TouchMouseWithRebound touchClass = new ManipTest.TouchMouseWithRebound();
        private void InitEventHandler()
        {
            this.CA_ExpSL.PreviewMouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double rate = e.GetPosition(CA_ExpSL).Y / CA_ExpSL.ActualHeight;
                    touchClass.ResetBack(false);
                    SL_Down.Value = rate * SL_Down.Maximum;
                    Canvas.SetTop(STK_Containner, -(touchClass.MaxTop) * rate);
                }
            };
            touchClass.EndOperation += () =>
            {
                //endSign = true;
                if (touchClass.MaxTop > 0)
                {
                    SL_Down.Value = SL_Down.Maximum * (-Canvas.GetTop(STK_Containner) / touchClass.MaxTop);
                }
                //endSign = false;
            };
        }
        public string Title
        {
            set { TBK_Title.Text = value; }
            get { return TBK_Title.Text; }
        }
        public FlowDocument HelpContent
        {
            set { TB_Main.Document=value; }
            get { return TB_Main.Document; }
        }
        public string HelpContentText
        {
            set { R_Text.Text = value; }
            get { return R_Text.Text; }
        }
	}
}