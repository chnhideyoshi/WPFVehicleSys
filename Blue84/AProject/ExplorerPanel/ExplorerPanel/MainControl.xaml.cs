using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
namespace ExplorerPanel
{
	/// <summary>
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl : UserControl
	{
		public MainControl()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化
        private void Init()
        {
            touchClass.InitTouchListOperation(CA_DownPanel, STK_MainContainer);
        }
        private void InitEventHandler()
        {
            #region 滑竿操作
            this.CA_ExpSL.PreviewMouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double rate = e.GetPosition(CA_ExpSL).Y / CA_ExpSL.ActualHeight;
                    SL_Down.Value = rate * SL_Down.Maximum;
                    if (touchClass.MaxTop >= 0)
                    {
                        touchClass.Y = -(touchClass.MaxTop) * rate;
                    }
                }
            };
            touchClass.EndOperation += () =>
            {
                if (touchClass.MaxTop > 0)
                {
                    SL_Down.Value = SL_Down.Maximum * (-touchClass.Y / touchClass.MaxTop);
                }
            };
            touchClass.ClearFlag += () =>
            {
                if (readyItem != null)
                {
                    readyItem.ClearFlag();
                    readyItem = null;
                }
            };
            #endregion      
            #region 控件 事件
            UC_LeftStack.DirButtonClicked += new LeftStack.DirButtonClickEventHandler(UC_LeftStack_DirButtonClicked);
            UC_LeftStack.RootButtonClicked += () =>
            {
                UnSelectAll();
                InitDriverView();
            };
            BTN_Cancel.Click += (sender, e) =>
            {
                App.Current.Shutdown();
            };
            BTN_OK.Click += (sender, e) =>
            {
                ShowItemWindowToImport();
            };
            BTN_Include.Click += (sender, e) => 
            {
                ShowItemWindowToInclude();
            };
            BTN_Delete.Click += (sender, e) =>
            {
                Delete();
            };
            BTN_SelectAll.Click += (sender, e) =>
            {
                SelectAll();
            };
            this.BTN_CancelSelectAll.Click += (sender, e) =>
            {
                UnSelectAll();
            };
            #endregion‘
            this.Loaded += (sender, e) =>
            {
                InitDriverView();
            };
        }
        #endregion
        #region 删除功能
        private void Delete()
        {
            CaculateNumber();
            if (App.ViewModel.TotalCount != 0)
            {
                Pop_OkCancel pop = new Pop_OkCancel();
                if (pop.ShowDialog("确定删除？") == true)
                {
                    ProgressWindow window = new ProgressWindow();
                    window.IsCopy = false;
                    window.BeginWork();
                    window.ShowDialog();
                    RefreshCurrentDirectory();
                }
            }
            else
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("请选择项");
            }
        }
        #endregion
        #region 包含文件夹功能
        string bookConfigPath ="Book/BookConfig.xml";
        string videoConfigPath = "Video/VideoConfig.xml";
        //string musicConfigPath = "Music/MusicConfig.xml";
        string pictureConfigPath = "Picture/PictureConfig.xml";
        private void ShowItemWindowToInclude()
        {
            ItemWindow window = new ItemWindow();
            window.IsIncludeMode = true;
            window.ImportMusic += new ItemWindow.ImportMusicEventHandler(ItemWindow_IncludeMusic);
            window.ImportImage += new ItemWindow.ImportImageEventHandler(ItemWindow_IncludeImage);
            window.ImportVideo += new ItemWindow.ImportVideoEventHandler(ItemWindow_IncludeVideo);
            window.ImportBook += new ItemWindow.ImportBookEventHandler(ItemWindow_IncludeBook);
            window.ShowDialog();
        }
        private void AddValidExtDirectoires(string xmlFilePath,List<string> directoryPathCollection)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(xmlFilePath);
            }
            catch { return; }
            System.Xml.XmlNamespaceManager xnm = new System.Xml.XmlNamespaceManager(doc.NameTable);
            System.Xml.XmlNode node = doc.SelectSingleNode("/Settings/ValidExtDirectoires", xnm);
            for (int i = 0; i < directoryPathCollection.Count; i++)
            {
                System.Xml.XmlElement xe = doc.CreateElement("Directory");
                xe.SetAttribute("value",directoryPathCollection[i]);
                node.AppendChild(xe);
            }
            doc.Save(xmlFilePath);
        }
        private bool ContainsSpecialFolder()
        {
            string p1 = ViewModel.DefaultBookPath;
            string p2 = ViewModel.DefaultMusicPath;
            string p3 = ViewModel.DefaultPicturePath;
            string p4 = ViewModel.DefaultVideoPath;
            if (App.ViewModel.ChoosenDirectoryList.Contains(p1)
                || App.ViewModel.ChoosenDirectoryList.Contains(p2)
                || App.ViewModel.ChoosenDirectoryList.Contains(p3)
                || App.ViewModel.ChoosenDirectoryList.Contains(p4)
                )
                return true;
            return false;
        }
        private void ExecuteInclude(string configPath)
        {
            CaculateNumber();
            if (App.ViewModel.TotalCount == 0)
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("请选中项！");
                return;
            }
            if (App.ViewModel.ChoosenFileList.Count != 0)
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("请只选择目录！");
                return;
            }
            if (ContainsSpecialFolder())
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("包含非法目录！");
                return;
            }
            AddValidExtDirectoires(configPath, App.ViewModel.ChoosenDirectoryList);
        }
        private void ItemWindow_IncludeVideo()
        {
            ExecuteInclude(videoConfigPath);
        }
        private void ItemWindow_IncludeImage()
        {
            ExecuteInclude(pictureConfigPath);
        }
        private void ItemWindow_IncludeMusic()
        {
            PopOk pop = new PopOk();
            pop.ShowDialog("暂不支持！");
            return;
            //ExecuteInclude(musicConfigPath);
        }
        private void ItemWindow_IncludeBook()
        {
            ExecuteInclude(bookConfigPath);
        }
        #endregion
        #region 导入功能
        private string TargetDirectory
        {
            get;
            set;
        }
        private void CaculateNumber()
        {
            App.ViewModel.ChoosenDirectoryList.Clear();
            App.ViewModel.ChoosenFileList.Clear();
            for (int i = 0; i < STK_MainContainer.Children.Count; i++)
            {
                FileItem fi = STK_MainContainer.Children[i] as FileItem;
                if (fi.Visibility == Visibility.Visible && fi.IsChecked == true)
                {
                    if (fi.IsFile)
                    {
                        App.ViewModel.ChoosenFileList.Add(fi.Info);
                    }
                    else
                    {
                        App.ViewModel.ChoosenDirectoryList.Add(fi.Info);
                    }
                }
            }
        }
        private bool IsSpectialFolder()
        {
            return (App.ViewModel.CurrentDirectoryPath == ViewModel.DefaultBookPath)
                || (App.ViewModel.CurrentDirectoryPath == ViewModel.DefaultMusicPath)
                || (App.ViewModel.CurrentDirectoryPath == ViewModel.DefaultPicturePath)
                || (App.ViewModel.CurrentDirectoryPath == ViewModel.DefaultVideoPath);
        }
        private void ShowItemWindowToImport()
        {
            ItemWindow window = new ItemWindow();
            window.ImportMusic += new ItemWindow.ImportMusicEventHandler(ItemWindow_ImportMusic);
            window.ImportImage += new ItemWindow.ImportImageEventHandler(ItemWindow_ImportImage);
            window.ImportVideo += new ItemWindow.ImportVideoEventHandler(ItemWindow_ImportVideo);
            window.ImportBook += new ItemWindow.ImportBookEventHandler(ItemWindow_ImportBook);
            window.ShowDialog();
        } 
        private bool? ShowPopConfirm(string folderName,int count)
        {
            Pop_OkCancel pop = new Pop_OkCancel();
            return pop.ShowDialog("确定将当前"+count+"项复制到"+folderName+"文件夹?");
        }
        private void ExecuteImport(string itemName,FileClass fclass)
        {
            CaculateNumber();
            if (App.ViewModel.TotalCount == 0)
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("请选中项！");
                return;
            }
            if (IsSpectialFolder())
            {
                PopOk pop = new PopOk();
                pop.ShowDialog("此文件夹不需要导入 ! ");
                return;
            }
            if (ShowPopConfirm(itemName, App.ViewModel.TotalCount) == true)
            {
                ProgressWindow pw = new ProgressWindow(true);
                pw.BeginWork(fclass);
                pw.ShowDialog();
            }
        }
        private void ItemWindow_ImportVideo()
        {
            ExecuteImport("视频",FileClass.Video);
        }
        private void ItemWindow_ImportImage()
        {
            ExecuteImport("图片",FileClass.Picture);
        }
        private void ItemWindow_ImportMusic()
        {
            ExecuteImport("音乐",FileClass.Music);
        }
        private void ItemWindow_ImportBook()
        {
            ExecuteImport("图书",FileClass.Book);
        }
        #endregion
        #region 浏览功能
        private string[] FileInfoList;//当前目录的文件的路径集合
        private string[] DirectoryInfoList;//当前目录的子目录的集合
        private void LoadControls()
        {
            touchClass.ReSetState();
            STK_MainContainer.Children.Clear();
            if (DirectoryInfoList != null)
            {
                for (int i = 0; i < DirectoryInfoList.Length; i++)
                {
                    try
                    {
                        if (ViewModel.IsSystemVisible(DirectoryInfoList[i]))
                        {
                            FileItem fi = new FileItem();
                            fi.FileName = ViewModel.GetDirectoryName(DirectoryInfoList[i]);
                            fi.Info = DirectoryInfoList[i];
                            fi.IsFile = false;
                            fi.HeadImage = CreateHeadImage(DirectoryInfoList[i], false);
                            fi.Clicked += new FileItem.ClickEventHandler(Item_Clicked);
                            fi.ItemReady += new FileItem.ItemReadyEventHandler(Item_ItemReady);
                            STK_MainContainer.Children.Add(fi);
                        }
                    }
                    catch { continue; }
                }
            }
            if (FileInfoList != null)
            {
                for (int i = 0; i < FileInfoList.Length; i++)
                {
                    try
                    {
                        if (ViewModel.IsFunctionalVisible(FileInfoList[i]))
                        {
                            FileItem fi = new FileItem();
                            fi.Info = FileInfoList[i];
                            fi.FileName = Path.GetFileName(FileInfoList[i]);
                            fi.HeadImage = CreateHeadImage(FileInfoList[i], true);
                            fi.IsFile = true;
                            fi.Clicked += new FileItem.ClickEventHandler(Item_Clicked);
                            fi.ItemReady += new FileItem.ItemReadyEventHandler(Item_ItemReady);
                            STK_MainContainer.Children.Add(fi);
                        }
                    }
                    catch { continue; }
                }
            }
        }//根据获取的文件集合和目录集合创建控件
        private void InitDriverView()
        {
            touchClass.ReSetState();
            DirectoryInfoList = Environment.GetLogicalDrives();
            UC_LeftStack.Clear();
            STK_MainContainer.Children.Clear();
            for (int i = 0; i < DirectoryInfoList.Length; i++)
            {
                string driverName= "未命名卷";
                try
                {
                    driverName = new System.IO.DriveInfo(DirectoryInfoList[i]).VolumeLabel;
                }
                catch { }
                finally
                {
                    FileItem fi = new FileItem();
                    fi.FileName = driverName + "(" + DirectoryInfoList[i].Replace("\\","") + ")";
                    fi.Info = DirectoryInfoList[i];
                    fi.IsFile = false;
                    fi.CKB_Check.Visibility = Visibility.Hidden;
                    fi.HeadImage = CreateHeadImage(DirectoryInfoList[i], false);
                    fi.Clicked += new FileItem.ClickEventHandler(Item_Clicked);
                    fi.ItemReady += new FileItem.ItemReadyEventHandler(Item_ItemReady);
                    STK_MainContainer.Children.Add(fi);
                }
            }
        }//调用本函数加载驱动器列表界面
        private void EnterDirectory(string info)
        {
            App.ViewModel.CurrentDirectoryPath = info;
            if (RefreshCurrentDirectory())
            {
                UC_LeftStack.Push(info);
            }
            else
            {
                App.ViewModel.CurrentDirectoryPath = Path.GetDirectoryName(info);
            }
        }////获取当前CrentDirectoryPath的文件目录信息 并更改LeftStack
        private bool RefreshCurrentDirectory()
        {
            FileInfoList = App.ViewModel.GetFiles();
            DirectoryInfoList = App.ViewModel.GetDirectories();
            if (FileInfoList == null && DirectoryInfoList == null)
            {
                new PopOk().ShowDialog("无权限访问");
                return false;
            }
            LoadControls();
            return true;
        }//获取当前CrentDirectoryPath的文件目录信息并加载控件 失败则返回false
        private FileClass CreateHeadImage(string path, bool isFile)
        {
            if (!isFile)
            {
                if (path.EndsWith("\\") || path.EndsWith("/"))
                {
                    return FileClass.Driver;
                }
                return FileClass.Folder;
            }
            if (ViewModel.IsImageFileExtension(Path.GetExtension(path)))
            {
                return FileClass.Picture;
            }
            if (ViewModel.IsVideoFileExtension(Path.GetExtension(path)))
            {
                return FileClass.Video;
            }
            if (ViewModel.IsMusicFileExtension(Path.GetExtension(path)))
            {
                return FileClass.Music;
            }
            return FileClass.File;
        }//为FileItem控件制定相应文件的种类图标
        private void UC_LeftStack_DirButtonClicked(object sender, string path)
        {
            UnSelectAll();
            App.ViewModel.CurrentDirectoryPath = path;
            RefreshCurrentDirectory();
        }
        #endregion
        #region 界面功能
        FileItem readyItem = null;
        ManipTranformTest.TouchMouseWithRebound touchClass = new ManipTranformTest.TouchMouseWithRebound();
        private void UnSelectAll()
        {
            for (int i = 0; i < STK_MainContainer.Children.Count; i++)
            {
                FileItem fi = STK_MainContainer.Children[i] as FileItem;
                fi.IsChecked = false;
            }
            BTN_CancelSelectAll.Visibility = Visibility.Collapsed;
            BTN_SelectAll.Visibility = Visibility.Visible;
            App.ViewModel.ChoosenDirectoryList.Clear();
            App.ViewModel.ChoosenFileList.Clear();
        }//将当前目录各项设为被选中
        private void SelectAll()
        {
            for (int i = 0; i < STK_MainContainer.Children.Count; i++)
            {
                FileItem fi = STK_MainContainer.Children[i] as FileItem;
                if (fi.Visibility == Visibility.Visible && fi.CKB_Check.Visibility == Visibility.Visible)
                {
                    fi.IsChecked = true;
                }
            }
            BTN_CancelSelectAll.Visibility = Visibility.Visible;
            BTN_SelectAll.Visibility = Visibility.Collapsed;
        }////将当前目录各项设为未选中
        private void Item_ItemReady(FileItem sender)
        {
            readyItem = sender;
        }//当某FileItem项被按下时记录该项
        private void Item_Clicked(object sender, string info, bool isFile)
        {
            UnSelectAll();
            if (!isFile)
            {
                EnterDirectory(info);
            }
        }//当FileItem鼠标松开时判断标记为引发Click
        #endregion
    }
}