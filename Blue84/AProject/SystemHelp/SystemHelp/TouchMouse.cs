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
                if (BeginOperation != null){ BeginOperation(); }
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
                DecelerationRatio=1
                //EasingFunction = new System.Windows.Media.Animation.CircleEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
            };
            InertiaAnimation_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)InertiraTime)),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
                By = 400,
                DecelerationRatio = 1
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
            STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty,null);
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
            //if (Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) >= BoundSpeed)
            //{
            //    if ((nowoffset - startOffset_Mouse) / counttime_Mouse > 0)
            //    {
            //        double offset = GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
            //        InertiaAnimation_Mouse.By = offset;
            //        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
            //    }
            //    if ((nowoffset - startOffset_Mouse) / counttime_Mouse < 0)
            //    {
            //        double offset = -GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
            //        InertiaAnimation_Mouse.By = offset;
            //        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
            //    }
            //}
            //if (counttime_Mouse > 0 && Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) < BoundSpeed)
            //{
                EnsureBack();
            //}
            counttime_Mouse = 0;
        }
        public double MaxTop
        {
            get { return STK_MainStk_Mouse.ActualHeight - CA_MainCa_Mouse.ActualHeight; }
        }
        public double BoundSpeed = 8;
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
    public class TouchMouseWithOutRebound
    {
        #region public method and attribute
        public void InitTouchListOperation(Canvas mainCanvas, Panel mainStackPanel)
        {
            CA_MainCa_Mouse = mainCanvas;
            STK_MainStk_Mouse = mainStackPanel;
            this.CA_MainCa_Mouse.PreviewMouseDown += (sender, e) =>
            {
                if (MaxTop <= 0) { return; }
                isdrag_Mouse = true;
                oldOffset_Mouse = e.GetPosition(STK_MainStk_Mouse).Y;
                startOffset_Mouse = e.GetPosition(CA_MainCa_Mouse).Y;
                timer_Mouse.Start();
            };
            this.CA_MainCa_Mouse.PreviewMouseMove += (sender, e) =>
            {
                if (MaxTop <= 0) { return; }
                if (isdrag_Mouse && e.LeftButton == MouseButtonState.Pressed)
                {
                    if (ClearFlag != null && Math.Abs(e.GetPosition(CA_MainCa_Mouse).Y - startOffset_Mouse) > ClearBound)
                    {
                        ClearFlag();
                    }
                    STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, null);
                    if (e.GetPosition(CA_MainCa_Mouse).Y - oldOffset_Mouse <= 0 && e.GetPosition(CA_MainCa_Mouse).Y - oldOffset_Mouse >= -MaxTop)
                    {
                        Canvas.SetTop(STK_MainStk_Mouse, e.GetPosition(CA_MainCa_Mouse).Y - oldOffset_Mouse);
                    }
                }
            };
            this.CA_MainCa_Mouse.MouseLeave += (sender, e) =>
            {
                if (MaxTop <= 0) { return; }
                ReleaseMouse(e.GetPosition(CA_MainCa_Mouse));
            };
            this.CA_MainCa_Mouse.PreviewMouseUp += (sender, e) =>
            {
                if (MaxTop <= 0) { return; }
                ReleaseMouse(e.GetPosition(CA_MainCa_Mouse));
            };
            InertiaAnimation_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)InertiraTime)),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
                By = 400,
                DecelerationRatio = 1
            };
            timer_Mouse.Interval = TimeSpan.FromMilliseconds(1);
            timer_Mouse.Tick += new EventHandler((sender, e) => { counttime_Mouse++; });
        }

        private void ReleaseMouse(Point p)
        {
            isdrag_Mouse = false;
            timer_Mouse.Stop();
            double nowoffset = p.Y;
            if (counttime_Mouse == 0) { return; }
            if (Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) >= BoundSpeed)
            {
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse > 0)
                {
                    double offset = GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    if (Canvas.GetTop(STK_MainStk_Mouse) + offset > 0)
                    {
                        InertiaAnimation_Mouse.By = null;
                        InertiaAnimation_Mouse.To = 0;
                        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                    }
                    else
                    {
                        InertiaAnimation_Mouse.To = null;
                        InertiaAnimation_Mouse.By = offset;
                        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                    }

                }
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse < 0)
                {
                    double offset = -GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    if (Canvas.GetTop(STK_MainStk_Mouse) + offset < -MaxTop)
                    {
                        InertiaAnimation_Mouse.To = -MaxTop;
                        InertiaAnimation_Mouse.By = null;
                        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                    }
                    else
                    {
                        InertiaAnimation_Mouse.To = null;
                        InertiaAnimation_Mouse.By = offset;
                        STK_MainStk_Mouse.BeginAnimation(Canvas.TopProperty, InertiaAnimation_Mouse);
                    }
                }
            }
            if (counttime_Mouse > 0 && Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) < BoundSpeed)
            {
                EnsureBack();
            }
            counttime_Mouse = 0;
        }

        private void EnsureBack()
        {

        }
        public double MaxTop
        {
            get { return STK_MainStk_Mouse.ActualHeight - CA_MainCa_Mouse.ActualHeight; }
        }
        public double BoundSpeed = 15;
        public double BackTimeFromMillionSeconds = 200;
        public double InertiraTime = 800;
        public delegate void ClearFlagEventHandler();
        public event ClearFlagEventHandler ClearFlag;
        public double ClearBound = 2;
        #endregion
        #region private area
        private double GetSpeedRateOffset(double p)
        {
            return 50 * p;
        }
        System.Windows.Media.Animation.DoubleAnimation InertiaAnimation_Mouse;
        //System.Windows.Media.Animation.DoubleAnimation InertiaAnimationBack_Mouse;
        Canvas CA_MainCa_Mouse;
        Panel STK_MainStk_Mouse;
        System.Windows.Threading.DispatcherTimer timer_Mouse = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
        int counttime_Mouse = 0;
        double oldOffset_Mouse = 0;
        double startOffset_Mouse = 0;
        bool isdrag_Mouse = false;
        #endregion
    }
    //public class Manip
    //{
    //    #region Manipulation
    //    public double BackTimeFromMillionSeconds = 300;
    //    bool istop = true;
    //    Canvas CA_TouchPad;
    //    Panel STK_Panel;
    //    System.Windows.Media.Animation.DoubleAnimation InertiaAnimationBack;
    //    TranslateTransform translateTransform;
    //    InertiaTranslationBehavior inertiaTranslationBehavior = new InertiaTranslationBehavior();
    //    double startOffSet = 0;
    //    public void InitManipulationOperationOnCanvasTop(Canvas layoutRoot, Panel container)
    //    {
    //        CA_TouchPad = layoutRoot;
    //        STK_Panel = container;
    //        STK_Panel.IsManipulationEnabled = true;
    //        CA_TouchPad.ClipToBounds = true;
    //        InertiaAnimationBack = new System.Windows.Media.Animation.DoubleAnimation()
    //        {
    //            Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)BackTimeFromMillionSeconds)),
    //            FillBehavior = System.Windows.Media.Animation.FillBehavior.Stop,
    //            To = 0,
    //            EasingFunction = new System.Windows.Media.Animation.CircleEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
    //        };
    //        InertiaAnimationBack.Completed += (sender, e) =>
    //        {
    //            if (istop)
    //            {
    //                Canvas.SetTop(STK_Panel, 0);
    //            }
    //            else
    //            {
    //                Canvas.SetTop(STK_Panel, -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight));
    //            }
    //        };
    //        CA_TouchPad.ManipulationStarting += ((sender, e) =>
    //        {
    //            e.ManipulationContainer = CA_TouchPad;
    //            e.Mode = ManipulationModes.TranslateY;
    //        });
    //        CA_TouchPad.ManipulationDelta += ((sender, e) =>
    //        {
    //            //FrameworkElement element = e.OriginalSource as FrameworkElement;
    //            if (Canvas.GetTop(STK_Panel) + e.DeltaManipulation.Translation.Y <=
    //                -STK_Panel.ActualHeight + 0.2 * CA_TouchPad.ActualHeight)
    //            {
    //                e.Handled = true;
    //                return;
    //            }
    //            if (Canvas.GetTop(STK_Panel) + e.DeltaManipulation.Translation.Y >= 0.8 * CA_TouchPad.ActualHeight)
    //            {
    //                e.Handled = true;
    //                return;
    //            }
    //            Canvas.SetTop(STK_Panel, Canvas.GetTop(STK_Panel) + e.DeltaManipulation.Translation.Y);
    //            e.Handled = true;
    //        });
    //        CA_TouchPad.ManipulationCompleted += ((sender, e) =>
    //        {
    //            if (Canvas.GetTop(STK_Panel) < -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight))
    //            {
    //                InertiaAnimationBack.To = -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight);
    //                istop = false;
    //                STK_Panel.BeginAnimation(Canvas.TopProperty, InertiaAnimationBack);
    //                return;
    //            }
    //            if (Canvas.GetTop(STK_Panel) > 0)
    //            {
    //                InertiaAnimationBack.To = 0;
    //                istop = true;
    //                STK_Panel.BeginAnimation(Canvas.TopProperty, InertiaAnimationBack);
    //                return;
    //            }
    //        });
    //        CA_TouchPad.ManipulationInertiaStarting += ((sender, e) =>
    //        {
    //            e.TranslationBehavior = inertiaTranslationBehavior;
    //            e.TranslationBehavior.InitialVelocity = e.InitialVelocities.LinearVelocity;
    //            e.TranslationBehavior.DesiredDeceleration = 0.005;
    //        });
    //    }
    //    public void InitManipulationOperationOnTranslateTransform(Canvas layoutRoot, Panel container)
    //    {
    //        CA_TouchPad = layoutRoot;
    //        STK_Panel = container;
    //        translateTransform = new TranslateTransform(0, 0);
    //        STK_Panel.RenderTransform = translateTransform;
    //        STK_Panel.IsManipulationEnabled = true;
    //        CA_TouchPad.ClipToBounds = true;
    //        InertiaAnimationBack = new System.Windows.Media.Animation.DoubleAnimation()
    //        {
    //            Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)BackTimeFromMillionSeconds)),
    //            FillBehavior = System.Windows.Media.Animation.FillBehavior.Stop,
    //            To = 0,
    //            EasingFunction = new System.Windows.Media.Animation.CircleEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
    //        };
    //        InertiaAnimationBack.Completed += (sender, e) =>
    //        {
    //            if (istop)
    //            {
    //                translateTransform.Y = 0;
    //            }
    //            else
    //            {
    //                translateTransform.Y = -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight);
    //            }
    //        };
    //        CA_TouchPad.ManipulationStarting += ((sender, e) =>
    //        {
    //            e.ManipulationContainer = CA_TouchPad;
    //            startOffSet = translateTransform.Y;
    //            e.Mode = ManipulationModes.TranslateY;
    //        });
    //        CA_TouchPad.ManipulationDelta += ((sender, e) =>
    //        {
    //            translateTransform.Y += e.DeltaManipulation.Translation.Y;
    //        });
    //        CA_TouchPad.ManipulationCompleted += ((sender, e) =>
    //        {
    //            if (Math.Abs(e.TotalManipulation.Translation.Y) > 5)
    //            {

    //            }
    //            if (translateTransform.Y < -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight))
    //            {
    //                InertiaAnimationBack.To = -(STK_Panel.ActualHeight - CA_TouchPad.ActualHeight);
    //                istop = false;
    //                translateTransform.BeginAnimation(TranslateTransform.YProperty, InertiaAnimationBack);
    //                return;
    //            }
    //            if (translateTransform.Y > 0)
    //            {
    //                InertiaAnimationBack.To = 0;
    //                istop = true;
    //                translateTransform.BeginAnimation(TranslateTransform.YProperty, InertiaAnimationBack);
    //                return;
    //            }
    //        });
    //        CA_TouchPad.ManipulationInertiaStarting += ((sender, e) =>
    //        {
    //            e.TranslationBehavior = inertiaTranslationBehavior;
    //            e.TranslationBehavior.InitialVelocity = e.InitialVelocities.LinearVelocity;
    //            e.TranslationBehavior.DesiredDeceleration = 0.005;
    //        });
    //    }
    //    #endregion
    //}
    public class TouchMouseHorizontalWithRebound
    {
        #region public method and attribute
        public void InitTouchListOperation(Canvas mainCanvas, Panel mainStackPanel)
        {
            CA_MainCa_Mouse = mainCanvas;
            STK_MainStk_Mouse = mainStackPanel;
            this.CA_MainCa_Mouse.PreviewMouseDown += (sender, e) =>
            {
                isdrag_Mouse = true;
                oldOffset_Mouse = e.GetPosition(STK_MainStk_Mouse).X;
                startOffset_Mouse = e.GetPosition(CA_MainCa_Mouse).X;
                timer_Mouse.Start();
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
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, null);
                    Canvas.SetLeft(STK_MainStk_Mouse, e.GetPosition(CA_MainCa_Mouse).X - oldOffset_Mouse);
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
            };
            InertiaAnimation_Mouse = new System.Windows.Media.Animation.DoubleAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)InertiraTime)),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd,
                By = 400,
                DecelerationRatio = 1
            };
            timer_Mouse.Interval = TimeSpan.FromMilliseconds(1);
            timer_Mouse.Tick += new EventHandler((sender, e) => { counttime_Mouse++; });
            InertiaAnimation_Mouse.Completed += (sender, e) =>
            {
                EnsureBack();
            };
            InertiaAnimationBack_Mouse.Completed += (sender, e) => { if (EndOperation != null) { EndOperation(); } };
        }

        private void ReleaseMouse(Point p)
        {
            isdrag_Mouse = false;
            timer_Mouse.Stop();
            double nowoffset = p.X;
            if (counttime_Mouse == 0) { if (EndOperation != null) { EndOperation(); } return; }
            if (Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) >= BoundSpeed)
            {
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse > 0)
                {
                    double offset = GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    InertiaAnimation_Mouse.By = offset;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, InertiaAnimation_Mouse);
                }
                if ((nowoffset - startOffset_Mouse) / counttime_Mouse < 0)
                {
                    double offset = -GetSpeedRateOffset(Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse));
                    InertiaAnimation_Mouse.By = offset;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, InertiaAnimation_Mouse);
                }
            }
            if (counttime_Mouse > 0 && Math.Abs((nowoffset - startOffset_Mouse) / counttime_Mouse) < BoundSpeed)
            {
                EnsureBack();
            }
            counttime_Mouse = 0;
        }
        public double MaxLeft
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
        #endregion
        #region private area
        private double GetSpeedRateOffset(double p)
        {
            return 50 * p;
        }
        private void EnsureBack()
        {
            if (MaxLeft > 0)
            {
                if (Canvas.GetLeft(STK_MainStk_Mouse) < -MaxLeft)
                {
                    InertiaAnimationBack_Mouse.To = -MaxLeft;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, InertiaAnimationBack_Mouse);
                }
                else if (Canvas.GetLeft(STK_MainStk_Mouse) > 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, InertiaAnimationBack_Mouse);
                }
                else
                {
                    if (EndOperation != null) { EndOperation(); }
                }
            }
            else
            {
                if (Canvas.GetLeft(STK_MainStk_Mouse) != 0)
                {
                    InertiaAnimationBack_Mouse.To = 0;
                    STK_MainStk_Mouse.BeginAnimation(Canvas.LeftProperty, InertiaAnimationBack_Mouse);
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
