using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;

namespace VideoPanel
{
    public class ViewModel
    {
        public ViewModel()
        {
            InitBackgroundWorker();//初始化所有后台线程
            EnsureBackgroundWorkWorking();//该定时器定时调用方法检查线程工作状态（非必须，但可提高可靠性）
        }
        #region private
        List<List<VideoInfo>> allVideoInfoMatrix;
        List<VideoInfo> allVideoInfoList;
        List<string> allDirectoryList = new List<string>();
        private void MergeDirectoris(string path, ref List<string> allDirectories, ref List<List<VideoInfo>> allMatrixs)
        {
            #region merge this dir
            List<VideoInfo> topfileList = new List<VideoInfo>();
            string[] allTopFiles = Directory.GetFiles(path, "*");
            for (int j = 0; j < allTopFiles.Length; j++)
            {
                if (VideoInfo.IsVideoFileExtension(System.IO.Path.GetExtension(allTopFiles[j])))
                {
                    VideoInfo vi = new VideoInfo(allTopFiles[j]);
                    topfileList.Add(vi);
                }
            }
            if (topfileList.Count != 0)
            {
                allDirectories.Add(path);
                allMatrixs.Add(topfileList);
            }
            #endregion//读取当前文件夹
            #region merge child dirs
            string[] allDirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < allDirs.Length; i++)
            {
                if (allDirs[i].Contains(VideoInfo.ThumbDirectoryName)) { continue; }
                List<VideoInfo> list = new List<VideoInfo>();
                string[] files = System.IO.Directory.GetFiles(allDirs[i], "*", System.IO.SearchOption.TopDirectoryOnly);
                for (int j = 0; j < files.Length; j++)
                {
                    if (VideoInfo.IsVideoFileExtension(System.IO.Path.GetExtension(files[j])))
                    {
                        VideoInfo vi = new VideoInfo(files[j]);
                        list.Add(vi);
                    }
                }
                if (list.Count != 0)
                {
                    allDirectories.Add(allDirs[i]);
                    allMatrixs.Add(list);
                }
            }
            #endregion//读取当前文件夹的子文件夹
        }
        private void ClearRedundancy(string directoryPath, string pattern)
        {
            if (!Directory.Exists(directoryPath)) { return; }
            string[] files = Directory.GetFiles(directoryPath);
            if (files == null) { return; }
            for (int i = 0; i < files.Length; i++)
            {
                string OriginalName = Path.GetFileName(files[i]).Replace(pattern, "");
                string OriginalPath = Path.GetDirectoryName(Path.GetDirectoryName(files[i])) + "\\" + OriginalName;
                if (!File.Exists(OriginalPath))
                {
                    File.Delete(files[i]);
                }
            }
        }//本函数作用是清除无源文件（可能已删除）的缩略图（_thumb 与_small）
        #endregion    
        public List<List<VideoInfo>> AllVideoInfoMatrix
        {
            get { return allVideoInfoMatrix; }
        }//获取按二维数数组组织的VideoInfo
        public List<List<VideoInfo>> GetAllVideoInfoMatrix()
        {
            if (allVideoInfoMatrix != null) { return allVideoInfoMatrix; }
            else
            {
                allVideoInfoMatrix = new List<List<VideoInfo>>();
                #region merge default dirs
                MergeDirectoris(App.DefaultPath, ref allDirectoryList, ref allVideoInfoMatrix);
                #endregion
                #region merge extra dirs
                List<string> mergedDirs = (App.Current as App).ValidExtDirectoires;
                for (int i = 0; i < mergedDirs.Count; i++)
                {
                    MergeDirectoris(mergedDirs[i], ref allDirectoryList, ref allVideoInfoMatrix);
                }
                #endregion
                RunGuardWorker();
                return allVideoInfoMatrix;
            }
        }//获取按二维数数组组织的VideoInfo
        public List<VideoInfo> GetAllVideoInfoList()
        {
            if (allVideoInfoList != null) { return allVideoInfoList; }
            else
            {
                allVideoInfoList = new List<VideoInfo>();
                List<List<VideoInfo>> matrix = GetAllVideoInfoMatrix();
                for (int i = 0; i < matrix.Count; i++)
                {
                    for (int j = 0; j < matrix[i].Count; j++)
                    {
                        allVideoInfoList.Add(matrix[i][j]);
                    }
                }
                return allVideoInfoList;
            }
        }//获取一维数组组织的VideoInfo
        public List<VideoInfo> AllVideoInfoList
        {
            get { return allVideoInfoList; }
        }//获取一维数组组织的VideoInfo
        public List<string> AllDirectories
        {
            get { return allDirectoryList; }
        }
        #region 后台线程
        bool isCleaned = false;//该值指示guardWorker是否已经运行一次
        BackgroundWorker fileSystemWorker;//文件系统线程 该线程用于异步获取所有视频文件路径
        BackgroundWorker loadDetailWorker;//信息加载线程 该线程用于异步获取所有视频文件的详细信息（制作缩略图）本线程存在可靠性问题
        BackgroundWorker guardWorker;//守护线程 该后台线程负责在启动后检查，删除多余缩略图
        DispatcherTimer timer_Ensure;//该定时器定时调用方法检查loadDetailWorker线程工作状态（非必须，但可提高可靠性）
        private void EnsureBackgroundWorkWorking()
        {
            if (timer_Ensure == null)
            {
                #region 初始化timer_Ensure
                timer_Ensure = new DispatcherTimer(DispatcherPriority.Background);
                timer_Ensure.Interval = TimeSpan.FromSeconds(1.5);
                timer_Ensure.Tick += (sender, e) =>
                {
                    if (loadDetailWorker != null && DetaiQueue.Count != 0 && !loadDetailWorker.IsBusy)
                    {
                        RunDetailWorker();
                    }
                };
                #endregion
            }
            timer_Ensure.Start();
        }
        private void InitBackgroundWorker()
        {
            #region 文件系统线程初始化
            fileSystemWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            fileSystemWorker.DoWork += (sender, e) =>
            {
                GetAllVideoInfoMatrix();
            };
            fileSystemWorker.RunWorkerCompleted += (sender, e) =>
            {
                if (FileSystemWorkCompleted != null)
                    FileSystemWorkCompleted();
            };
            #endregion
            #region 信息加载线程初始化
            loadDetailWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            loadDetailWorker.DoWork += (sender, e) =>
            {
                //Shell32.Shell shell = new Shell32.Shell();//若需获取其他信息 取消相关注释
                while (DetaiQueue.Count != 0)
                {
                    VideoInfo currentWorkingObj;
                    lock (DetaiQueue)
                    {
                        currentWorkingObj = DetaiQueue.Dequeue();
                    }
                    #region 制作并加载缩略图
                    VideoInfo.EnsureThumb(currentWorkingObj);
                    #endregion
                    #region 获取详细信息 如帧宽高 比特率等
                    //Shell32.Folder dir = shell.NameSpace(System.IO.Path.GetDirectoryName(pi.AbsolutePath));
                    //Shell32.FolderItem item = dir.ParseName(System.IO.Path.GetFileName(pi.AbsolutePath));
                    //pi.FrameWidth = dir.GetDetailsOf(item, 282);
                    //pi.FrameHeight = dir.GetDetailsOf(item, 280);
                    #endregion
                    loadDetailWorker.ReportProgress(0, currentWorkingObj);//报告进度currentWorkingObj已经加载完毕
                }
            };
            loadDetailWorker.ProgressChanged += (sender, e) =>
            {
                if (LoadDetailsProgressChanged != null)
                {
                    LoadDetailsProgressChanged((e.UserState as VideoInfo));
                }
            };
            loadDetailWorker.RunWorkerCompleted += (sender, e) => 
            {
                if (DetailWorkCompleted != null)
                {
                    DetailWorkCompleted();
                }
            };
            #endregion
            #region 守护线程初始化
            guardWorker = new BackgroundWorker() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
            guardWorker.DoWork += (sender, e) =>
            {
                System.Threading.Thread.Sleep(2000);
                #region ClearPicture
                if (!isCleaned)
                {
                    for (int i = 0; i < allDirectoryList.Count; i++)
                    {
                        try
                        {
                            string thumbpath = allDirectoryList[i] + "\\" + VideoInfo.ThumbDirectoryName;
                            ClearRedundancy(thumbpath, VideoInfo.ThumbSuffix);
                        }
                        catch { continue; }
                    }
                    isCleaned = true;
                }
                #endregion
            };
            #endregion
        }
        #region events
        public delegate void FileSystemWorkCompletedEventHandler();
        public event FileSystemWorkCompletedEventHandler FileSystemWorkCompleted;
        public delegate void DetailWorkCompletedEventHandler();
        public event DetailWorkCompletedEventHandler DetailWorkCompleted;
        public delegate void LoadDetailsProgressChangedEventHandler(VideoInfo pi);
        public event LoadDetailsProgressChangedEventHandler LoadDetailsProgressChanged;
        #endregion
        public void RunFileSystemWorker()
        {
            if (fileSystemWorker != null && !fileSystemWorker.IsBusy)
            {
                fileSystemWorker.RunWorkerAsync();
            }
        }//启动本线程
        public void RunDetailWorker()
        {
            if (loadDetailWorker != null && !loadDetailWorker.IsBusy)
            {
                loadDetailWorker.RunWorkerAsync();
            }
        }//启动本线程
        public void RunGuardWorker()
        {
            if (guardWorker != null && !guardWorker.IsBusy)
            {
                guardWorker.RunWorkerAsync();
            }
        }//启动本线程
        #region workqueue
        Queue<VideoInfo> DetaiQueue = new Queue<VideoInfo>(20);//信息加载线程的任务队列,包含所有没有详细信息的VideoInfo对象
        public void AddWorkingItem(VideoInfo vi)
        {
            lock (DetaiQueue)
            {
                DetaiQueue.Enqueue(vi);
            }
        }//添加单个VideoInfo对象进入队列
        public void AddWorkingItem(List<VideoInfo> list, int startIndex, int endIndex)
        {
            lock (DetaiQueue)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    DetaiQueue.Enqueue(list[i]);
                }
            }
        }//添加一组VideoInfo对象进入队列
        #endregion
        #endregion
    }
    public class VideoInfo
    {
        public static readonly string ThumbSuffix = "_thumb.jpg";//缩略图特征后缀，所有的视频文件后缀为 文件名+ThumbSuffix 例子： a.avi  =》 a.avi_thumb.jpg
        public static readonly string ThumbDirectoryName = "thumbs";//缩略图目录名
        public BitmapImage ThumbImage;//视频的缩略图bitmapImage对象
        public string AbsolutePath;
        public string Name
        {
            get { return System.IO.Path.GetFileName(AbsolutePath); }
        }//文件名 ,带后缀名 
        public string NameWithoutExtension
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(AbsolutePath); }
        }//文件名 ,不带后缀名 
        public string Extension
        {
            get { return System.IO.Path.GetExtension(AbsolutePath); }
        }//后缀名
        public string DirectoryName
        {
            get { return DirectoryPath.Substring(DirectoryPath.LastIndexOf('\\') + 1); }
        }//所在目录的名字
        public string DirectoryPath
        {
            get { return System.IO.Path.GetDirectoryName(AbsolutePath); }
        }//所在目录路径
        public string ThumbPath
        {
            get
            {
                string filename = System.IO.Path.GetFileName(AbsolutePath);
                return DirectoryPath + "/thumbs/" + filename + ThumbSuffix;
            }
        }//缩略图路径
        public bool? IsLoadSuccessful //指示改对象是否已经成功加载缩略图为null表示为加载完毕 为false表示加载失败
        { get; set; }
        public VideoInfo(string path)
        {
            AbsolutePath = path;
        }
        public static bool IsVideoFileExtension(string extension)
        {
            return (App.Current as App).SupportedExtensionList.Contains(extension.ToUpper());
        }//检测后缀名是否是一个视频的后缀
        public static BitmapImage CreatePicture(string path)
        {
            if (!System.IO.File.Exists(path)) { return null; }
            try
            {
                BitmapImage bitmapImage = null;
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    FileInfo fi = new FileInfo(path);
                    byte[] bytes = reader.ReadBytes((int)fi.Length);
                    reader.Close();
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(bytes);
                    bitmapImage.EndInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                }
                return bitmapImage;
            }
            catch { return null; }
        }//按path指示的路径读取图片 失败返回null
        public static bool CreateThumb(VideoInfo videoInfo)
        {
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(videoInfo.ThumbPath)))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(videoInfo.ThumbPath));
                    File.SetAttributes(System.IO.Path.GetDirectoryName(videoInfo.ThumbPath), System.IO.FileAttributes.Hidden);
                }
                catch { return false; }
            }
            if (MakeThumb(videoInfo.AbsolutePath, videoInfo.ThumbPath, (App.Current as App).ThumbFramePositon, (App.Current as App).ThumbWidth, (App.Current as App).ThumbHeight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }//为参数对象创建缩略图
        private static bool MakeThumb(string sourcePath, string thumbTargetPath, int sleep, int width, int height)
        {
            try
            {
                if (File.Exists(thumbTargetPath)) { return true; }
                System.Threading.Thread.Sleep(100);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.WorkingDirectory = @"VideoPack";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = @"VideoPack\ffmpeg.exe";
                string s = "-i \"%1\" -ss %2 -vframes 1 -r 1 -ac 1 -ab 4 -s " + width + "*" + height + " -f  image2 \"%3\"";
                s = s.Replace("%1", sourcePath).Replace("%2", sleep.ToString()).Replace("%3", thumbTargetPath);
                p.StartInfo.Arguments = s;
                p.Start();
                p.WaitForExit(1300);
                p.Close();
            }
            catch { return false; }
            return true;
        }//在指定的thumbTargetPath路劲下为路劲为sourcePath的视频创建width*height大小的在sleep秒处的截屏缩略图
        public static void EnsureThumb(VideoInfo pi)
        {
            if (pi.ThumbImage == null)
            {
                if (File.Exists(pi.ThumbPath))
                {
                    pi.ThumbImage = CreatePicture(pi.ThumbPath);
                    if (pi.ThumbImage != null)
                    { pi.IsLoadSuccessful = true; }
                    else
                    { pi.IsLoadSuccessful = false; }
                }
                else
                {
                    if (CreateThumb(pi))
                    {
                        pi.ThumbImage = CreatePicture(pi.ThumbPath);
                        if (pi.ThumbImage != null)
                        { pi.IsLoadSuccessful = true; }
                        else
                        { pi.IsLoadSuccessful = false; }
                    }
                    else
                    {
                        pi.ThumbImage = null;
                        pi.IsLoadSuccessful = false; 
                    }
                }
            }
            
            if (pi.ThumbImage != null && pi.ThumbImage.CanFreeze && !pi.ThumbImage.IsFrozen)
            {
                pi.ThumbImage.Freeze();
            }
        }//检测对象是否有缩略图 若没有则创建，根据创建结果设置标志位 并冻结bitmap对象（否则不可跨线程访问）
    }//该对象一个对应一个视频文件信息
}
