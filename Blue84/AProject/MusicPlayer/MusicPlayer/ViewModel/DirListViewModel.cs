using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
namespace MusicPlayer.ViewModel
{
    public class DirListViewModel : ViewModelBase
    {
        #region Field

        private ObservableCollection<DirectoryInfo> _allMusicDir;

        #endregion // Field

        #region Property

        public ObservableCollection<DirectoryInfo> AllMusicDir
        {
            get
            {
                if (_allMusicDir != null) return _allMusicDir;
                _allMusicDir = new ObservableCollection<DirectoryInfo>();
                return _allMusicDir;
            }
        }

        public DirectoryInfo SelectedDir { get; set; }

        #endregion // Property

        #region Method
        private void GetAllMusicDir()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            var dirInfo = new DirectoryInfo(path);
            if ((dirInfo.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).Length != 0)
                || (dirInfo.GetFiles("*.wma", SearchOption.TopDirectoryOnly).Length != 0))
                AllMusicDir.Add(dirInfo);
            var subMusicDirs = from dir
                               in dirInfo.EnumerateDirectories("*", SearchOption.AllDirectories)
                               where (dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).Length != 0)
                                     || (dir.GetFiles("*.wma", SearchOption.TopDirectoryOnly).Length != 0)
                               select dir;
            foreach (var dir in subMusicDirs)
            {
                AllMusicDir.Add(dir);
            }
        }

        #endregion // Methods

        #region Init
        /// <summary>
        /// Initializes a new instance of the DirListViewModel class.
        /// </summary>
        public DirListViewModel()
        {
            GetAllMusicDir();
        }
        #endregion // Init

        #region Command
        RelayCommand _selectedChange;

        public ICommand SelectedChangeCommand
        {
            get
            {
                if (_selectedChange != null) return _selectedChange;
                _selectedChange = new RelayCommand(() =>
                                                       {
                                                           Messenger.Default.Send<string, PlayingViewModel>(SelectedDir.FullName);
                                                           Messenger.Default.Send<bool, MainViewModel>(false);
                                                       }
                    );
                return _selectedChange;
            }
        }
        #endregion // Command
    }
}