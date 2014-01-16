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

namespace WpfCalendar
{
	/// <summary>
	/// Interaction logic for LunarCalendarTest.xaml
	/// </summary>
	public partial class LunarCalendarTest : Calendar
	{


        public string LunarDay
        {
            get { return (string)GetValue(LunarDayProperty); }
            set { SetValue(LunarDayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LunarDay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LunarDayProperty =
            DependencyProperty.Register("LunarDay", typeof(string), typeof(LunarCalendarTest), new UIPropertyMetadata(0));

        
		public LunarCalendarTest()
		{
			this.InitializeComponent();
		}
	}
}