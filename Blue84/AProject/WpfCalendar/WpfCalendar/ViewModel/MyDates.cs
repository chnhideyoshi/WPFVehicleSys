using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using WpfCalendar.Model;
using System.Data;

namespace WpfCalendar.ViewModel
{
    public class MyDates : INotifyPropertyChanged
    {
        private List<Day> days = new List<Day>();
        public MyDate mydate1;
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime dt = new DateTime();
        private DateTime dttemp = new DateTime();
        private int year, month, day, weeknum;
        public List<Day> Days
        {
            get { return days; }
            set
            {
                days = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Days"));
                }
            }
        }

        public MyDates(DateTime datetime)
        {
            dt = datetime;
            days = new List<Day>();
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            dttemp = new DateTime(year, month, 1);
            weeknum = dttemp.DayOfWeek - DayOfWeek.Sunday;
            dt = dttemp.AddDays(0 - weeknum);
            try
            {
                Days.Clear();
                for (int i = 1; i <= 42; i++)
                {
                    mydate1 = new MyDate();
                    ChinaDate.GetChinaDate(dt, ref mydate1);
                    Day oneday = new Day();
                    oneday.year = dt.Year;
                    oneday.month = dt.Month;
                    oneday.day = dt.Day;
                    oneday.mydate = mydate1;
                    if (mydate1.Day.Equals("初一"))
                    {
                        oneday.lunarday = mydate1.Month;
                    }
                    else
                    {
                        oneday.lunarday = mydate1.Day;
                    }
                    if (!oneday.mydate.ChinaHoliday.Equals(string.Empty) && !oneday.mydate.ChinaHoliday.Equals("请返回重新查询"))
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + "\n" + oneday.mydate.ChinaHoliday;
                    }
                    else if (!oneday.mydate.SolorTerm.Equals(string.Empty) && !oneday.mydate.SolorTerm.Equals("到"))
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + "\n" + oneday.mydate.SolorTerm;
                    }
                    else
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + (oneday.mydate.Day.Equals("") ? string.Empty : "\n" + oneday.lunarday);
                    }
                    oneday.dayofweek = dt.DayOfWeek.ToString();
                    Days.Add(oneday);
                    dt = dt.AddDays(1);
                }
            }
            catch (Exception) { }
        }

        public MyDates()
        {
            dt = DateTime.Now;
            days = new List<Day>();
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            dttemp = new DateTime(year, month, 1);
            weeknum = dttemp.DayOfWeek - DayOfWeek.Sunday;
            dt = dttemp.AddDays(0 - weeknum);
            try
            {
                for (int i = 1; i <= 42; i++)
                {
                    mydate1 = new MyDate();
                    ChinaDate.GetChinaDate(dt, ref mydate1);
                    Day oneday = new Day();
                    oneday.year = dt.Year;
                    oneday.month = dt.Month;
                    oneday.day = dt.Day;
                    oneday.mydate = mydate1;
                    if (mydate1.Day.Equals("初一"))
                    {
                        oneday.lunarday = mydate1.Month;
                    }
                    else
                    {
                        oneday.lunarday = mydate1.Day;
                    }
                    if (!oneday.mydate.ChinaHoliday.Equals(string.Empty) && !oneday.mydate.ChinaHoliday.Equals("请返回重新查询"))
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + "\n" + oneday.mydate.ChinaHoliday;
                    }
                    else if (!oneday.mydate.SolorTerm.Equals(string.Empty) && !oneday.mydate.SolorTerm.Equals("到"))
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + "\n" + oneday.mydate.SolorTerm;
                    }
                    else
                    {
                        oneday.showday = (oneday.day > 9 ? " " + oneday.day.ToString() : "  " + oneday.day.ToString()) + (oneday.mydate.Day.Equals("") ? string.Empty : "\n" + oneday.lunarday);
                    }
                    oneday.dayofweek = dt.DayOfWeek.ToString();
                    Days.Add(oneday);
                    dt = dt.AddDays(1);
                }
            }
            catch (Exception) { }
        }
    }

    public class Day
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public string lunarday { get; set; }
        public string dayofweek { get; set; }
        public string showday{ get; set; }
        public MyDate mydate { get; set; }
    }
}
