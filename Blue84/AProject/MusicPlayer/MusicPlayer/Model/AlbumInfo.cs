using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
namespace MusicPlayer.Model
{
    public class AlbumInfo
    {
        public BitmapImage CoverImage { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        private ObservableCollection<MusicInfo> _allMusicInfo;

        public ObservableCollection<MusicInfo> AllMusic
        {
            get
            {
                if (_allMusicInfo == null)
                    _allMusicInfo = new ObservableCollection<MusicInfo>();
                return _allMusicInfo;
            }
            set { _allMusicInfo = value; }
        }
    }
}
