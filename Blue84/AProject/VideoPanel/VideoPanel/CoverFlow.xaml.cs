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
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using _3DTools;
using System.Windows.Media.Animation;
using System.IO;

namespace VideoPanel
{
    /// <summary>
    /// Interaction logic for CoverFlow.xaml
    /// </summary>
    public partial class CoverFlow : UserControl
    {
        public CoverFlow()
        {
            this.InitializeComponent();
            InitEventHandler();
            Init();
        }
        public void RefreshDetail(VideoInfo vi)
        {
            try
            {
                for (int i = 0; i < boderlist.Count; i++)
                {
                    if ((boderlist[i].Tag as VideoInfo).AbsolutePath==vi.AbsolutePath && vi.ThumbImage != null)
                    {
                        (boderlist[i].Background as ImageBrush).ImageSource = vi.ThumbImage;
                    }
                }
            }
            catch { return; }

        }

        //////////////////////////
        //////////////////////////
        /////////////////////////
        #region CurrentMidIndexProperty
        public static readonly DependencyProperty CurrentMidIndexProperty = DependencyProperty.Register(
            "CurrentMidIndex", typeof(double), typeof(CoverFlow),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));

        private static void CurrentMidIndexPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlow win = sender as CoverFlow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 获取或设置当前中间项序号
        /// </summary>
        public double CurrentMidIndex
        {
            get
            {
                return (double)this.GetValue(CurrentMidIndexProperty);
            }
            set
            {
                this.SetValue(CurrentMidIndexProperty, value);
            }
        }
        #endregion

        #region ModelAngleProperty
        public static readonly DependencyProperty ModelAngleProperty = DependencyProperty.Register(
            "ModelAngle", typeof(double), typeof(CoverFlow),
            new FrameworkPropertyMetadata(70.0, new PropertyChangedCallback(ModelAnglePropertyChangedCallback)));


        private static void ModelAnglePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlow win = sender as CoverFlow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 获取或设置模型沿Y轴的旋转角度
        /// </summary>
        public double ModelAngle
        {
            get
            {
                return (double)this.GetValue(ModelAngleProperty);
            }
            set
            {
                this.SetValue(ModelAngleProperty, value);
            }
        }
        #endregion

        #region XDistanceBetweenModelsProperty
        public static readonly DependencyProperty XDistanceBetweenModelsProperty = DependencyProperty.Register(
            "XDistanceBetweenModels", typeof(double), typeof(CoverFlow),
            new FrameworkPropertyMetadata(0.5, XDistanceBetweenModelsPropertyChangedCallback));

        private static void XDistanceBetweenModelsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlow win = sender as CoverFlow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 获取或设置X方向上两个模型间的距离
        /// </summary>
        public double XDistanceBetweenModels
        {
            get
            {
                return (double)this.GetValue(XDistanceBetweenModelsProperty);
            }
            set
            {
                this.SetValue(XDistanceBetweenModelsProperty, value);
            }
        }
        #endregion

        #region ZDistanceBetweenModelsProperty
        public static readonly DependencyProperty ZDistanceBetweenModelsProperty = DependencyProperty.Register(
            "ZDistanceBetweenModels", typeof(double), typeof(CoverFlow),
            new FrameworkPropertyMetadata(0.5, ZDistanceBetweenModelsPropertyChangedCallback));

        private static void ZDistanceBetweenModelsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlow win = sender as CoverFlow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 获取或设置Z方向上两个模型间的距离
        /// </summary>
        public double ZDistanceBetweenModels
        {
            get
            {
                return (double)this.GetValue(ZDistanceBetweenModelsProperty);
            }
            set
            {
                this.SetValue(ZDistanceBetweenModelsProperty, value);
            }
        }
        #endregion


        #region MidModelDistanceProperty
        public static readonly DependencyProperty MidModelDistanceProperty = DependencyProperty.Register(
            "MidModelDistance", typeof(double), typeof(CoverFlow),
            new FrameworkPropertyMetadata(1.5, MidModelDistancePropertyChangedCallback));

        private static void MidModelDistancePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlow win = sender as CoverFlow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 获取或设置中间的模型距离两边模型的距离
        /// </summary>
        public double MidModelDistance
        {
            get
            {
                return (double)this.GetValue(MidModelDistanceProperty);
            }
            set
            {
                this.SetValue(MidModelDistanceProperty, value);
            }
        }
        #endregion
        private List<VideoInfo> pictureInfoList;
        public List<VideoInfo> PictureInfoList
        {
            get { return pictureInfoList; }
            set
            {
                pictureInfoList = value;
                LoadData();
            }
        }
        private void LoadData()
        {
            for (int i = 0; i < items.Count; i++)
            {
                viewport3D.Children.Remove(items[i]);
            }
            items.Clear();
            boderlist.Clear();
            this.LoadImageToViewport3D(PictureInfoList);
            App.ViewModel.RunDetailWorker();
            CurrentMidIndex = 2;
        }
        private void InitEventHandler()
        {
            this.MouseDown += (sender, e) =>
            {
                if (e.GetPosition(this).X>500)
                {
                    if(CurrentMidIndex<PictureInfoList.Count-1)
                        this.CurrentMidIndex++;
                }
                else if (e.GetPosition(this).X < 300)
                {
                    if (CurrentMidIndex >0)
                      this.CurrentMidIndex--;
                }
                if (IndexChanged != null)
                {
                    IndexChanged(pictureInfoList, (int)CurrentMidIndex);
                }
            };
            App.ViewModel.LoadDetailsProgressChanged += (pi) => 
            {
                RefreshDetail(pi);
            };
            App.ViewModel.DetailWorkCompleted += () => 
            {
                this.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Visible;
            };
        }



        /// <summary>
        /// 添加图片到视口
        /// </summary>
        /// <param name="images"></param>
        private void LoadImageToViewport3D(List<VideoInfo> images)
        {
            if (images == null)
            {
                return;
            }

            for (int i = 0; i < images.Count; i++)
            {
                //string imageFile = images[i].ThumbPath;

                InteractiveVisual3D iv3d = this.CreateInteractiveVisual3D(images[i], i);
                items.Add(iv3d);
                this.viewport3D.Children.Add(iv3d);
            }

            this.ReLayoutInteractiveVisual3D();
        }
        List<InteractiveVisual3D> items = new List<InteractiveVisual3D>();
        /// <summary>
        /// 获取当前用户的图片文件夹中的图片(不包含子文件夹)
        /// </summary>
        /// <returns>返回图片路径列表</returns>
      
        public Point3D CarmeraPosition
        {
            get { return this.Camera.Position; }
            set { this.Camera.Position = value; }
        }
        /// <summary>
        ///  由指定的图片路径创建一个可视对象
        /// </summary>
        /// <param name="imageFile">图片路径</param>
        /// <returns>创建的可视对象</returns>
        /// 
        List<Control> boderlist = new List<Control>();
        private Visual CreateVisual(VideoInfo image, int index)
        {
            BitmapImage bmp = null;
            try
            {
                if (image.ThumbImage == null)
                {
                    App.ViewModel.AddWorkingItem(image);
                }
                else
                {
                    bmp = image.ThumbImage;
                }
            }
            catch
            {
                return null;
            }
            if (bmp == null)
            {
                bmp = this.FindResource("BI_Image") as BitmapImage;
            }
            Control outBorder = new Control();
            outBorder.Tag = image;
            outBorder.Style = this.FindResource("ControlStyle1") as Style;
            outBorder.Background = new ImageBrush(bmp);
            outBorder.Width = 120;
            boderlist.Add(outBorder);
            outBorder.MouseDown += delegate(object sender, MouseButtonEventArgs e)
            {
                int indexs = boderlist.IndexOf(sender as Control);
                if (indexs != -1 && ImageButtonClicked != null)
                {
                     ImageButtonClicked(sender, pictureInfoList[indexs], pictureInfoList);
                }
            };
            return outBorder;
        }
        public delegate void ImageButtonClickEventHandler(object sender, VideoInfo p, List<VideoInfo> list);
        public event ImageButtonClickEventHandler ImageButtonClicked;
        public delegate void IndexChangedEventHandler(List<VideoInfo> list, int newIndex);
        public event IndexChangedEventHandler IndexChanged;



        /// <summary>
        /// 创建3D图形
        /// </summary>
        /// <returns>创建的3D图形</returns>
        /// 
        double scanX = 2.2;
        double scanY = 4;
        public double ScanX
        {
            get { return scanX; }
            set { scanX = value; }
        }
        public double ScanY
        {
            get { return scanY; }
            set { scanY = value; }
        }

        private Geometry3D CreateGeometry3D()
        {
            MeshGeometry3D geometry = new MeshGeometry3D();

            geometry.Positions = new Point3DCollection();
            Point3D p1 = new Point3D(-0.5 * ScanX, 0.5 * ScanY, 0);
            Point3D p2 = new Point3D(-0.5 * ScanX, -0.5 * ScanY, 0);
            Point3D p3 = new Point3D(0.5 * ScanX, -0.5 * ScanY, 0);
            Point3D p4 = new Point3D(0.5 * ScanX, 0.5 * ScanY, 0);
            geometry.Positions.Add(p1);
            geometry.Positions.Add(p2);
            geometry.Positions.Add(p3);
            geometry.Positions.Add(p4);
         

            geometry.TriangleIndices = new Int32Collection();
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(1);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(3);

            geometry.TextureCoordinates = new PointCollection();
            geometry.TextureCoordinates.Add(new Point(0, 0));
            geometry.TextureCoordinates.Add(new Point(0, 1));
            geometry.TextureCoordinates.Add(new Point(1, 1));
            geometry.TextureCoordinates.Add(new Point(1, 0));

            return geometry;
        }

        /// <summary>
        /// 为指定图片路径创建一个3D视觉对象
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        private InteractiveVisual3D CreateInteractiveVisual3D(VideoInfo image, int index)
        {
            InteractiveVisual3D iv3d = new InteractiveVisual3D();
            iv3d.Visual = this.CreateVisual(image, index);
            iv3d.Geometry = this.CreateGeometry3D();
            iv3d.Transform = this.CreateEmptyTransform3DGroup();

            return iv3d;
        }

        /// <summary>
        /// 创建一个空的Transform3DGroup
        /// </summary>
        /// <returns></returns>
        private Transform3DGroup CreateEmptyTransform3DGroup()
        {
            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0)));
            group.Children.Add(new TranslateTransform3D(new Vector3D()));
            group.Children.Add(new ScaleTransform3D());

            return group;
        }
        /// <summary>
        /// 依照InteractiveVisual3D在列表中的序号来变换其位置等
        /// </summary>
        /// <param name="index">在列表中的序号</param>
        /// <param name="midIndex">列表中被作为中间项的序号</param>
        private void GetTransformOfInteractiveVisual3D(int index, double midIndex, out double angle, out double offsetX, out double offsetZ)
        {
            double disToMidIndex = index - midIndex;


            //旋转,两翼的图片各旋转一定的度数
            angle = 0;
            if (disToMidIndex < 0)
            {
                angle = this.ModelAngle;//左边的旋转N度
            }
            else if (disToMidIndex > 0)
            {
                angle = (-this.ModelAngle);//右边的旋转-N度
            }



            //平移,两翼的图片逐渐向X轴负和正两个方向展开
            offsetX = 0;//中间的不平移
            if (Math.Abs(disToMidIndex) <= 1)
            {
                offsetX = disToMidIndex * this.MidModelDistance;
            }
            else if (disToMidIndex != 0)
            {
                offsetX = disToMidIndex * this.XDistanceBetweenModels + (disToMidIndex > 0 ? this.MidModelDistance : -this.MidModelDistance);
            }


            //两翼的图片逐渐向Z轴负方向移动一点,造成中间突出(离观众较近的效果)
            offsetZ = Math.Abs(disToMidIndex) * -this.ZDistanceBetweenModels;

        }

        /// <summary>
        /// 重新布局3D内容
        /// </summary>
        PropertyPath p1 = new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[0].(RotateTransform3D.Rotation).(AxisAngleRotation3D.Angle)");
        PropertyPath p2 = new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[1].(TranslateTransform3D.OffsetX)");
        PropertyPath p3 = new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[1].(TranslateTransform3D.OffsetZ)");
        DoubleAnimation angleAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
        DoubleAnimation xAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
        DoubleAnimation zAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
        Storyboard story = new Storyboard();
        private void Init()
        {
            Storyboard.SetTargetProperty(
                       angleAnimation,
                       p1);
            Storyboard.SetTargetProperty(
                xAnimation,
               p2);
            Storyboard.SetTargetProperty(
                zAnimation,
               p3);
        }
        private void ReLayoutInteractiveVisual3D()
        {
            int j = 0;
            for (int i = 0; i < this.viewport3D.Children.Count; i++)
            {
                InteractiveVisual3D iv3d = this.viewport3D.Children[i] as InteractiveVisual3D;
                if (iv3d != null)
                {
                    double angle = 0;
                    double offsetX = 0;
                    double offsetZ = 0;
                    this.GetTransformOfInteractiveVisual3D(j++, this.CurrentMidIndex, out angle, out offsetX, out offsetZ);
                    // NameScope.SetNameScope(this, new NameScope());
                    // this.RegisterName("iv3d", iv3d);
                    //DoubleAnimation angleAnimation = new DoubleAnimation(angle, time);
                    angleAnimation.To = angle;
                    xAnimation.To = offsetX;
                    zAnimation.To = offsetZ;
                    //DoubleAnimation xAnimation = new DoubleAnimation(offsetX, time);
                    //DoubleAnimation zAnimation = new DoubleAnimation(offsetZ, time);
                    //Storyboard story = new Storyboard();
                    if (story.Children.Count == 0)
                    {
                        story.Children.Add(angleAnimation);
                        story.Children.Add(xAnimation);
                        story.Children.Add(zAnimation);
                    }
                    //Storyboard.SetTargetName(angleAnimation, "iv3d");
                    //Storyboard.SetTargetName(xAnimation, "iv3d");
                    //Storyboard.SetTargetName(zAnimation, "iv3d");
                    Storyboard.SetTarget(angleAnimation, iv3d);
                    Storyboard.SetTarget(xAnimation, iv3d);
                    Storyboard.SetTarget(zAnimation, iv3d);
                    //Storyboard.SetTargetProperty(
                    //    angleAnimation,
                    //    p1);
                    //Storyboard.SetTargetProperty(
                    //    xAnimation,
                    //   p2);
                    //Storyboard.SetTargetProperty(
                    //    zAnimation,
                    //   p3);
                    story.Begin(this);
                }
            }
        }
        ///////////////


       
        //bool isdesign = false;
        //public bool IsDesignTime
        //{
        //    get
        //    {
        //        return isdesign;
        //    }
        //    set
        //    {
        //        isdesign = value;
        //        if (value == true)
        //        {
        //            LoadTestData();
        //        }
        //    }
        //}
        //private void LoadTestData()
        //{
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        //    string[] Files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        //    pictureInfoList = new List<VideoInfo>();
        //    for(int i=0;i<Files.Length;i++)
        //    {
        //        VideoInfo vi = new VideoInfo("");
        //        vi.ThumbImage =VideoInfo.CreatePicture(Files[i]);
        //        pictureInfoList.Add(vi);
        //    }
        //    LoadData();
        //}


        //#region endregion
        //public void InitHook()
        //{
        //    System.Windows.Interop.HwndSource souce =
        //        PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
        //    if (souce != null)
        //    {
        //        souce.AddHook(new System.Windows.Interop.HwndSourceHook(WndProc));
        //    }
        //    else
        //    {
        //        MessageBox.Show("fail");
        //    }
        //}
        //IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{

        //    if (msg == 0x0100)
        //    {
        //        //MessageBox.Show("asdas");
        //        //if (wParam.ToInt32() == 65)
        //        //{
        //        //    MessageBox.Show("51");
        //        //}

        //    }
        //    return hwnd;
        //}
        //#endregion
    }
}