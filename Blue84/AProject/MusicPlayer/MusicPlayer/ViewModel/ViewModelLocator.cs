namespace MusicPlayer.ViewModel
{

    public class ViewModelLocator
    {
        #region MusicList
        private static MainMusicListViewModel _musicListViewModel;

        /// <summary>
        /// Gets the MusicList property.
        /// </summary>
        public static MainMusicListViewModel MusicListStatic
        {
            get
            {
                if (_musicListViewModel == null)
                {
                    CreateMusicList();
                }

                return _musicListViewModel;
            }
        }

        /// <summary>
        /// Gets the MusicList property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainMusicListViewModel MusicList
        {
            get
            {
                return MusicListStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the MusicList property.
        /// </summary>
        public static void ClearMusicList()
        {
            //_musicListViewModel.Cleanup();
            _musicListViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the MusicList property.
        /// </summary>
        public static void CreateMusicList()
        {
            if (_musicListViewModel == null)
            {
                _musicListViewModel = new MainMusicListViewModel();
            }
        }
        #endregion // MusicList

        #region Playing
        private static PlayingViewModel _playingViewModel;

        /// <summary>
        /// Gets the Playing property.
        /// </summary>
        public static PlayingViewModel PlayingStatic
        {
            get
            {
                if (_playingViewModel == null)
                {
                    CreatePlaying();
                }

                return _playingViewModel;
            }
        }

        /// <summary>
        /// Gets the Playing property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PlayingViewModel Playing
        {
            get
            {
                return PlayingStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Playing property.
        /// </summary>
        public static void ClearPlaying()
        {
            //_playingViewModel.Cleanup();
            _playingViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the Playing property.
        /// </summary>
        public static void CreatePlaying()
        {
            if (_playingViewModel == null)
            {
                _playingViewModel = new PlayingViewModel();
            }
        }

        #endregion // Playing

        #region MainViewModel
        private static MainViewModel _mainViewModel;

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public static MainViewModel MainStatic
        {
            get
            {
                if (_mainViewModel == null)
                {
                    CreateMain();
                }

                return _mainViewModel;
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return MainStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Main property.
        /// </summary>
        public static void ClearMain()
        {
            //_mainViewModel.Cleanup();
            _mainViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void CreateMain()
        {
            if (_mainViewModel == null)
            {
                _mainViewModel = new MainViewModel();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        #endregion
        #region Init&Clean
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view models
            ////}
            ////else
            ////{
            ////    // Create run time view models
            ////}
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>

        public static void Cleanup()
        {
            ClearMain();
            ClearMusicList();
            ClearPlaying();
        }
        #endregion // Init&Clean
 
    }
}