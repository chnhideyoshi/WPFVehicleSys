using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Model;
namespace MusicPlayer.ViewModel
{
    public class ArtistListViewModel : ViewModelBase
    {
        #region Field

        private ObservableCollection<MusicInfo> _allMusicCollection;

        private ObservableCollection<ArtistInfo> _allArtist;

        private ArtistInfo _selectedArtist;




        #endregion

        #region Property
        public ObservableCollection<MusicInfo> AllMusicCollection
        {
            get
            {
                if (_allMusicCollection != null) return _allMusicCollection;
                _allMusicCollection = new ObservableCollection<MusicInfo>();
                return _allMusicCollection;
            }
            set
            {
                _allMusicCollection = value;
                RaisePropertyChanged("AllMusicCollection");
            }
        }
        public ObservableCollection<ArtistInfo> AllArtist
        {
            get
            {
                if (_allArtist != null) return _allArtist;
                _allArtist = new ObservableCollection<ArtistInfo>();
                return _allArtist;
            }
            set
            {
                _allArtist = value;
                RaisePropertyChanged("AllArtist");
            }
        }
        public ArtistInfo SelectedArtist
        {
            get
            {
                return _selectedArtist;
            }
            set
            {
                _selectedArtist = value;
                RaisePropertyChanged("SelectedArtist");
            }
        }
        #endregion
        public ArtistListViewModel()
        {
            RegistMessage();
        }

        #region Commands
        RelayCommand _selectedChangedCommand;
        public ICommand SelectedChangeCommand
        {
            get
            {
                if (_selectedChangedCommand != null) return _selectedChangedCommand;
                _selectedChangedCommand = new RelayCommand(() =>
                                                               {
                                                                   var msg = new MusicPlayMessage { MusicList = SelectedArtist };
                                                                   Messenger.Default.Send<MusicPlayMessage, PlayingViewModel>(msg);
                                                                   Messenger.Default.Send<bool, MainViewModel>(false);
                                                               }
                    );
                return _selectedChangedCommand;
            }
        }
        #endregion
        #region Methods
        void RegistMessage()
        {
            Messenger.Default.Register<MusicLoadedMessage>(this, true, p =>
                {
                    AllMusicCollection = p.AllMusic;
                    GetArtistInfo();
                    SortArtist();
                }
            );

        }
        void GetArtistInfo()
        {
            var allArtistCollection = new ObservableCollection<ArtistInfo>();
            //找到所有专辑的名字
            var info = (from music in AllMusicCollection
                        select music.Artist).Distinct();

            foreach (var artistInfo in info)
            {
                //找到该专辑的信息
                var artist = (from music in AllMusicCollection
                              where music.Artist == artistInfo
                              select new ArtistInfo { Name = music.Artist }).ToList().First();
                //找到该专辑内所有曲目
                string info1 = artistInfo;
                var albumMusic = from music in AllMusicCollection
                                 where music.Artist == info1
                                 select music;
                foreach (var music in albumMusic)
                {
                    artist.Add(music);
                    artist.MusicCount++;
                }
                //添加专辑到专辑列表

                allArtistCollection.Add(artist);
            }

            AllArtist = allArtistCollection;
        }

        void SortArtist()
        {
            var tempCollection = new ObservableCollection<ArtistInfo>(AllArtist);
            var artists = from artist in tempCollection
                         orderby artist.Name
                         select artist;
            AllArtist.Clear();
            foreach (var artist in artists)
            {
                AllArtist.Add(artist);
            }
        }
        #endregion
    }
}