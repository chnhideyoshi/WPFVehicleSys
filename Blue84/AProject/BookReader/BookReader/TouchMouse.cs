using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace ManipTest
{
    public class TouchMouseWithRebound
    {
        #region public method and attribute
        public void InitTouchListOperation(Canvas mainCanvas, Panel mainStackPanel)
        {
            CA_MainCa_Mouse = mainCanvas;
            STK_MainStk_Mouse = mainStackPanel;
            this.CA_MainCa_Mouse.PreviewMouseDown += (sender, e) =>
            {
                isdrag_Mouse = true;
                oldOffset_Mouse = e.GetPosition(STK_MainStk_Mouse).Y;
                startOffset_Mouse = e.GetPosition(CA_MainCa_Mouse).Y;
                timer_Mouse.Start();
                if (BeginOperation != null) { BeginOperation(); }
            };
            this.CA_MainCa_Mouse.PreviewMouseMove += (sender, e) =>
            {
                if (isdrag_Mouse && e.LeftButton == MouseButtonState.Pressed)
                {
                    if (ClearFlag != null && Math.Abs(e.GetPosition(CA_MainCa_Mouse).Y - startOffset_Mouse) > ClearBound)
                    {
                        ClearFlag();
                    }
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, null);
                    Canvas.SetTop(STK_MainStk_Mouse, e.GetPosition(CA_MainCa_Mouse).Y - oldOffset_Mouse);
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
                DecelerationRatio = 1,
            };
            InertiaAnimation_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)InertiraTime)),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
                By = 400,
                DecelerationRatio = 1,
            };
            timer_Mouse.Interval = TimeSpan.FromMilliseconds(1);
            timer_Mouse.Tick += new EventHandler((sender, e) => { counttime_Mouse++; });
            InertiaAnimation_Mouse.Completed += (sender, e) =>
            {
                EnsureBack();
            };
            InertiaAnimationBack_Mouse.Completed += (sender, e) => { if (EndOperation != null) { EndOperation(); } };
        }
        public void ResetBack(bool withPosition)
        {
            double top = Canvas.GetTop(STK_MainStk_Mouse);
            STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, null);
            if (withPosition)
            {
                Canvas.SetTop(STK_MainStk_Mouse, top);
            }
        }
        private void ReleaseMouse(Point p)
        {
            isdrag_Mouse = false;
            timer_Mouse.Stop();
            double nowoffset = p.Y;
            if (counttime_Mouse == 0) { if (EndOperation != null) { EndOperation(); } return; }
            if (Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) >= BoundSpeed)
            {
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse > 0)
                {
                    double offset = GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    InertiaAnimation_Mouse.By = offset;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                }
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse < 0)
                {
                    double offset = -GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    InertiaAnimation_Mouse.By = offset;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                }
            }
            if (counttime_Mouse > 0 && Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) < BoundSpeed)
            {
                EnsureBack();
            }
            counttime_Mouse = 0;
        }
        public double MaxTop
        {
            get { return STK_MainStk_Mouse.ActualHeight - CA_MainCa_Mouse.ActualHeight; }
        }
        public double BoundSpeed = 20;
        public double BackTimeFromMillionSeconds = 200;
        public double InertiraTime = 800;
        public delegate void ClearFlagEventHandler();
        public event ClearFlagEventHandler ClearFlag;
        public delegate void BeginOperationEventHandler();
        public event BeginOperationEventHandler BeginOperation;
        public delegate void EndOperationEventHandler();
        public event EndOperationEventHandler EndOperation;
        public double ClearBound = 2;
        #endregion
        #region private area
        private double GetSpeedRateOffset(double p)
        {
            return 50 * p;
        }
        private void EnsureBack()
        {
            if (MaxTop > 0)
            {
                if (Canvas.GetTop(STK_MainStk_Mouse) < -MaxTop)
                {
                    InertiaAnimationBack_Mouse.To = -MaxTop;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimationBack_Mouse);
                }
                else if (Canvas.GetTop(STK_MainStk_Mouse) > 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimationBack_Mouse);
                }
                else
                {
                    if (EndOperation != null) { EndOperation(); }
                }
            }
            else
            {
                if (Canvas.GetTop(STK_MainStk_Mouse) != 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimationBack_Mouse);
                }
                else
                {
                    if (EndOperation != null) { EndOperation(); }
                }
            }
        }
        System.Windows.Media.Animation.DoubleAnimation InertiaAnimation_Mouse;
        System.Windows.Media.Animation.DoubleAnimation InertiaAnimationBack_Mouse;
        Canvas CA_MainCa_Mouse;
        Panel STK_MainStk_Mouse;
        System.Windows.Threading.DispatcherTimer timer_Mouse = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
        int counttime_Mouse = 0;
        double oldOffset_Mouse = 0;
        double startOffset_Mouse = 0;
        bool isdrag_Mouse = false;
        #endregion
    }
}

