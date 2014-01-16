using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;

namespace WpfCalendar.ViewModel
{
    public class NowTime : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime datetime = new DateTime();
        private string nowtime;
        public string TimeNow
        {
            get { return nowtime; }
            set
            {
                nowtime = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TimeNow"));
                }
            }
        }
        public NowTime()
        {
            datetime = DateTime.Now;
            TimeNow = datetime.GetDateTimeFormats('f')[0].ToString();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            datetime = DateTime.Now;
            TimeNow = datetime.GetDateTimeFormats('f')[0].ToString();
            //TimeNow = datetime.GetDateTimeFormats('f')[0].ToString();
        }

    }
}
