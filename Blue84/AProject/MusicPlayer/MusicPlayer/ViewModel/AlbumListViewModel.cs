using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.Model;
namespace MusicPlayer.ViewModel
{
    /// <summary>
    /// 专辑列表的ViewModel，负责控制专辑列表的数据
    /// </summary>
    public class AlbumListViewModel : ViewModelBase
    {
        #region Field

        private ObservableCollection<AlbumInfo> _allAlbumCollection;

        #endregion // Field

        #region Property

        /// <summary>
        /// 所有专辑列表
        /// </summary>
        public ObservableCollection<AlbumInfo> AllAlbumCollection
        {
            get { return _allAlbumCollection ?? (_allAlbumCollection = new ObservableCollection<AlbumInfo>()); }
            set
            {
                _allAlbumCollection = value;
                RaisePropertyChanged("AllAlbumCollection");
            }
        }

        /// <summary>
        /// 所有音乐集合
        /// </summary>
        public ObservableCollection<MusicInfo> AllMusicCollection { get; set; }

        #endregion // Property

        #region Method

        /// <summary>
        /// 注册接受，当MainMusicListViewModel载入歌曲完成时将发送所有歌曲信息。
        /// </summary>
        void RegistMessage()
        {
            Messenger.Default.Register<MusicLoadedMessage>(this, true, p =>
            {
                AllMusicCollection = p.AllMusic;
                GetAlbumInfo();
                //SortAlbum();
            });
        }

        /// <summary>
        /// 通过所有歌曲分类出专辑列表
        /// </summary>
        public void GetAlbumInfo()
        {
            var allAlbumCollection = new ObservableCollection<AlbumInfo>();
            //找到所有专辑的名字
            var info = (from music in AllMusicCollection
                        select music.Album).Distinct();

            foreach (var albumInfo in info)
            {
                //找到该专辑的信息
                var album = (from music in AllMusicCollection
                             where music.Album == albumInfo
                             select new AlbumInfo { Name = music.Album, Artist = music.Artist, CoverImage = music.CoverImage }).ToList().First();
                //找到该专辑内所有曲目
                var albumMusic = from music in AllMusicCollection
                                 where music.Album == album.Name
                                 select music;
                foreach (var music in albumMusic)
                {
                    album.AllMusic.Add(music);
                }
                //添加专辑到专辑列表

                allAlbumCollection.Add(album);
            }

            AllAlbumCollection = allAlbumCollection;
        }

        #endregion // Method

        #region Init&Cleanup

        /// <summary>
        /// Initializes a new instance of the AlbumList class.
        /// </summary>
        public AlbumListViewModel()
        {
            RegistMessage();
            
        }
        #endregion

    }
}