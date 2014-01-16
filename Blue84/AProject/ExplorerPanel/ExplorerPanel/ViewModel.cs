using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
namespace ExplorerPanel
{
    public class ViewModel
    {
        #region 资源管理浏览器
        public static string DefaultRootPath = Environment.GetLogicalDrives()[0];//Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        public static string DefaultMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public static string DefaultVideoPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        public static string DefaultPicturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static string DefaultBookPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public ViewModel()
        {
             CurrentDirectoryPath = DefaultRootPath;
        }
        public string CurrentDirectoryPath;
        public string[] GetFiles()
        {
            try
            {
                return Directory.GetFiles(CurrentDirectoryPath);
            }
            catch { return null; }
        }
        public string[] GetDirectories()
        {
            try
            {
                return Directory.GetDirectories(CurrentDirectoryPath);
            }
            catch { return null; }
        }
        public static bool IsImageFileExtension(string extension)
        {
            return (App.Current as App).SupportedPictureExtensionList.Contains(extension.ToUpper());
        }
        public static bool IsVideoFileExtension(string extension)
        {
            return (App.Current as App).SupportedVideoExtensionList.Contains(extension.ToUpper());
        }
        public static bool IsMusicFileExtension(string extension)
        {
            return (App.Current as App).SupportedMusicExtensionList.Contains(extension.ToUpper());
        }
        public static bool IsBookFileExtension(string extension)
        {
            return (App.Current as App).SupportedBookExtensionList.Contains(extension.ToUpper());
        }
        public static string GetDirectoryName(string fullPath)
        {
            string s = fullPath.Substring(fullPath.LastIndexOf('\\') + 1);
            if (s == "")
            {
                return fullPath;
            }
            return s;
        }
        public static bool IsSystemVisible(string path)
        {
            FileAttributes fa = File.GetAttributes(path);
            if (fa.HasFlag(FileAttributes.Hidden))
            {
                return false;
            }
            return true;
        }//判断该文件是否是隐藏文件
        public static bool IsFunctionalVisible(string path)
        {
            //if (!IsSystemVisible(path))
            //{ return false; }
            if (!(App.Current as App).ShowOnlySupportedFile)
            { return true; }
            string extension = Path.GetExtension(path);
            if (IsImageFileExtension(extension)
                || IsVideoFileExtension(extension)
                || IsMusicFileExtension(extension)
                || IsBookFileExtension(extension)
                )
            {
                return true;
            }
            return false;

        }//判断该文件是否是图片音乐等本系统可用文件
        #endregion
        #region 复制删除
        #region 变量
        public int TotalCount
        {
            get 
            {
                return ChoosenDirectoryList.Count + ChoosenFileList.Count;
            }
        }
        public List<string> ChoosenFileList = new List<string>();//选中的文件路径被添加在此表中
        public List<string> ChoosenDirectoryList = new List<string>();//选中的目录路径被添加在此表中
        private BackgroundWorker DeleteWorker;//删除文件的线程
        private BackgroundWorker CopyWorker;//复制文件的线程
        #endregion
        #region 功能函数
        public void BeginCopy(FileClass fclass)
        {
            if (CopyWorker == null) { InitBackgroundWorker();}
            if (!CopyWorker.IsBusy)
            {
                CopyWorker.RunWorkerAsync(GetNewImportFolderPath(fclass));
            }
        }
        public void BeginDelete()
        {
            if (DeleteWorker == null) { InitBackgroundWorker(); }
            if (!DeleteWorker.IsBusy)
            {
                DeleteWorker.RunWorkerAsync();
            }
        }
        public void CancelCopy()
        {
            if (CopyWorker.IsBusy)
            {
                CopyWorker.CancelAsync();
            }
        }//中断复制
        public void CancelDelete()
        {
            if (DeleteWorker.IsBusy)
            {
                DeleteWorker.CancelAsync();
            }
        }//中断删除

        private string GetNewImportFolderPath(FileClass fclass)
        {
            string rootPath = null;
            #region getBasePath
            switch (fclass)
            {
                case FileClass.Music:
                    {
                        rootPath = DefaultMusicPath;
                    } break;
                case FileClass.Video:
                    {
                        rootPath = DefaultVideoPath;
                    } break;
                case FileClass.Picture:
                    {
                        rootPath = DefaultPicturePath;
                    } break;
                case FileClass.Book:
                    {
                        rootPath = DefaultBookPath;
                    } break;
                default: return null;
            }
            #endregion
            string dirName = "导入文件" + DateTime.Now.ToString();
            dirName = dirName.Replace('/', '_').Replace(':', '-');
            string path = rootPath + "\\" + dirName;
            return path;
        }//获取fclass对应的文件导入路径
        private void InitBackgroundWorker()
        {
            if (CopyWorker == null)
            {
                CopyWorker = new System.ComponentModel.BackgroundWorker();
                CopyWorker.WorkerReportsProgress = true;
                CopyWorker.WorkerSupportsCancellation = true;
                CopyWorker.DoWork += (sender, e) =>
                {
                    System.Threading.Thread.Sleep(500);
                    ExcuteCopyPaste((string)e.Argument);
                };
                CopyWorker.ProgressChanged += (sender, e) =>
                {
                    if (CopyProgressChanged != null)
                        CopyProgressChanged(e.ProgressPercentage);
                };
                CopyWorker.RunWorkerCompleted += (sender, e) =>
                {
                    if (CopyCompleted != null)
                        CopyCompleted();
                };
            }
            if (DeleteWorker == null)
            {
                DeleteWorker = new System.ComponentModel.BackgroundWorker();
                DeleteWorker.WorkerReportsProgress = true;
                DeleteWorker.WorkerSupportsCancellation = true;
                DeleteWorker.DoWork += (sender, e) => { System.Threading.Thread.Sleep(500); ExcuteDelete(); };
                DeleteWorker.ProgressChanged += (sender, e) =>
                {
                    if (DeleteProgressChanged != null)
                        DeleteProgressChanged(e.ProgressPercentage);
                };
                DeleteWorker.RunWorkerCompleted += (sender, e) =>
                {
                    if (DeleteCompleted != null)
                        DeleteCompleted();
                };
            }
        }
        private void ExcuteDelete()
        {
            int totcount = TotalCount;
            for (int i = 0; i < ChoosenFileList.Count; i++)
            {
                if (DeleteWorker.CancellationPending) { return; }
                if (DeleteWorker != null)
                {
                    DeleteWorker.ReportProgress(100 *( i+1) / totcount);
                }
                try
                {
                    if (File.Exists(ChoosenFileList[i]))
                    {
                        try
                        {
                            if (File.GetAttributes(ChoosenFileList[i]) == FileAttributes.ReadOnly)
                            {
                                File.SetAttributes(ChoosenFileList[i], FileAttributes.Normal);
                            }
                            File.Delete(ChoosenFileList[i]);
                        }
                        catch { continue; }
                    }
                }
                catch { continue; }
            }
            for (int i = 0; i < ChoosenDirectoryList.Count; i++)
            {
                if (DeleteWorker.CancellationPending) { return; }
                if (DeleteWorker != null)
                {
                    DeleteWorker.ReportProgress(100 * (1+i) / totcount);
                }
                try
                {
                    if (Directory.Exists(ChoosenDirectoryList[i]))
                    {
                        try
                        { Directory.Delete(ChoosenDirectoryList[i], true); }
                        catch { continue; }
                    }
                }
                catch { continue; }
            }
        }//删除执行函数
        private void ExcuteCopyPaste(string targetParentDirPath)
        {
            int tcount = TotalCount;
            if (!Directory.Exists(targetParentDirPath))
            {
                try
                {
                    Directory.CreateDirectory(targetParentDirPath);
                }
                catch { return ; }
            }
            for (int i = 0; i < ChoosenFileList.Count; i++)
            {
                if (CopyWorker.CancellationPending) { return; }
                try
                {
                    if (File.Exists(ChoosenFileList[i]))
                    {
                        File.Copy(ChoosenFileList[i], targetParentDirPath + "\\" + Path.GetFileName(ChoosenFileList[i]), true);
                    }
                    CopyWorker.ReportProgress((i+1)* 100 / tcount);
                }
                catch { continue; }
            }
            for (int i = 0; i < ChoosenDirectoryList.Count; i++)
            {
                if (CopyWorker.CancellationPending) { return; }
                try
                {
                    if (Directory.Exists(ChoosenDirectoryList[i]))
                    {
                        CopyDirectory(ChoosenDirectoryList[i], targetParentDirPath);
                    }
                    CopyWorker.ReportProgress((i + 1) * 100 / tcount);
                }
                catch { continue; }
            }
        }//复制执行函数
        private void CopyDirectory(string srcdir, string desdir)
        {
            if (CopyWorker.CancellationPending) { return; }
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);
            string desfolderdir = desdir + "\\" + folderName;
            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (CopyWorker.CancellationPending) { return; }
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }
                else // 否则直接copy文件
                {
                    if (IsFunctionalVisible(file))
                    {
                        string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                        srcfileName = desfolderdir + "\\" + srcfileName;
                        if (!Directory.Exists(desfolderdir))
                        {
                            Directory.CreateDirectory(desfolderdir);
                        }
                        File.Copy(file, srcfileName, true);
                    }
                }
            }
        }//递归复制文件夹
        #endregion
        #region 事件
        public delegate void CopyProgressChangedEventHandler(int percentage);
        public event CopyProgressChangedEventHandler CopyProgressChanged;
        public delegate void CopyCompletedEventHandler();
        public event CopyCompletedEventHandler CopyCompleted;
        public delegate void DeleteProgressChangedEventHandler(int percentage);
        public event DeleteProgressChangedEventHandler DeleteProgressChanged;
        public delegate void DeleteCompletedEventHandler();
        public event DeleteCompletedEventHandler DeleteCompleted;
        #endregion
        #endregion
    }
    public enum FileClass
    {
        Folder = 0, File = 1, Picture = 2, Music = 3, Video = 4, Book = 6,
        Driver = 5,
    }
}
