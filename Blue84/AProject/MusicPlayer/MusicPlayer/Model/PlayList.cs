using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
namespace MusicPlayer.Model
{
    public class PlayList : ObservableCollection<MusicInfo>
    {
        #region Field

        private string _dirpath;

        #endregion // Field

        #region Property

        public string DirPath
        {
            get { return _dirpath; }
            set
            {
                if (_dirpath != value)
                {
                    _dirpath = value;
                    ChangeDirPath();
                }
            }
        }
        #endregion // Property


        //改变文件夹列表路径
        private void ChangeDirPath()
        {
            Clear();
            DirectoryInfo dir = new DirectoryInfo(DirPath);
            FileInfo[] files = dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    MusicInfo musicInfo = new MusicInfo(file.FullName);
                    base.Add(musicInfo);
                }
            }
        }
        public PlayList(string path)
        {
            DirPath = path;
        }

        public PlayList(ObservableCollection<MusicInfo> list)
        {
            foreach (var music in list)
            {
                Add(music);
            }
        }
        public PlayList()
        {

        }
    }
}
