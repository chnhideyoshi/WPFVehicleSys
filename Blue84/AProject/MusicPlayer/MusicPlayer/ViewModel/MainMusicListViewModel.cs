using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Model;

namespace MusicPlayer.ViewModel
{
    public class MainMusicListViewModel : ViewModelBase
    {
        #region Field

        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private AlbumListViewModel _albumListViewModel;

        private ObservableCollection<MusicInfo> _allMusicCollection;
        private AllMusicListViewModel _allMusicListViewModel;
        private ArtistListViewModel _artistListViewModel;
        private DirListViewModel _dirListViewModel;
        private Boolean _isBusy;

        #endregion // Field

        #region Property

        public Boolean IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public ObservableCollection<MusicInfo> AllMusicCollection
        {
            get
            {
                if (_allMusicCollection == null)
                {
                    _allMusicCollection = new ObservableCollection<MusicInfo>();
                    return _allMusicCollection;
                }
                return _allMusicCollection;
            }
        }

        public DirListViewModel DirList
        {
            get
            {
                if (_dirListViewModel == null)
                {
                    _dirListViewModel = new DirListViewModel();
                    return _dirListViewModel;
                }
                return _dirListViewModel;
            }
        }

        public ArtistListViewModel ArtistList
        {
            get
            {
                if (_artistListViewModel == null)
                    _artistListViewModel = new ArtistListViewModel();
                return _artistListViewModel;
            }
        }

        public AllMusicListViewModel AllMusicList
        {
            get
            {
                if (_allMusicListViewModel == null)
                    _allMusicListViewModel = new AllMusicListViewModel();
                return _allMusicListViewModel;
            }
        }

        public AlbumListViewModel AlbumList
        {
            get
            {
                if (_albumListViewModel == null)
                    _albumListViewModel = new AlbumListViewModel();
                return _albumListViewModel;
            }
        }

        #endregion // Property

        #region Init

        public MainMusicListViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                Init();
                GetUserMusicAsync();
            }
        }

        private void Init()
        {
            _artistListViewModel = new ArtistListViewModel();
            _allMusicListViewModel = new AllMusicListViewModel();
            _albumListViewModel = new AlbumListViewModel();
            _dirListViewModel = new DirListViewModel();
           
        }

        #endregion // Init

        #region Methods

        private void GetUserMusicAsync()
        {
            IsBusy = true;
            _backgroundWorker.DoWork += (sender, e) =>
                                           {
                                               //App.Current.Dispatcher.BeginInvoke((Action)(() =>
                                               //{
                                               string path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                                               var dir = new DirectoryInfo(path);
                                               FileInfo[] files = dir.GetFiles("*.mp3", SearchOption.AllDirectories);
                                               foreach (var musicInfo in files.Select(file => new MusicInfo(file.FullName)))
                                               {
                                                   AllMusicCollection.Add(musicInfo);
                                               }
                                               files = dir.GetFiles("*.wma", SearchOption.AllDirectories);
                                               foreach (var musicInfo in files.Select(file => new MusicInfo(file.FullName)))
                                               {
                                                   AllMusicCollection.Add(musicInfo);
                                               }
                                               //}));
                                           };
            _backgroundWorker.RunWorkerCompleted += (sender, e) =>
                                                       {
                                                           IsBusy = false;

                                                           var msg = new MusicLoadedMessage
                                                                         {AllMusic = AllMusicCollection};
                                                           Messenger.Default.Send(msg);
                                                       };
            _backgroundWorker.RunWorkerAsync();
        }

        #endregion // Methods

        #region Command

        private RelayCommand _toPlaying;

        public ICommand ToPlayingCommand
        {
            get
            {
                if (_toPlaying == null)
                {
                    _toPlaying = new RelayCommand(() => Messenger.Default.Send<bool, MainViewModel>(false));
                }
                return _toPlaying;
            }
        }

        #endregion
    }
}