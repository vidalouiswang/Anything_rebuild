using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Anything.Class;
using Anything.UserControls;

namespace Anything.Form
{
    /// <summary>
    /// wndSE.xaml 的交互逻辑
    /// </summary>
    public partial class wndSE : Window
    {

        private DispatcherTimer timer = new DispatcherTimer();

        public wndSE()
        {
            InitializeComponent();
            InitTimer();
        }

        private void InitTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (new WindowInteropHelper(this).Handle != HotKey.GetActiveWindow())
            {
                this.timer.Stop();
                this.Close();
            }
        }

        public wndSE(string keyword)
        {
            InitializeComponent();
            InitTimer();
            if (Manage.listOfSearchEngineInnerData.Count > 0)
            {
                foreach (SearchEngineItem item in Manage.listOfSearchEnginesVisualElement)
                {
                    item.Keyword = keyword;
                    this.spMain.Children.Add(item);
                }
            }
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.spMain.Children.Count > 0)
                this.spMain.Children[0].Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.spMain.Children.Clear();
        }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Close();
        }
    }
}
