using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Model;
namespace MusicPlayer.ViewModel
{

    public class PlayingViewModel : ViewModelBase
    {
        #region Field

        Player _player;

        PlayList _playList;

        //当前播放的音乐信息
        MusicInfo _currentMusic;

        bool _isShuffe;

        bool _isSigle;

        bool _isPlaying = true;

        #endregion // Field

        #region Property

        public Player Player
        {
            get
            {
                if (_player == null)
                    _player = new Player();
                return _player;
            }
        }
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
                RaisePropertyChanged("IsPlaying");
            }
        }
        public PlayList PlayList
        {
            get
            {
                if (_playList == null)
                {
                    _playList = new PlayList();
                }
                return _playList;
            }
            set
            {
                if (_playList != value)
                {
                    _playList = value;
                    RaisePropertyChanged("PlayList");
                }
            }
        }
        public bool IsSigle
        {
            get
            {
                return _isSigle;
            }
            set
            {
                if (_isSigle != value)
                {
                    _isSigle = value;
                    RaisePropertyChanged("IsSigle");
                }
            }
        }
        public bool IsShuffe
        {
            get
            {
                return _isShuffe;
            }
            set
            {
                if (_isShuffe != value)
                {
                    _isShuffe = value;
                    RaisePropertyChanged("IsShuffe");
                }
            }
        }

        public MusicInfo CurrentMusic
        {
            get
            {
                return _currentMusic;
            }
            set
            {
                //当当前音乐改变时，重新打开音乐流
                if (value != null)
                {
                    _currentMusic = value;
                    Player.Open(new Uri(value.Path, UriKind.RelativeOrAbsolute));
                    Player.Play();
                    RaisePropertyChanged("CurrentMusic");
                    IsPlaying = true;
                }
            }
        }


        #endregion

        #region Commands

        RelayCommand<double> _seekCommand;

        public ICommand SeekCommand
        {
            get
            {
                if (_seekCommand == null)
                {
                    _seekCommand = new RelayCommand<double>(Seek);
                }
                return _seekCommand;
            }
        }
        RelayCommand _playCommand;

        public ICommand PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new RelayCommand(() => {
                        try
                        {
                            Player.Play();
                            IsPlaying = true;
                        }
                        catch (Exception)
                        { }
                    });
                }
                return _playCommand;
            }
        }
        RelayCommand _pauseCommand;

        public ICommand PauseCommand
        {
            get
            {
                if (_pauseCommand == null)
                {
                    _pauseCommand = new RelayCommand(() => { Player.Pause(); IsPlaying = false; });
                }
                return _pauseCommand;
            }
        }

        RelayCommand _nextCommand;

        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(Next);
                }
                return _nextCommand;
            }
        }
        RelayCommand _previousCommand;

        public ICommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(Previous);
                }
                return _previousCommand;
            }
        }

        RelayCommand _toMusicList;

        public ICommand ToMusicListCommand
        {
            get
            {
                if (_toMusicList == null)
                {
                    _toMusicList = new RelayCommand(() => Messenger.Default.Send<bool, MainViewModel>(true));
                }
                return _toMusicList;
            }
        }
        #endregion // Commands

        #region Init

        #endregion // Init
        public PlayingViewModel()
        {
            RegistMessenger();
            Player.MediaEnded += (sender, e) => Next();
        }

        #region Methods

        private void RegistMessenger()
        {
            //文件夹路径的播放列表
            Messenger.Default.Register<string>(this, true, p =>
            {
                PlayList.DirPath = p;
            });
            Messenger.Default.Register<MusicPlayMessage>(this, true, p =>
            {

                PlayList = new PlayList(p.MusicList);
                CurrentMusic = p.SelectedMusic;
            });
        }

        private void Next()
        {
            try
            {
                var currentIndex = (from music in PlayList
                                    where music == CurrentMusic
                                    select PlayList.IndexOf(music)).ToList().First();

                //单曲播放
                if (IsSigle)
                    CurrentMusic = PlayList[currentIndex];
                //列表播放
                else
                {
                    //列表随机
                    int nextIndex;
                    if (IsShuffe)
                    {
                        do
                        {
                            var random = new Random();
                            nextIndex = random.Next(0, PlayList.Count);
                        } while (currentIndex == nextIndex);
                        CurrentMusic = PlayList[nextIndex];
                    }
                    //列表播放
                    else
                    {
                        nextIndex = currentIndex + 1;
                        if (nextIndex >= PlayList.Count)
                        {
                            nextIndex = 0;
                        }
                        CurrentMusic = PlayList[nextIndex];
                    }
                }
            }
            catch { CurrentMusic = PlayList[0]; }

        }

        private void Previous()
        {
            try
            {
                var currentIndex = (from music in PlayList
                                    where music == CurrentMusic
                                    select PlayList.IndexOf(music)).ToList().First();

                //单曲播放
                if (IsSigle)
                    CurrentMusic = PlayList[currentIndex];
                //列表播放
                else
                {
                    //列表随机
                    int nextIndex;
                    if (IsShuffe)
                    {
                        do
                        {
                            var random = new Random();
                            nextIndex = random.Next(0, PlayList.Count);
                        }
                        while (currentIndex == nextIndex);
                        CurrentMusic = PlayList[nextIndex];
                    }
                    //列表播放
                    else
                    {
                        nextIndex = currentIndex - 1;
                        if (nextIndex < 0)
                        {
                            nextIndex = PlayList.Count - 1;
                        }
                        CurrentMusic = PlayList[nextIndex];
                    }
                }
            }
            catch { CurrentMusic = PlayList[0]; }


        }

        private void Seek(double secondsToSeek)
        {
            Player.Position = TimeSpan.FromSeconds(secondsToSeek);
        }

        #endregion
    }
}