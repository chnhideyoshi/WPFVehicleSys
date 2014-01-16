using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Threading;
namespace MusicPlayer.Model
{
    public class Player :MediaPlayer ,INotifyPropertyChanged
    {
        #region Field
        private readonly DispatcherTimer _positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);

        #endregion // Field

        #region INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        public Player()
        {
            _positionTimer.Interval = TimeSpan.FromMilliseconds(500);
            _positionTimer.Tick += new EventHandler(positionTimer_Tick);
            _positionTimer.IsEnabled = true;
        }

        void positionTimer_Tick(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Position");
        }
    }
}
