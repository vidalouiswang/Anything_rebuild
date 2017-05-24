﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Anything.Form
{
    /// <summary>
    /// wndTip.xaml 的交互逻辑
    /// </summary>
    public partial class wndTip : Window
    {
        public string Tip
        {
            get { return (string)GetValue(TipProperty); }
            set { SetValue(TipProperty, value); }
        }
        public static readonly DependencyProperty TipProperty =
            DependencyProperty.Register("Tip", typeof(string), typeof(wndTip), new PropertyMetadata((string)"This is tip"));

        private DispatcherTimer timerFollow = new DispatcherTimer();
        private DispatcherTimer timerFixed = new DispatcherTimer();
        private Window wnd = null;

        private int Mode = -1;

        private double OffsetX = 0;
        private double OffsetY = 0;

        public wndTip()
        {
            this.DataContext = this.txt;
            InitializeComponent();
            timerFollow.Interval = TimeSpan.FromMilliseconds(5);
            timerFollow.Tick += TimerFollow_Tick;
            timerFollow.Stop();

            timerFixed.Interval = TimeSpan.FromSeconds(3);
            timerFixed.Tick += TimerFixed_Tick;
            timerFixed.Stop();

            this.Topmost = true;
            this.ShowInTaskbar = false;


        }

        private void TimerFixed_Tick(object sender, EventArgs e)
        {
            if (this.Mode == 0)
            {
                this.HideMe();
                if (timerFixed.IsEnabled == true)
                    timerFixed.IsEnabled = false;
            }
            else
            {
                this.HideMe();
                if (timerFollow.IsEnabled == true)
                    timerFollow.IsEnabled = false;
            }
        }

        private void TimerFollow_Tick(object sender, EventArgs e)
        {
            if (wnd != null)
            {
                Point pt = GetPoint();
                this.Left = pt.X + OffsetX;
                this.Top = pt.Y + OffsetY;

                if ((Left + this.ActualWidth) > SystemParameters.PrimaryScreenWidth)
                {
                    this.Left -= this.ActualWidth;
                }

                if ((this.Top + this.ActualHeight) > SystemParameters.PrimaryScreenHeight)
                {
                    this.Top -= this.ActualHeight;
                }
            }
        }

        private Point GetPoint()
        {

            if (wnd != null)
            {
                double x = Mouse.GetPosition(wnd).X + wnd.Left;
                double y = Mouse.GetPosition(wnd).Y + wnd.Top;
                return new Point(x, y);
            }
            else
                return new Point(0, 0);
        }

        public void ClearOffset()
        {
            this.OffsetX = 0;
            this.OffsetY = 0;
        }

        public void ShowFixed(Window wnd, string Text, double OffsetX = 0, double OffsetY = 0)
        {
            if (!this.IsVisible)
                this.Show();

            ClearOffset();
            this.wnd = wnd;
            this.Mode = 0;
            this.Tip = Text;

            Point pt = GetPoint();
            this.Left = pt.X + OffsetX;
            this.Top = pt.Y + OffsetY;

            if ((Left + this.ActualWidth) > SystemParameters.PrimaryScreenWidth)
            {
                this.Left -= this.ActualWidth;
            }

            if ((this.Top + this.ActualHeight) > SystemParameters.PrimaryScreenHeight)
            {
                this.Top -= this.ActualHeight;
            }

            DoubleAnimation da = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
            timerFixed.Start();


            Console.Write("wnd Height:" + this.ActualHeight.ToString() + "\n");
            Console.Write("wnd Width:" + this.ActualWidth.ToString() + "\n");

            Console.Write("txt Height:" + this.txt.ActualHeight.ToString() + "\n");
            Console.Write("txt Width:" + this.txt.ActualWidth.ToString() + "\n");
            Console.Write("------------------------------------------------------------------\n");
        }

        public void ShowFollow(Window wnd, string Text, double OffsetX = 0, double OffsetY = 0)
        {
            this.Mode = 1;
            this.wnd = wnd;
            this.Tip = Text;

            Point pt = GetPoint();
            this.Left = pt.X + OffsetX;
            this.Top = pt.Y + OffsetY;

            if ((Left + this.ActualWidth) > SystemParameters.PrimaryScreenWidth)
            {
                this.Left -= this.ActualWidth;
            }

            if ((this.Top + this.ActualHeight) > SystemParameters.PrimaryScreenHeight)
            {
                this.Top -= this.ActualHeight;
            }

            this.OffsetX = OffsetX;
            this.OffsetY = OffsetY;

            DoubleAnimation da = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
            timerFollow.Start();
        }

        public void HideMe()
        {
            if (timerFollow.IsEnabled == true)
                timerFollow.IsEnabled = false;

            DoubleAnimation da = new DoubleAnimation(this.Opacity, 0, TimeSpan.FromSeconds(0.3), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
        }

    }
}
