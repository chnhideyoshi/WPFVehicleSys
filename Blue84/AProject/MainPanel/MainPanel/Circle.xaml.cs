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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace MainPanel
{
	/// <summary>
	/// Interaction logic for Circle.xaml
	/// </summary>
	public partial class Circle : UserControl
	{
		public Circle()
		{
			this.InitializeComponent();
            Init();
            InitEventHandler();
		}
        #region 初始化变量
        private void Init()
        {
            buttonCollection.Add(BTN_Book);
            buttonCollection.Add(BTN_FM);
            buttonCollection.Add(BTN_Help);
            buttonCollection.Add(BTN_GPS);
            buttonCollection.Add(BTN_Setting);
            buttonCollection.Add(BTN_Music);
            buttonCollection.Add(BTN_Picture);
            buttonCollection.Add(BTN_Tool);
            buttonCollection.Add(BTN_Video);
            for (int i = 0; i < buttonCollection.Count; i++)
            {
                buttonCollection[i].Tag = i;
            }
            STB_NewProcess = this.FindResource("STB_NewProcess") as Storyboard;
            centerPoint= new Point(0.5 * EP_Base.Width + Canvas.GetLeft(EP_Base), 0.5 * EP_Base.Height + Canvas.GetTop(EP_Base));
        }
        private void InitEventHandler()
        {
            #region
            this.BTN_Book.PreviewMouseDown += (sender, e) => { ButtonDown("Book"); };
            this.BTN_FM.PreviewMouseDown += (sender, e) => { ButtonDown("FM"); };
            this.BTN_Help.PreviewMouseDown += (sender, e) => { ButtonDown("Help"); };
            this.BTN_GPS.PreviewMouseDown += (sender, e) => { ButtonDown("GPS"); };
            this.BTN_Setting.PreviewMouseDown += (sender, e) => { ButtonDown("Setting"); };
            this.BTN_Music.PreviewMouseDown += (sender, e) => { ButtonDown("Music"); };
            this.BTN_Picture.PreviewMouseDown += (sender, e) => { ButtonDown("Picture"); };
            this.BTN_Tool.PreviewMouseDown += (sender, e) => { ButtonDown("Tool"); };
            this.BTN_Video.PreviewMouseDown += (sender, e) => { ButtonDown("Video"); };
            this.BTN_Book.PreviewMouseDown += (sender, e) => { ButtonDown("Book"); };
            #endregion
            #region
            this.BTN_Book.PreviewMouseUp += ButtonUp;
            this.BTN_FM.PreviewMouseUp += ButtonUp;
            this.BTN_Help.PreviewMouseUp += ButtonUp;
            this.BTN_GPS.PreviewMouseUp += ButtonUp;
            this.BTN_Setting.PreviewMouseUp += ButtonUp;
            this.BTN_Music.PreviewMouseUp += ButtonUp;
            this.BTN_Picture.PreviewMouseUp += ButtonUp;
            this.BTN_Tool.PreviewMouseUp += ButtonUp;
            this.BTN_Video.PreviewMouseUp += ButtonUp;
            #endregion
            STB_NewProcess.Completed += (sender, e) =>
            {
                if (ButtonClick != null)
                {
                    ButtonClick(moduleNameWaitForStart);
                }
                isInAnime = false;
            };
        }
        #endregion
        #region 判断点击按钮并且引发事件的逻辑
        private Storyboard STB_NewProcess;//当点击按钮时启动的动画，动画结束后再启动进程  定义在xaml中
        private double? recordPos;//用来记录鼠标按下的Start值，用于和松开的值进行比较吗，再判断是否处在在转动状态
        public double Bound = 1;//如果按下和松开时的star差值超过此值，则处于转动状态
        private void ButtonDown(string moduleName)
        {
            recordPos = this.Start;
            moduleNameWaitForStart = moduleName;
        }//按下时记录点击的按钮对应的模块名和此时的角度start
        private void ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (recordPos.HasValue && Math.Abs(this.Start - recordPos.Value) <= Bound)
            {
                BeginSTB(moduleNameWaitForStart);
            }
        }//如果按下时的角度和松开时动的角度差别小于Bound 则用户动作为点击此按钮
        private bool isInAnime = false;//判断是否在动画中的标记
        private string moduleNameWaitForStart;//标记准备启动的模块名
        private void SetSTB(Storyboard storyboard, string buttonName)
        {
            Button targetButton = this.FindName("BTN_"+buttonName) as Button;
            DoubleAnimationUsingKeyFrames ani_scX = storyboard.Children[0] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames ani_scY = storyboard.Children[1] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames ani_opa = storyboard.Children[2] as DoubleAnimationUsingKeyFrames;
            Storyboard.SetTarget(ani_scX, targetButton);
            Storyboard.SetTarget(ani_scY, targetButton);
            Storyboard.SetTarget(ani_opa, targetButton);
        }//设定动画作用的Button
        private void BeginSTB(string moduleName)
        {
            if (!isInAnime)
            {
                isInAnime = true;
                SetSTB(STB_NewProcess,moduleName);
                STB_NewProcess.Begin();
                moduleNameWaitForStart = moduleName;
            }
        }//开始动画播放
        #region events sent to parent control
        public delegate void ButtonClickEventHandler(string waitForStart);
        public event ButtonClickEventHandler ButtonClick;
        #endregion
        #endregion
        #region 和椭圆布局相关的逻辑
        #region private area
        List<Button> buttonCollection = new List<Button>();
        Point centerPoint=new Point(0,0);
        private void SetButtonPosition(FrameworkElement target,double X, double Y)
        {
            double realx = CenterPoint.X + X;
            double realy = CenterPoint.Y - Y;
            Canvas.SetTop(target, realy-0.5*target.Height);
            Canvas.SetLeft(target, realx - 0.5 * target.Width);
        }//将taget的中心点设置到位置为X，Y的点上
        private void ReLayoutButtons()
        {
            for (int i = 0; i < buttonCollection.Count; i++)
            {
                double initValue = i * (2*Math.PI / buttonCollection.Count);
                double deltaValue = Start/180*Math.PI;
                SetPositionByTheta(buttonCollection[i],initValue+deltaValue);
            }
        }//重新按Start布局所有的button
        private void SetPositionByTheta(FrameworkElement target,double theta)
        {
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            double R = Math.Sqrt(1 / ((cos * cos) / (RadiusX * RadiusX) + (sin * sin) / (RadiusY * RadiusY)));
            double x = R * cos;
            double y = R * sin;
            SetButtonPosition(target, x, y);
        }//按theta角去设置target的位置
        #endregion
        #region public property
        public Point CenterPoint
        {
            get 
            {
                centerPoint.X = 0.5 * EP_Base.Width + Canvas.GetLeft(EP_Base);
                centerPoint.Y = 0.5 * EP_Base.Height + Canvas.GetTop(EP_Base);
                return centerPoint;
            }
        }
        public double RadiusX
        {
            get { return EP_Base.Width / 2; }
        }
        public double RadiusY
        {
            get { return EP_Base.Height / 2; }
        }
        #region StartProperty
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register(
            "Start", typeof(double), typeof(Circle),
            new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(StartPropertyChangedCallback)));
        private static void StartPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            Circle c = sender as Circle;
            c.ReLayoutButtons();
        }
        public double Start
        {
            get
            {
                return (double)this.GetValue(StartProperty);
            }
            set
            {
                this.SetValue(StartProperty, value);
            }
        }
        #endregion
        #region EllipseLeftProperty
        public static readonly DependencyProperty EllipseLeftProperty = DependencyProperty.Register(
            "EllipseLeft", typeof(double), typeof(Circle),
            new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(EllipseLeftPropertyChangedCallback)));
        private static void EllipseLeftPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            Circle c = sender as Circle;
            Canvas.SetLeft(c.EP_Base, c.EllipseLeft);
            c.ReLayoutButtons();
        }
        public double EllipseLeft
        {
            get
            {
                return (double)this.GetValue(EllipseLeftProperty);
            }
            set
            {
                this.SetValue(EllipseLeftProperty, value);
            }
        }
        #endregion
        #region EllipseTopProperty
        public static readonly DependencyProperty EllipseTopProperty = DependencyProperty.Register(
            "EllipseTop", typeof(double), typeof(Circle),
            new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(EllipseTopPropertyChangedCallback)));
        private static void EllipseTopPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            Circle c = sender as Circle;
            Canvas.SetTop(c.EP_Base, c.EllipseTop);
            c.ReLayoutButtons();
        }
        public double EllipseTop
        {
            get
            {
                return (double)this.GetValue(EllipseTopProperty);
            }
            set
            {
                this.SetValue(EllipseTopProperty, value);
            }
        }
        #endregion
        #region EllipseWidthProperty
        public static readonly DependencyProperty EllipseWidthProperty = DependencyProperty.Register(
            "EllipseWidth", typeof(double), typeof(Circle),
            new FrameworkPropertyMetadata(640.0, new PropertyChangedCallback(EllipseWidthPropertyChangedCallback)));
        private static void EllipseWidthPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            Circle c = sender as Circle;
            c.EP_Base.Width = c.EllipseWidth;
            c.ReLayoutButtons();
        }
        public double EllipseWidth
        {
            get
            {
                return (double)this.GetValue(EllipseWidthProperty);
            }
            set
            {
                this.SetValue(EllipseWidthProperty, value);
            }
        }
        #endregion
        #region EllipseHeightProperty
        public static readonly DependencyProperty EllipseHeightProperty = DependencyProperty.Register(
            "EllipseHeight", typeof(double), typeof(Circle),
            new FrameworkPropertyMetadata(640.0, new PropertyChangedCallback(EllipseHeightPropertyChangedCallback)));
        private static void EllipseHeightPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            Circle c = sender as Circle;
            c.EP_Base.Height = c.EllipseHeight;
            c.ReLayoutButtons();
        }
        public double EllipseHeight
        {
            get
            {
                return (double)this.GetValue(EllipseHeightProperty);
            }
            set
            {
                this.SetValue(EllipseHeightProperty, value);
            }
        }
        #endregion
        #endregion
        #endregion


    }
}