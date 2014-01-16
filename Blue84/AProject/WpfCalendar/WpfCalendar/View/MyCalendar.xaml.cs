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
using WpfCalendar.ViewModel;
using System.ComponentModel;
using WpfCalendar.Model;
using System.Windows.Media.Animation;

namespace WpfCalendar.View
{
    /// <summary>
    /// Interaction logic for MyCalendar.xaml
    /// </summary>
    public partial class MyCalendar : UserControl
    {

        public int daybuttonsize = 40;
        public int monthbuttonsize = 70;
        public MyDates mydates;
        public Dictionary<int, Button> daybuttons = new Dictionary<int, Button>();
        public Dictionary<int, Button> mbts = new Dictionary<int, Button>();
        public int year, month, day;
        public BackgroundWorker backgroundWorker;
        Storyboard sb;
        string time;
        MyDate mydate;
        Button TempButton = new Button();
        int x, y;
        int showlevel = 0;

        public MyCalendar()
        {
            InitializeComponent();
            sb = (Storyboard)this.FindResource("CalendarCross");
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            DrawYearsAndMonthsButton();
            DrawDaysButton();
        }

        #region 后台线程更新数据
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            showdays();
            sb.Begin();
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            mydates = new MyDates();
            mydates = new MyDates(new DateTime(year, month, 1));
        }
        #endregion

        #region 画按钮
        /// <summary>
        /// 画日期按钮
        /// </summary>
        public void DrawDaysButton()
        {
            try
            {
                daybuttons.Clear();
                mydates = new MyDates();
                year = mydates.Days[20].year;
                month = mydates.Days[20].month;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        Button daybutton = new Button();
                        daybutton.Width = daybuttonsize;
                        daybutton.Height = daybuttonsize;
                        daybutton.BorderBrush = null;
                        daybutton.BorderThickness = new Thickness(0);
                        daybutton.FontSize = 14;
                        daybutton.Click += new RoutedEventHandler(DayButton_Click);
                        CalendarDays.Children.Add(daybutton);
                        Canvas.SetTop(daybutton, i * daybuttonsize);
                        Canvas.SetLeft(daybutton, j * daybuttonsize);
                        daybuttons.Add(i * 7 + j, daybutton);
                    }
                }
            }
            catch (Exception) { }
            showdays();
        }

        public void DrawYearsAndMonthsButton()
        {
            try
            {
                mbts.Clear();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Button yearandmonthbutton = new Button();
                        yearandmonthbutton.Width = monthbuttonsize;
                        yearandmonthbutton.Height = monthbuttonsize;
                        yearandmonthbutton.BorderBrush = null;
                        yearandmonthbutton.BorderThickness = new Thickness(0);
                        yearandmonthbutton.FontSize = 16;
                        yearandmonthbutton.Content = (i * 4 + j + 1).ToString() + "月";
                        yearandmonthbutton.Click += new RoutedEventHandler(YearAndMonthbutton_Click);
                        CalendarMonths.Children.Add(yearandmonthbutton);
                        Canvas.SetTop(yearandmonthbutton, i * monthbuttonsize + 20);
                        Canvas.SetLeft(yearandmonthbutton, j * monthbuttonsize);
                        mbts.Add(i * 4 + j, yearandmonthbutton);
                    }
                }
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 点击年与月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void YearAndMonthbutton_Click(object sender, RoutedEventArgs e)
        {
            TempButton = (sender as Button);
            int x = (int)((Canvas.GetTop(TempButton) - 20) / monthbuttonsize);
            int y = (int)(Canvas.GetLeft(TempButton) / monthbuttonsize);
            showlevel--;
            switch (showlevel)
            {
                case 0:
                    month = x * 4 + y + 1;
                    break;
                case 1:
                    year = year + x * 4 + y - 1;
                    break;
                case 2:
                    year = year + (x * 4 + y) * 10 - 10;
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            backgroundWorker.RunWorkerAsync();
        }
        /// <summary>
        /// 点击日期显示农历信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DayButton_Click(object sender, RoutedEventArgs e)
        {
            TempButton = (sender as Button);
            x = (int)Canvas.GetTop(TempButton) / daybuttonsize;
            y = (int)Canvas.GetLeft(TempButton) / daybuttonsize;
            ShowLunarInfo(x, y);
            //是否当前月，如不是，则刷新界面
            if (!mydates.Days[20].month.Equals(mydates.Days[x * 7 + y].month))
            {
                TempButton.IsDefault = false;
                backgroundWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region 显示农历信息
        public void ShowLunarInfo(int x, int y)
        {
            try
            {
                time = string.Empty;
                mydate = new MyDate();
                mydate = mydates.Days[x * 7 + y].mydate;
                year = mydates.Days[x * 7 + y].year;
                month = mydates.Days[x * 7 + y].month;
                day = mydates.Days[x * 7 + y].day;
                string timetemp = year.ToString() + "年" + month.ToString() + "月" + day.ToString() + "日";
                time = "\n\n" + mydate.Year + "\n" + mydate.Month + mydate.Day;
                if (mydate.SolorTerm.Equals(""))
                {
                    time = "\n" + time;
                }
                else
                {
                    time = time + "\n" + mydate.SolorTerm;
                }
                if (mydate.Holiday.Equals(""))
                {
                    time = "\n" + time;
                }
                else
                {
                    time = time + "\n" + mydate.Holiday;
                }
                if (mydate.ChinaHoliday.Equals(""))
                {
                    time = "\n" + time;
                }
                else
                {
                    time = time + "\n" + mydate.ChinaHoliday;
                }
                ShowGenerate.Text = timetemp;
                ShowLunar.Text = time;
            }
            catch (Exception) { }
        }
        #endregion

        #region 显示日期按钮
        private void showdays()
        {
            switch (showlevel)
            {
                case 0:
                    CalendarMonths.Visibility = Visibility.Collapsed;
                    CalendarDays.Visibility = Visibility.Visible;
                    try
                    {
                        for (int i = 0; i < 42; i++)
                        {
                            daybuttons[i].Content = mydates.Days[i].showday;
                            if (mydates.Days[i].year == DateTime.Now.Year && mydates.Days[i].month == DateTime.Now.Month && mydates.Days[i].day == DateTime.Now.Day)
                            {
                                daybuttons[i].Background = new SolidColorBrush(Colors.Yellow);
                                ShowLunarInfo(i / 6, i % 7);
                            }
                            else
                            {
                                daybuttons[i].Background = new SolidColorBrush(Colors.LightGray);
                            }
                        }
                        YearAndMonth.Content = mydates.Days[20].year + "年" + mydates.Days[20].month + "月";
                    }
                    catch (Exception) { }
                    break;
                case 1:
                    try
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mbts[i].Content = (i + 1).ToString() + "月";
                        }
                        YearAndMonth.Content = year.ToString() + "年";
                    }
                    catch (Exception) { }
                    break;
                case 2:
                    try
                    {
                        year = (int)year / 10 * 10;
                        for (int i = 0; i < 12; i++)
                        {
                            mbts[i].Content = (year - 1 + i).ToString() + "年";
                        }
                        YearAndMonth.Content = (year).ToString() + "-" + (year + 10).ToString() + "年";
                    }
                    catch (Exception) { }
                    break;
                case 3:
                    try
                    {
                        year = (int)year / 100 * 100;
                        for (int i = 0; i < 12; i++)
                        {
                            mbts[i].Content = (year - 10 + i * 10).ToString() + "-\n" + (year + i * 10).ToString();
                        }
                        YearAndMonth.Content = (year).ToString() + "-" + (year + 99).ToString() + "年";
                    }
                    catch (Exception) { }
                    break;
                default:
                    showlevel = 0;
                    backgroundWorker.RunWorkerAsync();
                    break;
            }
        }

        #endregion

        #region 向前向后一页操作
        private void Forword_Click(object sender, RoutedEventArgs e)
        {
            switch (showlevel)
            {
                case 0:
                    year = mydates.Days[20].year;
                    month = mydates.Days[20].month + 1;
                    if (month > 12)
                    {
                        year += 1;
                        month -= 12;
                    }
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 1:
                    year++;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 2:
                    year += 10;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 3:
                    year += 100;
                    backgroundWorker.RunWorkerAsync();
                    break;
                default:
                    break;
            }
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            switch (showlevel)
            {
                case 0:
                    year = mydates.Days[20].year;
                    month = mydates.Days[20].month - 1;
                    if (month < 1)
                    {
                        month += 12;
                        year -= 1;
                    }
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 1:
                    year--;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 2:
                    year -= 10;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 3:
                    year -= 100;
                    backgroundWorker.RunWorkerAsync();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 点击标题，显示年月信息
        private void YearAndMonth_Click(object sender, RoutedEventArgs e)
        {
            showlevel++;
            switch (showlevel)
            {
                case 0:
                    CalendarMonths.Visibility = Visibility.Collapsed;
                    CalendarDays.Visibility = Visibility.Visible;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 1:
                    //YearAndMonth.Content = year.ToString() + "年";
                    CalendarDays.Visibility = Visibility.Collapsed;
                    CalendarMonths.Visibility = Visibility.Visible;
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 2:
                    backgroundWorker.RunWorkerAsync();
                    break;
                case 3:
                    backgroundWorker.RunWorkerAsync();
                    break;
                default:
                    showlevel = 3;
                    break;
            }
        }
        #endregion
    }
}
