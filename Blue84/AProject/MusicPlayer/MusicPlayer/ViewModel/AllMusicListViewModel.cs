using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Model;
namespace MusicPlayer.ViewModel
{

    public class AllMusicListViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<MusicInfo> _allMusicCollection;
        private MusicInfo _selectedMusic;
        private bool _isBusy = true;
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

        public MusicInfo SelectedMusic
        {
            get
            {
                return _selectedMusic;
            }
            set
            {
                _selectedMusic = value;
                RaisePropertyChanged("SelectedMusic");
            }
        }
        #endregion // Property
        public AllMusicListViewModel()
        {
            RegistMessage();
        }
        #region Command
        RelayCommand _selectedChange;

        public ICommand SelectedChangeCommand
        {
            get
            {
                if (_selectedChange != null) return _selectedChange;
                _selectedChange = new RelayCommand(() =>
                                                       {
                                                           var msg = new MusicPlayMessage { MusicList = AllMusicCollection, SelectedMusic = SelectedMusic };
                                                           Messenger.Default.Send<MusicPlayMessage, PlayingViewModel>(msg);
                                                           Messenger.Default.Send<bool, MainViewModel>(false);
                                                       }
                    );
                return _selectedChange;
            }
        }
        #endregion // Command

        #region Method

        void RegistMessage()
        {
            Messenger.Default.Register<MusicLoadedMessage>(this, true, p =>
                {
                    AllMusicCollection = p.AllMusic;
                    SortMusic();
                    IsBusy = false;
                });
        }

        void SortMusic()
        {
            var tempCollection = new ObservableCollection<MusicInfo>(AllMusicCollection);
            var musics = from music in tempCollection
                         orderby music.Title
                         select music;
            AllMusicCollection.Clear();
            foreach (var music in musics)
            {
                AllMusicCollection.Add(music);
            }
        }
        #endregion // Method
    }
}