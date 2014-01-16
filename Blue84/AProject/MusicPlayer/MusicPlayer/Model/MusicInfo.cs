using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagLib;
using System.Windows.Media.Imaging;
using System.IO;
using Shell32;
namespace MusicPlayer.Model
{
    public class MusicInfo
    {
        public string Album { get; protected set; }
        public string Artist { get; protected set; }
        public string Title { get; protected set; }
        public BitmapImage CoverImage { get; protected set; }
        public int SampleRate { get; protected set; }
        public string Path { get; protected set; }
        public int Track { get; set; }
        public TimeSpan Duration { get; set; }
        public int Bitrate { get; set; }
        public MusicInfo(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    TagLib.File file = TagLib.File.Create(path);
                    Shell shell = new Shell();
                    string p = path.ToString();
                    Folder dir = shell.NameSpace(System.IO.Path.GetDirectoryName(p));
                    FolderItem item = dir.ParseName(System.IO.Path.GetFileName(p));
                    Path = path;
                    Title = dir.GetDetailsOf(item, 21);
                    if (String.IsNullOrEmpty(Title))
                        Title = GetTitleFromPath(path);
                    Album = dir.GetDetailsOf(item, 14);
                    if (String.IsNullOrEmpty(Album))
                        Album = "未知专辑";
                    Artist = dir.GetDetailsOf(item, 13);
                    if (String.IsNullOrEmpty(Artist))
                        Artist = "未知歌手";
                    //Tarck
                    Track = (int)file.Tag.Track;

                    //封面
                    if (file.Tag.Pictures.Count() != 0)
                    {
                        CoverImage = ByteToBitmapImage(file.Tag.Pictures[0].Data.Data);
                        if (CoverImage.CanFreeze)
                        {
                            CoverImage.Freeze();
                        }
                    }

                    SampleRate = file.Properties.AudioSampleRate;
                    Duration = file.Properties.Duration;
                    Bitrate = file.Properties.AudioBitrate;
                    file.Dispose();
                }
            }
            catch { }
        }

        #region Helpers
        /// <summary>
        /// 通过路径获取歌曲标题
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>标题</returns>
        private string GetTitleFromPath(string path)
        {
            FileInfo info = new FileInfo(path);
            var strings = info.Name.Split(new char[] { '.' });
            return strings.First();
        }
        private BitmapImage ByteToBitmapImage(byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            return image;
        }
        private string DecodeString(string s)
        {
            System.IO.FileStream streamMusicFile = System.IO.File.OpenRead(Path);
            streamMusicFile.Seek(20, SeekOrigin.Begin);
            int intEncoding = streamMusicFile.ReadByte();
            streamMusicFile.Close();
            streamMusicFile.Dispose();
            //当intencoding为0时表示需要转码
            if (intEncoding == 0)
            {
                var encoding = Encoding.GetEncoding("Latin1");
                byte[] sourceBytes = encoding.GetBytes(s);
                byte[] destBytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, sourceBytes);
                var outstring = Encoding.UTF8.GetString(destBytes);
                return outstring;
            }
            else return s;
        }
        #endregion
    }
}
