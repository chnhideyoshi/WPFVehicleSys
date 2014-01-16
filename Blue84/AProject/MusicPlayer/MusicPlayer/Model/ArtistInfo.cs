using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace MusicPlayer.Model
{
    public class ArtistInfo : ObservableCollection<MusicInfo>
    {
        public int MusicCount { get; set; }
        public string Name { get; set; }
    }
}
