using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
namespace PictureBrowserPanel
{
    public class PictureInfo
    {
        public static readonly string ThumbSuffix = "_thumb.jpg";//缩略图后缀  a.jpg 缩略图为 a.jpg_thumb.jpg
        public static readonly string SmallVersionSuffix = "_small.jpg";//大缩略图后缀  a.jpg 缩略图为 a.jpg_small.jpg
        public static readonly string ThumbDirectoryName = "thumbs";//缩略图文件夹名
        public static readonly string SmallVersionDirectoryName = "smallversion";//大缩略图文件夹名

        public PictureInfo(string path)
        {
            AbsolutePath = path;
        }

        public BitmapImage Image = null;//大缩略图位图对象
        public BitmapImage ThumbImage = null;//缩略图位图对象
        public string AbsolutePath;//文件绝对路径
        public string Name
        {
            get { return System.IO.Path.GetFileName(AbsolutePath); }
        }//文件名
        public string Extension
        {
            get { return System.IO.Path.GetExtension(AbsolutePath); }
        }//后缀 带.
        public string DirectoryName
        {
            get { return DirectoryPath.Substring(DirectoryPath.LastIndexOf('\\') + 1); }
        }//所在文件夹名
        public string DirectoryPath
        {
            get { return System.IO.Path.GetDirectoryName(AbsolutePath); }
        }//所在文件夹路径
        public string ThumbPath
        {
            get
            {
                string filename = System.IO.Path.GetFileName(AbsolutePath);
                return DirectoryPath + "/" + ThumbDirectoryName + "/" + filename + ThumbSuffix;
            }
        }//缩略图路径
        public string SmallVersionPath
        {
            get
            {
                string filename = System.IO.Path.GetFileName(AbsolutePath);
                return DirectoryPath + "/" + SmallVersionDirectoryName + "/" + filename + SmallVersionSuffix;
            }
        }//大缩略图路径
        
        public static bool IsImageFileExtension(string extension)
        {
            return (App.Current as App).SupportedExtensionList.Contains(extension.ToUpper());
        }//判断是否为支持的图片文件后缀
        public static BitmapImage CreatePicture(string path)
        {
            try
            {
                if (!System.IO.File.Exists(path)) { return null; }
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
        }//从path处创建图片的Bitmap对象
        public static void EnsureThumb(PictureInfo pictureInfo)
        {
            if (pictureInfo.ThumbImage == null)
            {
                if (File.Exists(pictureInfo.ThumbPath))
                {
                    pictureInfo.ThumbImage = CreatePicture(pictureInfo.ThumbPath);
                }
                else
                {
                    if (CreateThumb(pictureInfo))
                    {
                        pictureInfo.ThumbImage = CreatePicture(pictureInfo.ThumbPath);
                    }
                    else
                    {
                        pictureInfo.ThumbImage = null;
                    }
                }
            }
            if (pictureInfo.ThumbImage != null && pictureInfo.ThumbImage.CanFreeze && !pictureInfo.ThumbImage.IsFrozen)
            {
                pictureInfo.ThumbImage.Freeze();
            }
        }//创建pictureInfo对象的缩略图并提取到内存
        public static void CreateSmallVersion(PictureInfo pictureInfo)
        {
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(pictureInfo.SmallVersionPath)))
            {
                try
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pictureInfo.SmallVersionPath));
                    File.SetAttributes(Path.GetDirectoryName(pictureInfo.SmallVersionPath), FileAttributes.Hidden);
                }
                catch { }
            }
            ThumbNailMaker.GetReducedImageFitSize((App.Current as App).SmallWidth, (App.Current as App).SmallHeight, pictureInfo.AbsolutePath, pictureInfo.SmallVersionPath);
        }//创建pictureInfo对象的大缩略图
        internal static bool CreateThumb(PictureInfo pictureInfo)
        {
            if (System.IO.File.Exists(pictureInfo.ThumbPath)) { return true; }
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pictureInfo.ThumbPath)))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pictureInfo.ThumbPath));
                    File.SetAttributes(System.IO.Path.GetDirectoryName(pictureInfo.ThumbPath), System.IO.FileAttributes.Hidden);
                }
                catch { return false; }
            }
            if (ThumbNailMaker.GetReducedImage((App.Current as App).ThumbWidth, (App.Current as App).ThumbHeight, pictureInfo.AbsolutePath, pictureInfo.ThumbPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }//创建缩略图并提取到内存
    }
    public class ViewModel
    {
        public ViewModel()
        {
            InitBackgroundWorker();
            EnsureBackgroundWorkWorking();
        }
        #region private
        List<List<PictureInfo>> allPictureInfoMatrix;
        List<PictureInfo> allPictureInfoList;
        List<string> allDirectoryList = new List<string>();
        private void MergeDirectoris(string path,ref List<string> alldirectories, ref List<List<PictureInfo>> allMatrix)
        {
            #region merge this dir
            List<PictureInfo> topfiles = new List<PictureInfo>();
            string[] alltopfiles = Directory.GetFiles(path, "*");
            for (int j = 0; j < alltopfiles.Length; j++)
            {
                if (PictureInfo.IsImageFileExtension(System.IO.Path.GetExtension(alltopfiles[j])))
                {
                    PictureInfo vi = new PictureInfo(alltopfiles[j]);
                    topfiles.Add(vi);
                }
            }
            if (topfiles.Count != 0)
            {
                alldirectories.Add(path);
                allMatrix.Add(topfiles);
            }
            #endregion
            #region merge child dirs
            string[] allDirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < allDirs.Length; i++)
            {
                if (allDirs[i].Contains(PictureInfo.ThumbDirectoryName) || allDirs[i].Contains(PictureInfo.SmallVersionDirectoryName)) { continue; }
                List<PictureInfo> list = new List<PictureInfo>();
                string[] files = System.IO.Directory.GetFiles(allDirs[i], "*", System.IO.SearchOption.TopDirectoryOnly);
                for (int j = 0; j < files.Length; j++)
                {
                    if (PictureInfo.IsImageFileExtension(System.IO.Path.GetExtension(files[j])))
                    {
                        PictureInfo vi = new PictureInfo(files[j]);
                        list.Add(vi);
                    }
                }
                if (list.Count != 0)
                {
                    alldirectories.Add(allDirs[i]);
                    allMatrix.Add(list);
                }
            }
            #endregion
        }
        #endregion
        public List<List<PictureInfo>> GetAllPictureInfoMatrix()
        {
            if (allPictureInfoMatrix != null) { return allPictureInfoMatrix; }
            else
            {
                allPictureInfoMatrix = new List<List<PictureInfo>>();
                #region 添加MyXXX文件夹的文件
                MergeDirectoris(App.DefaultPath, ref allDirectoryList, ref allPictureInfoMatrix);
                #endregion
                #region 添加附加文件夹的文件
                List<string> mergedDirs = (App.Current as App).ValidExtDirectoires;
                for (int i = 0; i < mergedDirs.Count; i++)
                {
                    MergeDirectoris(mergedDirs[i], ref allDirectoryList, ref allPictureInfoMatrix);
                }
                #endregion
                RunGuardWorker();//clear pictures async
                return allPictureInfoMatrix;
            }
        }//获取按2维数数组组织的PictureInfo
        public List<PictureInfo> GetAllPictureInfoList()
        {
            if (allPictureInfoList != null) { return allPictureInfoList; }
            else
            {
                allPictureInfoList = new List<PictureInfo>();
                List<List<PictureInfo>> matrix = GetAllPictureInfoMatrix();
                for (int i = 0; i < matrix.Count; i++)
                {
                    for (int j = 0; j < matrix[i].Count; j++)
                    {
                        allPictureInfoList.Add(matrix[i][j]);
                    }
                }
                return allPictureInfoList;
            }
        }//获取按1维数数组组织的PictureInfo
        public List<string> GetAllDirectories()
        {
            return allDirectoryList;
        }//获取按二维数数组组织的PictureInfo
        public List<string> AllDirectoryList
        {
            get { return allDirectoryList; }
        }//获取所有的有效的图片目录列表
        #region 后台线程
        bool isCleaned = false;//该值指示guardWorker是否已经运行一次
        BackgroundWorker fileSystemWorker;//文件系统线程 该线程用于异步获取所有图片文件路径
        BackgroundWorker loadDetailWorker;//信息加载线程 该线程用于异步获取所有图片文件的小型缩略图
        BackgroundWorker guardWorker;//守护线程 该后台线程负责在启动后检查，删除多余缩略图
        BackgroundWorker viewWorker;//全屏图片加载线程 异步载入全屏播放大图的线程（载入大型缩略图）
        DispatcherTimer timer_Ensure;//该定时器定时调用方法检查loadDetailWorker线程工作状态（非必须，但可提高可靠性）
        private void EnsureBackgroundWorkWorking()
        {
            if (timer_Ensure == null)
            {
                timer_Ensure = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
                timer_Ensure.Interval = TimeSpan.FromSeconds(1.5);
                timer_Ensure.Tick += (sender, e) =>
                {
                    if (loadDetailWorker != null && DetaiQueue.Count != 0 && !loadDetailWorker.IsBusy)
                    {
                        RunDetailWorker();
                    }
                };
                timer_Ensure.Start();
            }
            else
            {
                timer_Ensure.Start();
            }
        }
        private void InitBackgroundWorker()
        {
            #region 文件系统线程
            fileSystemWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            fileSystemWorker.DoWork += (sender, e) =>
            {
                GetAllPictureInfoMatrix();
            };
            //fileSystemWorker.ProgressChanged += (sender, e) => { };
            fileSystemWorker.RunWorkerCompleted += (sender, e) =>
            {
                if (FileSystemWorkCompleted != null)
                    FileSystemWorkCompleted();
            };
            #endregion
            #region 信息加载线程
            loadDetailWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            loadDetailWorker.DoWork += (sender, e) =>
            {
                while (DetaiQueue.Count != 0)
                {
                    PictureInfo pi;
                    lock (DetaiQueue)
                    {
                        pi = DetaiQueue.Dequeue();
                    }
                    PictureInfo.EnsureThumb(pi);
                    loadDetailWorker.ReportProgress(0, pi);
                }
            };
            loadDetailWorker.ProgressChanged += (sender, e) =>
            {
                if (LoadDetailsProgressChanged != null)
                {
                    LoadDetailsProgressChanged((e.UserState as PictureInfo));
                }
            };
            loadDetailWorker.RunWorkerCompleted += (sender, e) =>
            {
                if (DetailWorkCompleted != null)
                {
                    DetailWorkCompleted();
                }
            };
            //loadDetailWorker.RunWorkerCompleted += (sender, e) => { };
            #endregion
            #region 守护线程
            guardWorker = new BackgroundWorker() { WorkerReportsProgress = false, WorkerSupportsCancellation = true };
            guardWorker.DoWork += (sender, e) =>
            {
                #region ClearPicture
                if (!isCleaned)
                {
                    for (int i = 0; i < allDirectoryList.Count; i++)
                    {
                        try
                        {
                            string thumbpath = allDirectoryList[i] + "\\" + PictureInfo.ThumbDirectoryName;
                            ClearRedundancy(thumbpath, PictureInfo.ThumbSuffix);
                            string smallversionpath = allDirectoryList[i] + "\\" + PictureInfo.SmallVersionDirectoryName;
                            ClearRedundancy(smallversionpath, PictureInfo.SmallVersionSuffix);
                        }
                        catch { continue; }
                    }
                    isCleaned = true;
                }
                #endregion
            };
            #endregion
            #region 全屏图片加载线程
            viewWorker = new BackgroundWorker();
            viewWorker.DoWork += (sender, e) =>
            {
                while (ViewWorkQueue.Count != 0)
                {
                    PictureInfo pi;
                    lock (ViewWorkQueue)
                    {
                        pi = ViewWorkQueue.Dequeue();
                    }
                    if (pi != null)
                    {
                        if (File.Exists(pi.SmallVersionPath))
                        {
                            pi.Image = PictureInfo.CreatePicture(pi.SmallVersionPath);
                            if (pi.Image != null && pi.Image.CanFreeze)
                            {
                                pi.Image.Freeze();
                            }
                        }
                        else
                        {
                            PictureInfo.CreateSmallVersion(pi);
                            pi.Image = PictureInfo.CreatePicture(pi.SmallVersionPath);
                            if (pi.Image != null && pi.Image.CanFreeze)
                            {
                                pi.Image.Freeze();
                            }
                        }
                    }
                }
            };
            viewWorker.RunWorkerCompleted += (sender, e) =>
            {
                if (ViewWorkCompleted != null)
                {
                    ViewWorkCompleted();
                }
            };
            #endregion
        }
        private void ClearRedundancy(string dirPath, string pattern)
        {
            if (!Directory.Exists(dirPath)) { return; }
            string[] files = Directory.GetFiles(dirPath);
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
        }
        #region 事件
        public delegate void FileSystemWorkCompletedEventHandler();
        public event FileSystemWorkCompletedEventHandler FileSystemWorkCompleted;
        public delegate void DetailWorkCompletedEventHandler();
        public event DetailWorkCompletedEventHandler DetailWorkCompleted;
        public delegate void LoadDetailsProgressChangedEventHandler(PictureInfo pi);
        public event LoadDetailsProgressChangedEventHandler LoadDetailsProgressChanged;
        public delegate void ViewWorkCompletedEventHandler();
        public event ViewWorkCompletedEventHandler ViewWorkCompleted;
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
            if (loadDetailWorker != null && !loadDetailWorker.IsBusy&&DetaiQueue.Count!=0)
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
        public void RunViewWorker()
        {
            if (!viewWorker.IsBusy)
            {
                viewWorker.RunWorkerAsync();
            }
        }//启动本线程
        #region 工作队列
        public int DetaiQueueCount
        {
            get { return DetaiQueue.Count; }
        }
        Queue<PictureInfo> DetaiQueue = new Queue<PictureInfo>(20);//任务队列,包含所有没有缩略图的PictureInfo对象
        Queue<PictureInfo> ViewWorkQueue = new Queue<PictureInfo>();//任务队列,包含所有没大缩略图的PictureInfo对象
        public void AddWorkingItem(PictureInfo vi)
        {
            lock (DetaiQueue)
            {
                DetaiQueue.Enqueue(vi);
            }
        }
        public void AddWorkingItem(List<PictureInfo> list, int fromIndex, int toIndex)
        {
            lock (DetaiQueue)
            {
                for (int i = fromIndex; i <= toIndex; i++)
                {
                    DetaiQueue.Enqueue(list[i]);
                }
            }
        }
        public void AddViewWorkItem(PictureInfo pi)
        {
            lock (ViewWorkQueue)
            {
                ViewWorkQueue.Enqueue(pi);
            }
        }
        #endregion
        #endregion
    }
    public static class ThumbNailMaker
    {
        //<summary>
        //生成缩略图重载方法2，将缩略图文件保存到指定的路径
        //</summary>.
        //<param name=”Width”>缩略图的宽度</param>
        //<param name=”Height”>缩略图的高度</param>
        //<param name=”targetFilePath”>缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>
        //<returns>成功返回true，否则返回false</returns>
        public static bool GetReducedImage(int Width, int Height, string sourcePath, string targetFilePath)
        {
            try
            {
                Image ResourceImage = Image.FromFile(sourcePath);
                #region 过滤掉很小不用做缩略图的图片
                if (ResourceImage.Width < Width)
                {
                    ResourceImage.Save(@targetFilePath, ImageFormat.Jpeg);
                    if (System.IO.File.Exists(@targetFilePath))
                    {
                        File.SetAttributes(@targetFilePath, FileAttributes.Hidden);
                    }
                    ResourceImage.Dispose();
                    return true;
                } 
                #endregion
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(() => { return false; });
                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                ResourceImage.Dispose();
                if (System.IO.File.Exists(@targetFilePath))
                {
                    File.SetAttributes(@targetFilePath, FileAttributes.Hidden);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool GetReducedImageFitSize(int maxWidth, int maxHeight, string sourcePath, string targetFilePath)
        {
            try
            {
                Image ResourceImage = Image.FromFile(sourcePath);
                #region 过滤掉很小不用做缩略图的图片
                if (ResourceImage.Width < maxWidth)
                {
                    ResourceImage.Save(@targetFilePath, ImageFormat.Jpeg);
                    if (System.IO.File.Exists(@targetFilePath))
                    {
                        File.SetAttributes(@targetFilePath, FileAttributes.Hidden);
                    }
                    ResourceImage.Dispose();
                    return true;
                }
                #endregion
                int realWidth = maxWidth;
                int realHeight = maxHeight;
                #region 计算按比例缩小后的大小
                double rate = (double)ResourceImage.Width / (double)ResourceImage.Height;
                if (rate > 800.0 / 480.0)
                {
                    realWidth = 800;
                    realHeight = (int)(realWidth / rate);
                }
                else
                {
                    realHeight = 480;
                    realWidth =(int) (realHeight * rate);
                }
                #endregion 
                Bitmap ReducedImage = new Bitmap(ResourceImage, realWidth, realHeight);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                ResourceImage.Dispose();
                if (System.IO.File.Exists(@targetFilePath))
                {
                    File.SetAttributes(@targetFilePath, FileAttributes.Hidden);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }//缩略图制作工具类
}
