﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Anything.Class;
using Anything.UserControls;
using ApplicationInformations.Anything;

namespace Anything.Form
{
    /// <summary>
    /// wndMain.xaml 的交互逻辑
    /// </summary>
    public partial class wndMain : Window
    {



        #region 重载WndProc完成改变大小及响应热键

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            //定义距离
            int GripSize = 8;
            int BorderSize = 5;

            //定义Window
            Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;

            //判断消息
            if (msg == constForSize.WM_NCHITTEST)
            {
                int x = lParam.ToInt32() << 16 >> 16, y = lParam.ToInt32() >> 16;
                Point pt = wnd.PointFromScreen(new Point(x, y));

                //底部
                if (pt.X > GripSize && pt.X < wnd.ActualWidth - GripSize && pt.Y >= wnd.ActualHeight - BorderSize)
                {
                    handle = true;
                    return (IntPtr)constForSize.HTBOTTOM;
                }

                //右侧
                if (pt.Y > GripSize && pt.X > wnd.ActualWidth - BorderSize && pt.Y < wnd.ActualHeight - GripSize)
                {
                    handle = true;
                    return (IntPtr)constForSize.HTRIGHT;
                }

                //右下角
                if (pt.X > wnd.ActualWidth - GripSize && pt.Y >= wnd.ActualHeight - GripSize)
                {
                    handle = true;
                    return (IntPtr)constForSize.HTBOTTOMRIGHT;
                }
            }
            else if (msg == HotKey.WM_HOTKEY)
            {
                int wparam = wParam.ToInt32();
                if (wparam == HotKey.QUICK_SEARCH_HOTKEY_ID)
                {
                    //设置活动窗体
                    HotKey.SetForegroundWindow(Manage.WindowMainHandle);

                    this.BdrFunction.RaiseEvent(MEEnter);

                    this.ClearSearch();

                    //聚焦到检索框
                    SetFocus();
                }
                else
                {
                    //检查其他热键
                    Manage.FindHotKeyAndExecute(wparam);
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 资源初始化完成后Hook WndProc
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }

        #endregion


        #region 成员变量

        //边界颜色
        private Color bdrColor = new Color();

        //用于设置动画相关
        private Animation.animation animationInstance = Animation.GetInstance();

        //关闭按钮的计数
        private byte btnCloseOnceClick = 0;

        //用于自动存储位置大小的开关指示
        public bool IsInformationsInitialized = false;

        //用去触发上部标题栏的卷起动画
        MouseEventArgs MELeave = new MouseEventArgs(Mouse.PrimaryDevice, 0);

        //用去触发上部标题栏的显示动画
        MouseEventArgs MEEnter = new MouseEventArgs(Mouse.PrimaryDevice, 0);

        //提示窗体
        private wndTip tipMainForm = new wndTip();

        //用于搜索后的快速打开操作
        private bool QuickStart = false;

        //指示是否正在重命名项目
        public bool NowReName = false;

        private List<Item> SearchValue;

        public string KeywordTip = Application.Current.FindResource("VEwndMainKeywordTip") as string;

        public string HotKeyFailed = Application.Current.FindResource("VEwndMainHotKeyFailed") as string;

        public string CloseTip = Application.Current.FindResource("VEwndMainCloseTip") as string;

        public string AllLoadHeader = Application.Current.FindResource("VEwndMainAllLoadHeader") as string;

        public string AllLoadFooter = Application.Current.FindResource("VEwndMainAllLoadFooter") as string;

        private Key Lastkey=Key.None;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public wndMain()
        {
            InitializeComponent();

            //边界颜色定义
            bdrColor = Color.FromArgb(0xff, 0x28, 0x28, 0x28);
            this.bdrMainForm.Background = new SolidColorBrush(bdrColor);
        }

        #endregion


        #region public

        /// <summary>
        /// 清空搜索框
        /// </summary>
        public void ClearSearch()
        {
            this.txtMain.Text = this.KeywordTip;
        }


        #endregion


        #region 窗体事件响应

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wndProgressBar wndpb = new wndProgressBar(AllLoadHeader, AllLoadFooter, 100);

            #region Load language and register hotkey

            //加载语言
            Manage.LoadingLanguage();

            //注册热键
            Manage.WindowMainHandle = new WindowInteropHelper(this).Handle; ;

            HotKeyVisualItem hkvi = new HotKeyVisualItem();

            if (hkvi.OuterGetKeys(AppInfoOperations.GetHotKey()))
            {
                if (!HotKey.TestHotKey(3, System.Windows.Forms.Keys.D1, HotKey.QUICK_SEARCH_HOTKEY_ID, false))
                {
                    tipMainForm.ShowFixed(this, this.HotKeyFailed);
                }
            }
            else
            {
                if (!HotKey.TestHotKey(3, System.Windows.Forms.Keys.D1, HotKey.QUICK_SEARCH_HOTKEY_ID, false))
                {
                    tipMainForm.ShowFixed(this, this.HotKeyFailed);
                }
            }

            hkvi = null;

            wndpb.Increase(25);

            #endregion


            #region Initialize Visual Settings
            this.WindowState = WindowState.Normal;
            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double ScreenHeight = SystemParameters.PrimaryScreenHeight;

            //获取位置
            //并检查合法性
            double readLeft = AppInfoOperations.GetLeft();
            double readTop = AppInfoOperations.GetTop();

            if (readLeft > 2)
                this.Left = readLeft - 2;
            else
                this.Left = 0;

            if (readTop > 1)
                this.Top = readTop - 1;
            else
                this.Top = 0;

            //获取宽高
            double readWidth = AppInfoOperations.GetWidth();
            double readHeight = AppInfoOperations.GetHeight();

            //检查数值合法性
            if (readWidth > ScreenWidth) readWidth = ScreenWidth;
            if (readHeight > ScreenHeight) readHeight = ScreenHeight;

            //应用宽高
            this.Width = readWidth;
            this.Height = readHeight;


            Manage.LoadBackground();

            ClearSearch();

            wndpb.Increase(25);

            #endregion


            #region Initialize Background Data

            //用于自动存储位置大小的开关指示
            IsInformationsInitialized = true;

            //初始化后台数据
            Manage.InitializeData();

            wndpb.Increase(25);

            ////初始化删除计时器
            //Manage.timer.Interval = TimeSpan.FromSeconds(3);
            //Manage.timer.Stop();
            //Manage.timer.Tick += Timer_Tick;

            //其他
            MELeave.RoutedEvent = Border.MouseLeaveEvent;
            MEEnter.RoutedEvent = Border.MouseEnterEvent;

            //获取插件
            Class.Plugins.GetPlugins();

            //关联事件处理
            this.StateChanged += new EventHandler(animationInstance.Window_StateChanged);
            this.SizeChanged += new SizeChangedEventHandler(animationInstance.Window_SizeChanged);

            //设置窗体渐隐与显示
            animationInstance.InitBdrStyle(ref this.bdrMain);

            
            //关闭加载窗体
            //Manage.WindowLoading.Close();

            wndpb.Increase(25);

            #endregion

            wndpb.Increase();

        }

        /// <summary>
        /// 位置改变时响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //检查信息是否已初始化
            if (IsInformationsInitialized)
            {
                //保存位置信息
                AppInfoOperations.SetLeft(this.Left);
                AppInfoOperations.SetTop(this.Top);

                //更新Manage类的主窗体位置信息
                Manage.WindowMainRect.left = (int)this.Left;
                Manage.WindowMainRect.right = (int)(this.Left + this.ActualWidth);
                Manage.WindowMainRect.top = (int)this.Top;
                Manage.WindowMainRect.bottom = (int)(this.Top + this.ActualHeight);
            }

        }

        /// <summary>
        /// 防止因意外不能关闭强制措施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //卸载注册的热键
            Manage.UnregisterAllHotKeys();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 主窗体鼠标移动的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_MouseMove(object sender, MouseEventArgs e)
        {

            //检查是否是鼠标左键
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    //移动窗体
                    this.DragMove();
                }
                catch
                {

                }

                //取消冒泡
                e.Handled = true;
            }
            else
            {
                //边缘吸附
                if (this.Left <= 50 && this.Left > 0)
                    this.Left = 0;

                if (this.Top <= 50 && this.Top > 0)
                    this.Top = 0;
            }


        }

        /// <summary>
        /// 当窗体被激活时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_Activated(object sender, EventArgs e)
        {
            //当窗体被激活时无论如何都要正常显示
            this.WindowState = WindowState.Normal;

            //检查是否初始化完成
            if (IsInformationsInitialized)
            {

                //检查不透明度
                if (this.Opacity==0)
                {
                    //直接还原窗体
                    animationInstance.SetNormal(this);
                }
                else
                {
                    //否则只是未最小化而临时隐藏的，所以直接还原透明度
                    CheckHidden();
                }
            }
        }


        #endregion




        #region 控件事件响应

        /// <summary>
        /// 光标进入输入框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Manage.ClearOrFillText(ref this.txtMain, true, this.KeywordTip);
        }

        /// <summary>
        /// 光标离开输入框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseLeave(object sender, MouseEventArgs e)
        {
            Manage.ClearOrFillText(ref this.txtMain, false, this.KeywordTip);
        }

        /// <summary>
        /// 点击网络搜索按钮时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenSearchWindow(this.txtMain.Text);
        }

        /// <summary>
        /// 清空检索框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.BdrFunction.Style = null;
            Manage.ClearOrFillText(ref this.txtMain, true, this.KeywordTip);
        }

        /// <summary>
        /// 检索框获得焦点的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_GotFocus(object sender, RoutedEventArgs e)
        {
            this.BdrFunction.Style = null;
            Manage.ClearOrFillText(ref this.txtMain, true, this.KeywordTip);

            CheckHidden();
        }

        /// <summary>
        /// 检索框丢失焦点的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_LostFocus(object sender, RoutedEventArgs e)
        {
            PackUp();
        }

        ///// <summary>
        ///// 移除定时器的响应
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    if (Manage.listOfRemoveItem.Count > 0)
        //    {
        //        foreach (Item i in Manage.listOfRemoveItem)
        //        {
        //            Manage.mMAIN.RemoveChild(i.ID);

        //            FileOperation.DeleteFile(Manage.IconPath + i.ID + ".ib");

        //            this.Recent.Children.Remove(i);
        //        }
        //    }


        //    Manage.timer.Stop();
        //}

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.btnCloseOnceClick++;
            if (this.btnCloseOnceClick >= 2)
            {
                foreach (ItemData itemdata in Manage.listOfInnerData)
                {
                    itemdata.SaveIcon();
                }
                tipMainForm.Close();
                animationInstance.Close(this);
            }
            else
                tipMainForm.ShowFixed(this, this.CloseTip,5,5);
        }

        /// <summary>
        /// 最大化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                animationInstance.SetMax(this);
            }
            else
            {
                animationInstance.SetMax(this, 1);
            }

        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            animationInstance.SetMin(this);
        }

        /// <summary>
        /// 关闭按钮离开处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnCloseOnceClick = 0;
            tipMainForm.HideMe();
        }

        /// <summary>
        /// 当拖放文件进入区域时的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 当拖放文件拖放时的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMainForm_Drop(object sender, DragEventArgs e)
        {
            //获取拖放的文件
            String[] arr = (String[])e.Data.GetData(DataFormats.FileDrop);

            wndPreSet wps = new wndPreSet(arr);

            wps.ShowDialog();

        }

        /// <summary>
        /// 检索框内容改变时的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInformationsInitialized)
            {

                string str = this.txtMain.Text.Trim();
                if (!string.IsNullOrEmpty(str) && str != this.KeywordTip)
                {
                    this.btnSearch.Visibility = Visibility.Visible;

                    foreach (object obj in this.Recent.Children)
                    {
                        if (obj is ExpanderEx)
                        {
                            ((ExpanderEx)obj).Visibility = Visibility.Collapsed;
                            foreach (Item i in (((ExpanderEx)obj).Content as WrapPanel).Children)
                            {
                                i.Hide();
                            }
                        }
                        else if (obj is Item)
                        {
                            ((Item)obj).Hide();
                        }
                    }

                    str = str.EndsWith(":Path") ? str.Substring(0, str.Length - 4) : str;

                    str = str.EndsWith(":Tag Name") ? str.Substring(0, str.Length - 8) : str;

                    SearchValue = Manage.MultiSearch(str);


                    if (SearchValue != null)
                    {

                        if (SearchValue.Count <= 3)
                            QuickStart = true;
                        else
                            QuickStart = false;


                        foreach (Item i in SearchValue)
                        {
                            if (i.Parent is WrapPanel)
                            {
                                WrapPanel wpTmp = (WrapPanel)i.Parent;
                                if (wpTmp.Parent is ExpanderEx)
                                {
                                    ((ExpanderEx)wpTmp.Parent).IsExpanded = true;
                                    ((ExpanderEx)wpTmp.Parent).Visibility = Visibility.Visible;
                                }
                            }
                            i.Show();
                        }
                    }

                }
                else
                {
                    this.btnSearch.Visibility = Visibility.Collapsed;
                    foreach (object obj in this.Recent.Children)
                    {
                        if (obj is ExpanderEx)
                        {
                            ((ExpanderEx)obj).Visibility = Visibility.Visible;
                            WrapPanel wp = (WrapPanel)((ExpanderEx)obj).Content;

                            if (wp.Children.Count > 0)
                            {
                                foreach (Item i in wp.Children)
                                {
                                    i.Show();
                                }
                            }
                        }
                        else
                        {
                            if (obj is Item)
                            {
                                (obj as Item).Show();
                            }
                        }

                    }
                }
                if (Lastkey==Key.Oem1)
                {
                    if (this.txtMain.Text.EndsWith(":"))
                    {
                        string bit = "";
                        short count = 0;
                        for (int i = 0; i < this.txtMain.Text.Length; i++)
                        {
                            bit = this.txtMain.Text.Substring(i, 1);
                            if (bit == ":")
                            {
                                count++;
                            }
                        }
                        if (count == 1)
                        {
                            this.txtMain.Text += "Path";
                            this.txtMain.Select(this.txtMain.Text.Length - 4, 4);
                        }
                        else if (count == 2)
                        {
                            this.txtMain.Text += "Tag Name";
                            this.txtMain.Select(this.txtMain.Text.Length - 8, 8);
                        }
                        Lastkey = Key.None;
                    }
                }
                
            }
        }

        /// <summary>
        /// 检索框的鼠标移动的消息响应，用于取消事件冒泡，以防止鼠标选择文字时窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 内容按键的消息响应，设置检索框焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrlist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Left && e.Key != Key.Right)
            {
                SetFocus();
            }

            //e.Handled = true;
        }

        /// <summary>
        /// 主窗体按键时的消息响应，同上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                SetFocus();
        }

        /// <summary>
        /// 快速打开搜索后的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_KeyDown(object sender, KeyEventArgs e)
        {
            this.Lastkey = e.Key;
            //只按下了Enter
            if (e.Key == Key.Enter && !((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                if (QuickStart)
                {
                    SearchValue[0].refItemData.Execute();
                    Manage.SaveKeyword(this.txtMain.Text);
                    this.ClearSearch();
                }
            }

            //按下了Ctrl+ Enter
            else if (e.Key == Key.Enter && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                Manage.OpenSearchWindow(this.txtMain.Text);
            }

            //按下了Ctrl+F
            else if (e.Key == Key.F && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                SearchValue[0].refItemData.FindLocation();
                Manage.SaveKeyword(this.txtMain.Text);
                this.ClearSearch();
            }

            //按下了Alt+C
            else if (e.Key == Key.C && ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                this.ClearSearch();
            }
            else if (e.Key == Key.Tab)
            {

            }
            else
            {
                Manage.ClearOrFillText(ref this.txtMain, false, this.KeywordTip);
            }
        }

        /// <summary>
        /// 离开窗体时收起标题栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMain_MouseLeave(object sender, MouseEventArgs e)
        {
            PackUp();
        }

        #endregion




        #region 菜单项事件响应

        /// <summary>
        /// 展开所有的expander
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.ExpanderExOperation();
        }

        /// <summary>
        /// 收起所有的expander
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetractAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.ExpanderExOperation(false);
        }

        /// <summary>
        /// 打开设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            wndSettings ws = new wndSettings();
            ws.Show();
        }

        /// <summary>
        /// 从菜单项进入网络搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchWebMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenSearchWindow(this.txtMain.Text);
        }

        /// <summary>
        /// 打开手动添加窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenAddWindow();
        }

        /// <summary>
        /// 添加我的电脑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddComputerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddComputer());
        }

        /// <summary>
        /// 添加我的文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMyDocumentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddMyDocument());
        }

        /// <summary>
        /// 添加回收站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRecycleBinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddRecycleBin());
        }

        /// <summary>
        /// 添加控制面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddControlPanelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddControlPanel());
        }

        /// <summary>
        /// 添加网络邻居
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNetworkNeighborhoodMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddNetworkNeighbor());
        }
        #endregion




        #region 私有

        /// <summary>
        /// 收起标题栏
        /// </summary>
        private void PackUp()
        {
            if (IsInformationsInitialized)
            {
                this.BdrFunction.Style = this.FindResource("BdrFunctionStyle") as Style;
                Manage.ClearOrFillText(ref this.txtMain, false, this.KeywordTip);
                MELeave.RoutedEvent = Mouse.MouseLeaveEvent;
                this.BdrFunction.RaiseEvent(MELeave);

                this.Topmost = false;
            }
        }

        /// <summary>
        /// 设置焦点到检索框
        /// </summary>
        private void SetFocus()
        {
            if (!NowReName)
            {
                this.BdrFunction.Style = null;
                CheckHidden();
                Manage.ClearOrFillText(ref this.txtMain, true, this.KeywordTip);
                if (Keyboard.FocusedElement != this.txtMain)
                {
                    this.txtMain.Focus();

                    Animation.UniversalBeginDoubleAnimation<Border>(ref this.BdrFunction, Border.HeightProperty, 0.2, 70, this.BdrFunction.ActualHeight);
                    Animation.UniversalBeginDoubleAnimation<Border>(ref this.BdrFunction, Border.OpacityProperty, 0.2, 1, this.BdrFunction.Opacity);
                }
            }
        }

        /// <summary>
        /// 检查窗体是否隐藏
        /// </summary>
        private void CheckHidden()
        {
            if (this.bdrMain.Opacity < AppInfoOperations.GetMaxOpacity())
            {
                Animation.UniversalBeginDoubleAnimation<Border>(ref this.bdrMain, OpacityProperty, 0.2, AppInfoOperations.GetMaxOpacity());
            }
        }







        #endregion

        private void MW_GotFocus(object sender, RoutedEventArgs e)
        {
            //检查是否初始化完成
            if (IsInformationsInitialized)
            {

                //检查不透明度
                if (this.Opacity == 0)
                {
                    //直接还原窗体
                    animationInstance.SetNormal(this);
                }
                else
                {
                    //否则只是未最小化而临时隐藏的，所以直接还原透明度
                    CheckHidden();
                }
            }
        }
    }
}
