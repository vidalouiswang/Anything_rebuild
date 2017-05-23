﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Anything.Form;
using ApplicationInformations.Anything;

namespace Anything.Class
{
    public sealed class Animation
    {
        private Animation() { }
        public static animation AnimationInstance = new animation();
        public static animation GetInstance()
        {
            return AnimationInstance;
        }

        /// <summary>
        /// 用于执行单个动画的通用函数
        /// </summary>
        /// <typeparam name="TYPE">主体类型</typeparam>
        /// <param name="TarObj">主体引用</param>
        /// <param name="dp">用于动画的依赖项属性</param>
        /// <param name="secTS">时间长度，以秒为单位</param>
        /// <param name="To">目标值</param>
        /// <param name="From">初始值</param>
        /// <param name="fl">完成后的动作</param>
        /// <returns></returns>
        public static int UniversalBeginDoubleAnimation<TYPE>(ref TYPE TarObj, DependencyProperty dp, double secTS, double To, double From = 0F, FillBehavior fl = FillBehavior.HoldEnd)
        {
            if (TarObj == null)
            {
                return -1;
            }

            DoubleAnimation da = new DoubleAnimation();
            if (From != 0)
                da.From = From;
            da.To = To;
            da.Duration = TimeSpan.FromSeconds(secTS);
            da.FillBehavior = fl;
            da.AutoReverse = false;

            if (typeof(TYPE) == typeof(Window))
            {
                Window wnd = TarObj as Window;
                if (wnd == null)
                {
                    return -2;
                }
                wnd.BeginAnimation(dp, da);
                return 0;
            }
            else if (typeof(TYPE) == typeof(StackPanel))
            {
                StackPanel stackpanel = TarObj as StackPanel;
                if (stackpanel == null)
                {
                    return -2;
                }
                stackpanel.BeginAnimation(dp, da);
                return 0;
            }
            else if (typeof(TYPE) == typeof(Border))
            {
                Border border = TarObj as Border;
                if (border == null)
                {
                    return -2;
                }
                border.BeginAnimation(dp, da);
                return 0;
            }
            else if (typeof(TYPE) == typeof(Button))
            {
                Button button = TarObj as Button;
                if (button == null)
                {
                    return -2;
                }
                button.BeginAnimation(dp, da);
                return 0;
            }
            else
            {
                return -3;
            }

        }


        public class animation
        {
            private Window wnd = null;
            private int Way = 0;
            private bool Closing = false;

            /// <summary>
            /// 无参构造
            /// </summary>
            public animation() { }


            /// <summary>
            /// 初始化窗体border的样式
            /// </summary>
            /// <param name="bdr"></param>
            public void InitBdrStyle(ref Border bdr)
            {
                //获取最小透明度
                double minOpa = AppInfoOperations.GetMinOpacity();

                //获取最大透明度
                double maxOpa = AppInfoOperations.GetMaxOpacity();

                //获取显示时长
                double showTimeSpan = AppInfoOperations.GetShowTimeSpan();

                //获取隐藏时长
                double hideTimeSpan = AppInfoOperations.GetHideTimeSpan();

                //获取超时时长
                double TimeoutSpan = AppInfoOperations.GetTimeoutTimeSpan();


                DoubleAnimation daShow = new DoubleAnimation();
                DoubleAnimation daHide = new DoubleAnimation();

                Storyboard sbShow = new Storyboard();
                Storyboard sbHide = new Storyboard();

                BeginStoryboard bsShow = new BeginStoryboard();
                BeginStoryboard bsHide = new BeginStoryboard();

                EventTrigger etShow = new EventTrigger(Mouse.MouseEnterEvent);
                EventTrigger etHide = new EventTrigger(Mouse.MouseLeaveEvent);

                Style style = new Style();


                //show
                daShow.To = maxOpa;
                daShow.Duration = TimeSpan.FromSeconds(showTimeSpan);
                Storyboard.SetTargetProperty(daShow, new PropertyPath(UIElement.OpacityProperty));

                Storyboard.SetDesiredFrameRate(sbShow, 60);
                sbShow.Children.Add(daShow);
                bsShow.Storyboard = sbShow;

                etShow.Actions.Add(bsShow);
                style.Triggers.Add(etShow);


                //hide

                daHide.To = minOpa;
                daHide.Duration = TimeSpan.FromSeconds(hideTimeSpan);
                Storyboard.SetTargetProperty(daHide, new PropertyPath(UIElement.OpacityProperty));

                sbHide.BeginTime = TimeSpan.FromSeconds(TimeoutSpan);
                Storyboard.SetDesiredFrameRate(sbHide, 60);
                sbHide.Children.Add(daHide);
                bsHide.Storyboard = sbHide;

                etHide.Actions.Add(bsHide);
                style.Triggers.Add(etHide);


                //apply
                bdr.Style = style;
            }


            /// <summary>
            /// 内部函数
            /// </summary>
            /// <param name="wnd"></param>
            /// <param name="From"></param>
            /// <param name="To"></param>
            /// <param name="ts"></param>
            /// <param name="Max"></param>
            private void OpacityChange(Window wnd, double From, double To, TimeSpan ts, bool Max = false)
            {

                DoubleAnimation da = new DoubleAnimation();
                da.From = From;
                da.To = To;
                if (Max)
                {
                    da.Completed += Da_Completed;
                }
                da.Duration = ts;
                wnd.BeginAnimation(Window.OpacityProperty, da);

            }

            private void Da_Completed(object sender, EventArgs e)
            {
                if (Closing)
                {
                    this.wnd.Close();
                }
                if (Way == 0)
                {
                    wnd.WindowState = WindowState.Maximized;
                    OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.3));
                }
                else
                {
                    wnd.WindowState = WindowState.Normal;
                    OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.3));
                    Way = 0;
                }
            }

            #region 操作

            /// <summary>
            /// 最小化
            /// </summary>
            /// <param name="wnd"></param>
            public void SetMin(Window wnd)
            {

                OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.5));
            }

            /// <summary>
            /// 还原
            /// </summary>
            /// <param name="wnd"></param>
            public void SetNormal(Window wnd)
            {
                
                OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.5));

            }

            /// <summary>
            /// 最大化
            /// </summary>
            /// <param name="wnd"></param>
            /// <param name="Way"></param>
            public void SetMax(Window wnd, int Way = 0)
            {
                if (Way == 0)
                {
                    this.wnd = wnd;
                    OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
                }
                else
                {
                    this.wnd = wnd;
                    this.Way = Way;
                    OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
                }

            }

            /// <summary>
            /// 关闭
            /// </summary>
            /// <param name="wnd"></param>
            public void Close(Window wnd)
            {
                this.Closing = true;
                this.wnd = wnd;
                OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
            }

            #endregion

            public void SetStackPanelStyle(ref Border bdr, double InneraActualHeight)
            {

                DoubleAnimation daShowHeight = new DoubleAnimation(5, InneraActualHeight, TimeSpan.FromSeconds(0.5), FillBehavior.HoldEnd);
                DoubleAnimation daHideHeight = new DoubleAnimation(bdr.ActualHeight, 5, TimeSpan.FromSeconds(0.5), FillBehavior.HoldEnd);


                Storyboard sbHide = new Storyboard();
                Storyboard sbShow = new Storyboard();

                sbHide.BeginTime = TimeSpan.FromSeconds(1);
                sbShow.BeginTime = TimeSpan.FromSeconds(0.5);


                BeginStoryboard bsdShow = new BeginStoryboard();
                BeginStoryboard bsdHide = new BeginStoryboard();

                bsdHide.HandoffBehavior = HandoffBehavior.Compose;
                bsdShow.HandoffBehavior = HandoffBehavior.Compose;


                EventTrigger etShow = new EventTrigger();
                EventTrigger etHide = new EventTrigger();

                Storyboard.SetTargetProperty(daShowHeight, new PropertyPath(StackPanel.HeightProperty));
                Storyboard.SetTargetProperty(daHideHeight, new PropertyPath(StackPanel.HeightProperty));

                sbHide.Children.Add(daHideHeight);
                sbShow.Children.Add(daShowHeight);

                bsdHide.Storyboard = sbHide;
                bsdShow.Storyboard = sbShow;

                etShow.RoutedEvent = Mouse.MouseUpEvent;
                etHide.RoutedEvent = Mouse.MouseLeaveEvent;

                etShow.Actions.Add(bsdShow);
                etHide.Actions.Add(bsdHide);


                Style style = new Style();

                style.Triggers.Add(etShow);
                style.Triggers.Add(etHide);


                bdr.Style = style;

            }

            #region 事件响应

            /// <summary>
            /// 窗体状态发生改变时的时间处理
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void Window_StateChanged(object sender, EventArgs e)
            {
                Window wnd = (Window)sender;
                if (wnd.Opacity == 0 && wnd.WindowState == WindowState.Minimized)
                {
                    wnd.WindowState = WindowState.Normal;
                    wnd.Opacity = 0.01;
                    SetNormal(wnd);
                }


            }

            /// <summary>
            /// 窗体大小发生改变时的事件处理
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void Window_SizeChanged(object sender, SizeChangedEventArgs e)
            {

                wndMain wnd = (wndMain)sender;
                wnd.UpdateLayout();
                if (wnd.Opacity == 0 && wnd.WindowState == WindowState.Normal)
                {
                    wnd.WindowState = WindowState.Minimized;

                }
                if (wnd.IsInformationsInitialized)
                {
                    AppInfoOperations.SetWidth(wnd.ActualWidth);
                    AppInfoOperations.SetHeight(wnd.ActualHeight);

                }
            }
            #endregion
        }
    }

}
