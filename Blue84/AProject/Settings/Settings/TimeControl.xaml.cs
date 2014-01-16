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
using System.Runtime.InteropServices;

namespace Settings
{
	/// <summary>
	/// Interaction logic for TimeControl.xaml
	/// </summary>
	public partial class TimeControl : UserControl
	{
		public TimeControl()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        private void Init()
        {
            Year = DateTime.Now.Year.ToString();
            Month = DateTime.Now.Month.ToString();
            Day = DateTime.Now.Day.ToString();
            Hour = DateTime.Now.Hour.ToString();
            Minute = DateTime.Now.Minute.ToString();
        }
        private void InitEventHandler()
        {
            #region button
            this.BTN_Yup.Click += (sender, e) =>
            {
                Year = AddOne(Year);
               if (!CheckDateLegal())
               {
                   Year = RemoveOne(Year);
               }
            };

            this.BTN_Ydown.Click += (sender, e) =>
            {
                Year = RemoveOne(Year);
                if (!CheckDateLegal())
                {
                    Year = AddOne(Year);
                }
            };

            this.BTN_MonthUp.Click += (sender, e) => 
            {
                Month = AddOne(Month);
                if (!CheckDateLegal())
                {
                    Month = RemoveOne(Month);
                }
            };
            this.BTN_Mdown.Click += (sender, e) =>
            {
                Month = RemoveOne(Month);
                if (!CheckDateLegal())
                {
                    Month = AddOne(Month);
                }
            };

            this.BTN_DayUp.Click += (sender, e) =>
            {
                Day = AddOne(Day);
                if (!CheckDateLegal())
                {
                    Day = RemoveOne(Day);
                }
            };
            this.BTN_DayDown.Click += (sender, e) =>
            {
                Day = RemoveOne(Day);
                if (!CheckDateLegal())
                {
                    Day = AddOne(Day);
                }
            };

            this.BTN_HourUp.Click += (sender, e) =>
            {
                Hour = AddOne(Hour);
                if (!CheckDateLegal())
                {
                    Hour = RemoveOne(Hour);
                }
            };
            this.BTN_HourDown.Click += (sender, e) =>
            {
                Hour = RemoveOne(Hour);
                if (!CheckDateLegal())
                {
                    Hour = AddOne(Hour);
                }
            };

            this.BTN_MinuteUp.Click += (sender, e) =>
            {
                Minute = AddOne(Minute);
                if (!CheckDateLegal())
                {
                    Minute = RemoveOne(Minute);
                }
            };
            this.BTN_MinuteDown.Click += (sender, e) => 
            {
                Minute = RemoveOne(Minute);
                if (!CheckDateLegal())
                {
                    Minute = AddOne(Minute);
                }
            };
            #endregion
        }
        #region w32
        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMiliseconds;
        }
        //[DllImport("Kernel32.dll")]
        //public static extern bool SetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);
        //[DllImport("Kernel32.dll")]
        //public static extern void GetSystemTime(ref SystemTime sysTime);
        //[DllImport("Kernel32.dll")]
        //public static extern void GetLocalTime(ref SystemTime sysTime);
        public static bool SetLocalTime(TimeControl uc)
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();
            sysTime.wYear = Convert.ToUInt16(uc.Year);
            sysTime.wMonth = Convert.ToUInt16(uc.Month);
            sysTime.wDay = Convert.ToUInt16(uc.Day);
            sysTime.wHour = Convert.ToUInt16(uc.Hour);
            sysTime.wMinute = Convert.ToUInt16(uc.Minute);
            sysTime.wSecond = Convert.ToUInt16(0);
            //注意：
            //结构体的wDayOfWeek属性一般不用赋值，函数会自动计算，写了如果不对应反而会出错
            //wMiliseconds属性默认值为一，可以赋值
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            //由于不是C#本身的函数，很多异常无法捕获
            //函数执行成功则返回true，函数执行失败返回false
            //经常不返回异常，不提示错误，但是函数返回false，给查找错误带来了一定的困难
            catch
            {
                PopWindowsLib.PopOk POP = new PopWindowsLib.PopOk();
                POP.ShowDialog("修改失败");
            }
            return flag;
        }
        #endregion
        public string Year
        {
            get { return TBK_Year.Text; }
            set { TBK_Year.Text = value; }
        }
        public string Month
        {
            get { return this.TBK_Month.Text; }
            set { TBK_Month.Text = value; }
        }
        public string Day
        {
            get { return this.TBK_Day.Text; }
            set { TBK_Day.Text = value; }
        }
        public string Hour
        {
            get { return this.TBK_Hour.Text; }
            set { TBK_Hour.Text = value; }
        }
        public string Minute
        {
            get { return this.TBK_Minute.Text; }
            set { TBK_Minute.Text = value; }
        }
        private string AddOne(string Year)
        {
            return (Convert.ToInt32(Year) + 1).ToString() ;
        }
        private string RemoveOne(string p)
        {
            return (Convert.ToInt32(p) - 1).ToString();
        }
        private bool CheckDateLegal()
        {
            int wYear = Convert.ToInt32(Year);
            int wMonth = Convert.ToInt32(Month);
            int wDay = Convert.ToInt32(Day);
            int wHour = Convert.ToInt32(Hour);
            int wMinute = Convert.ToInt32(Minute);
            int wSecond = Convert.ToInt32(0);
            try
            {
                DateTime time = new DateTime(wYear,wMonth,wDay,wHour,wMinute,wSecond);
            }
            catch
            {
                return false;
            }



            return true;
        }
        private bool CanRemove(string p)
        {
            //string itemContent =(string)this.GetType().GetProperty(p).GetValue(this,null);
            try
            {
                switch (p)
                {
                    case "Year": { if (Convert.ToInt32(Year) - 1 >= 1900) { return true; } } break;
                    case "Month": { if (Convert.ToInt32(Month) - 1 >= 1) { return true; } } break;
                    case "Day": { if (Convert.ToInt32(Day) - 1 >= 1) { return true; } } break;
                    case "Hour": { if (Convert.ToInt32(Hour) - 1 >= 0) { return true; } } break;
                    case "Minute": { if (Convert.ToInt32(Minute) - 1 >= 0) { return true; } } break;
                    default: break;
                }
                return false;
            }
            catch { return false; }
        }
        private bool CanAdd(string p)
        {
            try
            {
                switch (p)
                {
                    case "Year": { if (Convert.ToInt32(Year) + 1 <= 2200) { return true; } } break;
                    case "Month": { if (Convert.ToInt32(Month) + 1 <= 12) { return true; } } break;
                    case "Day": { if (Convert.ToInt32(Day) + 1 <= 31) { return true; } } break;
                    case "Hour": { if (Convert.ToInt32(Hour) + 1 <= 24) { return true; } } break;
                    case "Minute": { if (Convert.ToInt32(Minute) + 1 <= 59) { return true; } } break;
                    default: break;
                }
                return false;
            }
            catch { return false; }
        }
        public void Confirm()
        {
            SetLocalTime(this);
        }
	}
}