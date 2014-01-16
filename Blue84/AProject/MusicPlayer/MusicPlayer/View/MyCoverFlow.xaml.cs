using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using MusicPlayer.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using MusicPlayer.ViewModel;
namespace MusicPlayer.View
{
    /// <summary>
    /// Interaction logic for MyCoverFlow.xaml
    /// </summary>
    public partial class MyCoverFlow : UserControl
    {

        #region DP


        public string CurrentTitle
        {
            get { return (string)GetValue(CurrentTitleProperty); }
            set { SetValue(CurrentTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTitleProperty =
            DependencyProperty.Register("CurrentTitle", typeof(string), typeof(MyCoverFlow));



        public ObservableCollection<AlbumInfo> Albums
        {
            get { return (ObservableCollection<AlbumInfo>)GetValue(AlbumsProperty); }
            set { SetValue(AlbumsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Albums.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register("Albums", typeof(ObservableCollection<AlbumInfo>), typeof(MyCoverFlow));

        DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(
    MyCoverFlow.AlbumsProperty,
    typeof(MyCoverFlow));
        private void OnMyDependencyPropertyChanged(object sender, EventArgs e)
        {
            FillImageByStartIndexAndRange();
            if (startIndex < Albums.Count)
                CurrentTitle = Albums[(int)StartIndex].Name;
        }
        #endregion // DP

        public MyCoverFlow()
        {
            this.InitializeComponent();
            Init();
            prop.AddValueChanged(this, this.OnMyDependencyPropertyChanged);
            InitEventHandler();
        }

        private void InitEventHandler()
        {
            this.MouseDown += (sender, e) =>
            {
                if (isInAnime) { return; }
                if (e.GetPosition(this).X > 500)
                {
                    if (StartIndex < Albums.Count - 1)
                    {
                        isInAnime = true;
                        STB_SwitchLeft.Begin();
                    }
                }
                else if (e.GetPosition(this).X < 300)
                {
                    if (StartIndex > 0)
                    {
                        isInAnime = true;
                        STB_SwitchRight.Begin();
                    }
                }

            };
            for (int i = 0; i < controlList.Count; i++)
            {
                controlList[i].MouseDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    int indexs = controlList.IndexOf(sender as Control);
                    if (indexs != -1 && ImageButtonClicked != null)
                    {
                        ImageButtonClicked(sender, Albums[StartIndex], pictureInfoList);
                    }
                };
            }
            STB_SwitchLeft.Completed += STB_SwitchLeft_Completed;
            STB_SwitchRight.Completed += new EventHandler(STB_SwitchRight_Completed);
            //App.ViewModel.LoadDetailsProgressChanged += (vi) => { RefreshItem(vi); };
            //App.ViewModel.DetailWorkCompleted += () =>
            //{
            //    this.Visibility = Visibility.Collapsed;
            //    this.Visibility = Visibility.Visible;
            //};
        }


        void STB_SwitchLeft_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex++;
            if (IndexChanged != null)
            {
                IndexChanged(pictureInfoList, StartIndex);
            }
        }
        void STB_SwitchRight_Completed(object sender, EventArgs e)
        {
            isInAnime = false;
            StartIndex--;
            if (IndexChanged != null)
            {
                IndexChanged(pictureInfoList, StartIndex);
            }
        }

        #region var
        bool isInAnime = false;
        Storyboard STB_SwitchLeft;
        Storyboard STB_SwitchRight;
        private List<AlbumInfo> pictureInfoList;
        private int startIndex = 5;
        private readonly int range = 11;
        private List<Control> controlList = new List<Control>(11);
        public delegate void ImageButtonClickEventHandler(object sender, AlbumInfo p, List<AlbumInfo> list);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        public delegate void IndexChangedEventHandler(List<AlbumInfo> list, int newIndex);
        public event IndexChangedEventHandler IndexChanged;
        #endregion
        private void Init()
        {
            STB_SwitchLeft = this.FindResource("STB_SwitchLeft") as Storyboard;
            STB_SwitchRight = this.FindResource("STB_SwitchRight") as Storyboard;
            #region control
            controlList.Add(CT_L5);
            controlList.Add(CT_L4);
            controlList.Add(CT_L3);
            controlList.Add(CT_L2);
            controlList.Add(CT_L1);
            controlList.Add(CT_C);
            controlList.Add(CT_R1);
            controlList.Add(CT_R2);
            controlList.Add(CT_R3);
            controlList.Add(CT_R4);
            controlList.Add(CT_R5);
            for (int i = 0; i < controlList.Count; i++)
                controlList[i].Background = new ImageBrush();
            #endregion
            ImageButtonClicked += (sender, p, list) =>
            {
                MusicPlayMessage msg = new MusicPlayMessage() { MusicList = p.AllMusic, SelectedMusic = null };
                Messenger.Default.Send<MusicPlayMessage, PlayingViewModel>(msg);
                Messenger.Default.Send<bool, MainViewModel>(false);
            };
            IndexChanged += (sender, e) =>
                {
                    CurrentTitle = Albums[(int)StartIndex].Name;
                };
        }

        public List<AlbumInfo> PictureInfoList
        {
            get { return pictureInfoList; }
            set
            {
                pictureInfoList = value;
                if (pictureInfoList.Count >= 11)
                {
                    StartIndex = 5;
                }
                else
                {
                    StartIndex = PictureInfoList.Count / 2;
                }
            }
        }
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
            set
            {
                if (value >= 0 && value <= Albums.Count - 1)
                {
                    startIndex = value;
                    FillImageByStartIndexAndRange();
                }
            }
        }

        private void FillImageByStartIndexAndRange()
        {
            for (int i = -range / 2; i <= range / 2; i++)
            {
                if (i + StartIndex >= 0 && i + StartIndex <= Albums.Count - 1)
                {
                    if (Albums[i + StartIndex].CoverImage == null)
                    {
                        (controlList[range / 2 + i].Background as ImageBrush).ImageSource = this.FindResource("BI_DefaultBack") as BitmapImage;
                    }
                    else
                    {
                        (controlList[range / 2 + i].Background as ImageBrush).ImageSource = Albums[i + StartIndex].CoverImage;
                    }
                    controlList[range / 2 + i].Visibility = Visibility.Visible;
                }
                else
                {
                    controlList[range / 2 + i].Visibility = Visibility.Hidden;
                }
            }
            //App.ViewModel.RunDetailWorker();
        }
        //public void RefreshItem(AlbumInfo vi)
        //{
        //    try
        //    {
        //        for (int i = 0; i < controlList.Count; i++)
        //        {
        //            AlbumInfo v=controlList[i].Tag as AlbumInfo;
        //            if (v.AbsolutePath == vi.AbsolutePath)
        //            {
        //                (controlList[i].Background as ImageBrush).ImageSource = vi.ThumbImage;
        //            }
        //        }
        //    }
        //    catch { return; }
        //}
    }
}