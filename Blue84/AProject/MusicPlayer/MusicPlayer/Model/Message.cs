using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace MusicPlayer.Model
{
    public class MusicPlayMessage
    {
        public MusicInfo SelectedMusic { get; set; }
        public ObservableCollection<MusicInfo> MusicList { get; set; }
    }
    public class MusicLoadedMessage
    {
        public ObservableCollection<MusicInfo> AllMusic { get; set; }
    }
}
