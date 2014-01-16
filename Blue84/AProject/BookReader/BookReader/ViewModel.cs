using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
namespace BookReader
{
    public class ViewModel
    {
        List<string> allBookPaths = new List<string>();
        public List<string> AllBookPaths
        {
            get { return allBookPaths; }
            set { allBookPaths = value; }
        }
        public ViewModel()
        {
            InitWorker();
            GetFilesFromDirectory(App.DefaultPath);
            App app=App.Current as App;
            if (app.ValidExtDirectoires != null && app.ValidExtDirectoires.Count != 0)
            {
                for (int i = 0; i < app.ValidExtDirectoires.Count; i++)
                {
                    GetFilesFromDirectory(app.ValidExtDirectoires[i]);
                }
            }
        }
        private void GetFilesFromDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    string[] allFiles = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < allFiles.Length; i++)
                    {
                        if (BookInfo.IsBookFileExtension(Path.GetExtension(allFiles[i])))
                        {
                            allBookPaths.Add(allFiles[i]);
                        }
                    }
                }
            }
            catch { return; }
        }
        public List<BookInfo> GetAllBooks()
        {
            List<BookInfo> list = new List<BookInfo>();
            for (int i = 0; i < AllBookPaths.Count; i++)
            {
                BookInfo b = new BookInfo(AllBookPaths[i]);
                list.Add(b);
            }
            return list;
        }
        #region 异步获取书内容
        BackgroundWorker contentWorker;
        public void InitWorker()
        {
            contentWorker = new System.ComponentModel.BackgroundWorker() { WorkerReportsProgress=false,WorkerSupportsCancellation=false};
            contentWorker.DoWork += new System.ComponentModel.DoWorkEventHandler((sender, e) =>
            {
                e.Result = BookInfo.ReadFile((e.Argument as BookInfo).AbsolutePath);
            });
            contentWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler((sender, e) =>
            {
                LoadContentCompleted(e.Result as string);
            });
        }
        public void BegionLoadContentWorker(BookInfo bookInfo)
        {
            if (contentWorker != null && !contentWorker.IsBusy)
            {
                contentWorker.RunWorkerAsync(bookInfo);
            }
        }
        public delegate void LoadContentCompletedEventHandler(string content);
        public event LoadContentCompletedEventHandler LoadContentCompleted;
        #endregion
    }
    public class BookInfo
    {
        public BookInfo(string path)
        {
            AbsolutePath = path;
        }
        public string Content = null;
        private string[] sentenses;
        public string[] Sentenses
        {
            get 
            {
                if (sentenses == null&&Content!=null)
                {
                    sentenses = Content.Split('.',',','。','，','"','“','”');
                }
                return sentenses;
            }
        }//句子，将全文按标点符号分成句子分别朗读，朗读时就不会占太多内存
        public string AbsolutePath;
        public string Name 
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(AbsolutePath); }
        }//文件名（无后缀名）
        public static string ReadFile(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string s=sr.ReadToEnd();
                sr.Close();
                fs.Close();
                return s;
            }
            catch { return null; }
        }//读取path下的文件并返回其字符串内容
        public static bool IsBookFileExtension(string p)
        {
            if (p.ToUpper() == ".TXT")
            {
                return true;
            }
            return false;
        }
    }
}
