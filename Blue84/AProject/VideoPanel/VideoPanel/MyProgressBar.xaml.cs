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

namespace VideoPanel
{
	/// <summary>
	/// Interaction logic for MyProgressBar.xaml
	/// </summary>
	public partial class MyProgressBar : UserControl
	{
        public MyProgressBar()
        {
            this.InitializeComponent();
            InitEventHandler();
        }
        private void InitEventHandler()
        {
            LayoutRoot.PreviewMouseUp += new MouseButtonEventHandler(LayoutRoot_PreviewMouseUp);
            LayoutRoot.PreviewMouseDown += new MouseButtonEventHandler(LayoutRoot_PreviewMouseDown);
            SL_Progress.ValueChanged += new RoutedPropertyChangedEventHandler<double>(SL_Progress_ValueChanged);
        }
        void LayoutRoot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ProgressDragBegin != null && IsEnabled)
            {
                ProgressDragBegin(sender, SL_Progress.Value);
            }
        }
        void LayoutRoot_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ProgressDragEnd != null && IsEnabled)
            {
                ProgressDragEnd(sender, SL_Progress.Value);
            }
        }
        void SL_Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsEnabled)
            {
                SL_Progress.Value = 0;
            }
            if (ProgressValueChanged != null&&IsEnabled&&!IsCodeSet)
                ProgressValueChanged(sender, e);
        }
        public double Maximum
        {
            set
            {
                this.SL_Progress.Maximum = value;
            }
            get { return SL_Progress.Maximum; }
        }
        public double ProgressValue
        {
            set
            {
                IsCodeSet = true;
                this.SL_Progress.Value = value;
                IsCodeSet = false;
            }
            get { return SL_Progress.Value; }
        }
        bool IsCodeSet = false;
        public delegate void ProgressDragBeginEventHandler(object sender, double value);
        public event ProgressDragBeginEventHandler ProgressDragBegin;
        public delegate void ProgressDragEndEventHandler(object sender, double value);
        public event ProgressDragEndEventHandler ProgressDragEnd;
        public delegate void ProgressValueChangedEventHandler(object sender, RoutedPropertyChangedEventArgs<double> e);
        public event ProgressValueChangedEventHandler ProgressValueChanged;
        public Orientation Orientation
        {
            set { SL_Progress.Orientation = value; }
            get { return SL_Progress.Orientation; }
        }
	}
	
}