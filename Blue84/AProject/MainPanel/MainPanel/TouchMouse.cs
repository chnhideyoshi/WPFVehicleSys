using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using MainPanel;

namespace HorizontalVersion
{
    /// <summary>
    /// 此类用来描述Page控件的分页鼠标触摸操作
    /// </summary>
    public class SegmentTouchMouseWithRebound
    {
        #region public method and attribute
        public void ReSetState()
        {
            if (STK_MainStk_Mouse != null && TTF_transform != null)
            {
                TTF_transform.BeginAnimation(TranslateTransform.YProperty, null);
                TTF_transform.Y = 0;
            }
        }
        public void InitTouchListOperation(Canvas mainCanvas, FrameworkElement mainStackPanel)
        {
            CA_MainCa_Mouse = mainCanvas;
            mainCanvas.ClipToBounds = true;
            STK_MainStk_Mouse = mainStackPanel;
            TTF_transform = new TranslateTransform(0, 0);
            STK_MainStk_Mouse.RenderTransform = TTF_transform;
            this.CA_MainCa_Mouse.PreviewMouseDown += (sender, e) =>
            {
                isdrag_Mouse = true;
                oldOffset_Mouse = e.GetPosition(STK_MainStk_Mouse).X;
                startOffset_Mouse = e.GetPosition(CA_MainCa_Mouse).X;
                //timer_Mouse.Start();
                if (BeginOperation != null) { BeginOperation(); }
            };
            this.CA_MainCa_Mouse.PreviewMouseMove += (sender, e) =>
            {
                if (isdrag_Mouse && e.LeftButton == MouseButtonState.Pressed)
                {
                    if (ClearFlag != null && Math.Abs(e.GetPosition(CA_MainCa_Mouse).X - startOffset_Mouse) > ClearBound)
                    {
                        ClearFlag();
                    }
                    TTF_transform.BeginAnimation(TranslateTransform.XProperty, null);
                    //STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, null);
                    TTF_transform.X = e.GetPosition(CA_MainCa_Mouse).X - oldOffset_Mouse;
                    //Canvas.SetTop(STK_MainStk_Mouse, e.GetPosition(CA_MainCa_Mouse).Y - oldOffset_Mouse);
                }
            };
            this.CA_MainCa_Mouse.MouseLeave += (sender, e) =>
            {
                ReleaseMouse(e.GetPosition(CA_MainCa_Mouse));
            };
            this.CA_MainCa_Mouse.PreviewMouseUp += (sender, e) =>
            {
                ReleaseMouse(e.GetPosition(CA_MainCa_Mouse));
            };
            InertiaAnimationBack_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)BackTimeFromMillionSeconds)),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
                To = 0,
                DecelerationRatio = 1
                //EasingFunction = new System.Windows.Media.Animation.CircleEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
            };
            //InertiaAnimation_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            //{
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)InertiraTime)),
            //    FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
            //    By = 400,
            //    DecelerationRatio = 1
            //    //EasingFunction = new System.Windows.Media.Animation.CircleEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
            //};
            //timer_Mouse.Interval = TimeSpan.FromMilliseconds(1);
           // timer_Mouse.Tick += new EventHandler((sender, e) => { counttime_Mouse++; });
            //InertiaAnimation_Mouse.Completed += (sender, e) =>
            //{
            //    EnsureBack();
            //};
            InertiaAnimationBack_Mouse.Completed += (sender, e) => { if (EndOperation != null) { EndOperation(); } };
        }
        private void ReleaseMouse(Point p)
        {
            isdrag_Mouse = false;
            double nowoffset = p.X;
            EnsureBack(nowoffset>startOffset_Mouse);
        }
        public double MaxTop
        {
            get { return STK_MainStk_Mouse.ActualWidth - CA_MainCa_Mouse.ActualWidth; }
        }
        public double BoundSpeed = 15;
        public double BackTimeFromMillionSeconds = 200;
        public double InertiraTime = 800;
        public delegate void ClearFlagEventHandler();
        public event ClearFlagEventHandler ClearFlag;
        public delegate void BeginOperationEventHandler();
        public event BeginOperationEventHandler BeginOperation;
        public delegate void EndOperationEventHandler();
        public event EndOperationEventHandler EndOperation;
        public double ClearBound = 2;
        public double SegmentLength = 800;
        public int PageNumber = 4;
        #endregion
        #region private area
        private double GetSpeedRateOffset(double p)
        {
            return SegmentLength;
        }
        private void EnsureBack(bool leftright)
        {
            if (MaxTop > 0)
            {
                if (TTF_transform.X < -((PageNumber-1)*SegmentLength))
                {
                    InertiaAnimationBack_Mouse.To = -((PageNumber - 1) * SegmentLength);
                    TTF_transform.BeginAnimation(TranslateTransform.XProperty, InertiaAnimationBack_Mouse);
                }
                else if (TTF_transform.X > 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    TTF_transform.BeginAnimation(TranslateTransform.XProperty, InertiaAnimationBack_Mouse);
                }
                else
                {
                    if (!AtSteadyPoint())
                    {
                        double targetX = GetSteadyPoint(TTF_transform.X,leftright);
                        InertiaAnimationBack_Mouse.By = null;
                        InertiaAnimationBack_Mouse.To = targetX;
                        TTF_transform.BeginAnimation(TranslateTransform.XProperty, InertiaAnimationBack_Mouse);
                    }
                    if (EndOperation != null) { EndOperation(); }
                }
            }
            else
            {
                if (TTF_transform.X != 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    TTF_transform.BeginAnimation(TranslateTransform.XProperty, InertiaAnimationBack_Mouse);
                }
                else
                {
                    if (EndOperation != null) { EndOperation(); }
                }
            }
        }

        private double GetSteadyPoint(double p, bool direction)
        {
            if (direction)
            {
                int index = (int)((-p) / SegmentLength + 0.25);
                return -index * SegmentLength;
            }
            else
            {
                int index = (int)((-p) / SegmentLength + 0.75);
                return -index * SegmentLength;
            }
        }

        private bool AtSteadyPoint()
        {
            for (int i = 0; i < PageNumber; i++)
            {
                if (TTF_transform.X == -i * SegmentLength)
                {
                    return true;
                }
            }
            return false;
        }
        System.Windows.Media.Animation.DoubleAnimation InertiaAnimationBack_Mouse;
        Canvas CA_MainCa_Mouse;
        FrameworkElement STK_MainStk_Mouse;
        TranslateTransform TTF_transform;
        double oldOffset_Mouse = 0;
        double startOffset_Mouse = 0;
        bool isdrag_Mouse = false;
        #endregion
    }
}

namespace MainPanelCircle
{
    public class TouchMouseInMainPanel
    {
        #region public var
        public double Bound_Speed = 3;
        public double Bound_Distence = 3;
        #endregion
        #region 操作的控件
        private Canvas CA_MainCanvas;
        private MainPanel.Circle UC_Circle;
        #endregion
        #region private vars
        DoubleAnimation Ani_CWswitch;
        bool isdrag = false;
        Point startPoint = default(Point);
        Point centerPoint = default(Point);
        Point lastPoint = default(Point);
        double startStart = 0;
        int counttime = 0;
        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
        #endregion
        public void InitTouchOperation(Canvas CA_Main,MainPanel.Circle circle)
        {         
            #region InitVars
            UC_Circle = circle;
            CA_MainCanvas = CA_Main;
            Ani_CWswitch = new DoubleAnimation()
            {
                FillBehavior = FillBehavior.HoldEnd,
                DecelerationRatio = 0.5,
            };
            centerPoint = new Point(403, 267);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler((sender, e) => { counttime++; });
            #endregion
            #region event handlers
            CA_MainCanvas.PreviewMouseDown += new MouseButtonEventHandler((sender, e) =>
            {
                PressMouse(e.GetPosition(CA_MainCanvas));
            });
            CA_MainCanvas.PreviewMouseUp += new MouseButtonEventHandler((sender, e) =>
            {
                ReleaseMouse(e.GetPosition(CA_MainCanvas));
            });
            CA_MainCanvas.MouseLeave += (sender, e) =>
            {
                ReleaseMouse(e.GetPosition(CA_MainCanvas));
            };
            CA_MainCanvas.PreviewMouseMove += new MouseEventHandler((sender, e) =>
            {
                MoveMouse(e);
            });
            #endregion
        }
        #region operations
        private void ReleaseMouse(Point releasePoint)
        {
            isdrag = false;
            timer.Stop();
            if (counttime > 0 && (GetDistence(startPoint, releasePoint) / counttime >= Bound_Speed))
            {
                BeginAutoMove(startPoint, releasePoint);
            }
            counttime = 0;
        }
        private void PressMouse(Point pressPoint)
        {
            isdrag = true;
            startStart = this.UC_Circle.Start;
            startPoint =pressPoint;
            lastPoint = startPoint;
            timer.Start();
        }
        private void MoveMouse(MouseEventArgs e)
        {
            if (isdrag && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(CA_MainCanvas);
                if (GetDistence(lastPoint, endPoint) > Bound_Distence)
                {
                    this.UC_Circle.BeginAnimation(Circle.StartProperty, null);
                    DragMove(lastPoint, endPoint, -1);
                    lastPoint = endPoint;
                }
            }
        }
        #endregion
        #region assistence
        private double GetDistence(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        private void BeginAutoMove(Point start, Point end)
        {
            double v0X = start.X - centerPoint.X;
            double v0Y = start.Y - centerPoint.Y;
            double vmX = end.X - centerPoint.X;
            double vmY = end.Y - centerPoint.Y;
            if (v0Y * (vmX - v0X) - v0X * (vmY - v0Y) > 0)
            {
                Ani_CWswitch.By = 180;
            }
            else
            {
                Ani_CWswitch.By = -180;
            }
            this.UC_Circle.BeginAnimation(Circle.StartProperty, Ani_CWswitch);
        }
        private void DragMove(Point start, Point end, double rate)
        {
            double v0X = start.X - centerPoint.X;
            double v0Y = start.Y - centerPoint.Y;
            double vmX = end.X - centerPoint.X;
            double vmY = end.Y - centerPoint.Y;
            if (v0Y * (vmX - v0X) - v0X * (vmY - v0Y) > 0)
            {
                this.UC_Circle.Start = startStart - rate;// rate * (t - t0);
                startStart = this.UC_Circle.Start;
            }
            else
            {
                this.UC_Circle.Start = startStart + rate;// rate * (t - t0);
                startStart = this.UC_Circle.Start;
            }
        }
        #endregion
    }
}