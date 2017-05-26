using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Anything.Class;
using Anything.Form;

namespace Anything.UserControls
{
    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl
    {
        #region 构造函数

        /// <summary>
        /// 无参构造
        /// </summary>
        public Item()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="IS"></param>
        public Item(ItemData idRef)
        {
            InitializeComponent();
            this.refItemData = idRef;
            this.iLength = Manage.ItemLength;
            this.Txt.Text = idRef.Name;
            this.TxtWrite.Text = idRef.Name;
            this.ImgSrc = idRef.Icon;
            this.Margin = new Thickness(5);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (IsInTxt)
            {
                Manage.tipForItem.ShowFixed(Manage.WindowMain, this.Txt.Text);
            }
            this.timer.Stop();
        }

        #endregion

        #region 成员变量
        //边长




        public double iLength
        {
            get { return (double)GetValue(iLengthProperty); }
            set { SetValue(iLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iLengthProperty =
            DependencyProperty.Register("iLength", typeof(double), typeof(Item), new PropertyMetadata((double)128));





        //旧名称，用于比对是否改变了名称
        private string OldName = "";

        //后台数据对象的引用
        public ItemData refItemData = null;
        //private bool IsMouseDown

        private DateTime dtZero = Convert.ToDateTime("00:00:00 1/1/2000");
        private DateTime dtDown = Convert.ToDateTime("00:00:00 1/1/2000");

        private bool IsInTxt = false;
        private DispatcherTimer timer;

        public byte[] ImgSrc
        {
            get { return (byte[])GetValue(ImgSrcProperty); }
            set { SetValue(ImgSrcProperty, value); }
        }
        public static readonly DependencyProperty ImgSrcProperty =
            DependencyProperty.Register("ImgSrc", typeof(byte[]), typeof(Item), new PropertyMetadata(null));

        public bool IsOut { get; set; } = false;

        #endregion 

        #region 事件

        #region 单击
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Item));

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(Item.ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(Item.ClickEvent, value);
            }
        }
        #endregion

        #endregion

        #region 属性

        public ItemData RefItemData
        {
            get
            {
                return refItemData;
            }

            set
            {
                refItemData = value;
            }
        }

        #endregion

        #region public
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        #endregion

        #region 事件响应

        /// <summary>
        /// 将项目移出主窗体体时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ME_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.dtDown!=this.dtZero && !IsOut && (DateTime.Now-dtDown).TotalMilliseconds>200)
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    MoveOut(true);
                }
                e.Handled = true;
            }
            
        }

        /// <summary>
        /// 取消冒泡已防止冒泡到parent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWrite_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 鼠标按下响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Focus();
            e.Handled = true;
        }

        /// <summary>
        /// 双击响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseExecute(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RoutedEventArgs ee;
            ee = new RoutedEventArgs(Item.ClickEvent, this);
            base.RaiseEvent(ee);

        }

        /// <summary>
        /// 命名项目
        /// </summary>
        private void SetName()
        {
            Manage.WindowMain.NowReName = true;
            OldName = Txt.Text;
            this.Txt.Visibility = Visibility.Hidden;
            this.TxtWrite.Visibility = Visibility.Visible;
            this.TxtWrite.Focus();
        }

        /// <summary>
        /// 命名完成
        /// </summary>
        private void DoneName()
        {

            string tmpName = this.TxtWrite.Text.Trim();

            if (!string.IsNullOrEmpty(tmpName) && OldName!=tmpName)
            {
                this.refItemData.Name = tmpName;
            }

            Manage.WindowMain.NowReName = false;


            this.TxtWrite.Visibility = Visibility.Hidden;
            this.Txt.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 丢失焦点时保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            DoneName();
        }

        #endregion

        #region 菜单响应

        /// <summary>
        /// 打开参数窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArgumentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenArgumentsWindow(this.RefItemData);
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.RefItemData.Execute();
        }

        /// <summary>
        /// 管理员权限打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.RefItemData.Execute(1, true);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.mMAIN.RemoveChild(this.refItemData.ID);

            Manage.listOfInnerData.Remove(this.refItemData);

            FileOperation.DeleteFile(Manage.IconPath + this.refItemData.ID + ".ib");

            foreach (object obj in Manage.WindowMain.Recent.Children)
            {
                if (obj is Expander)
                {
                    WrapPanel wp = (WrapPanel)((Expander)obj).Content;

                    foreach (Item item in wp.Children)
                    {
                        if (item.refItemData.ID==this.refItemData.ID)
                        {
                            wp.Children.Remove(item);
                            break;
                        }
                    }
                }
                else
                {
                    if (obj is Item)
                    {
                        Item item = (Item)obj;
                        if (item.refItemData.ID==this.refItemData.ID)
                        {
                            Manage.WindowMain.Recent.Children.Remove(item);
                        }
                    }
                }
            }

            Manage.FindEmptyExpander();

        }

        /// <summary>
        /// 重命名项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SetName();
        }


        /// <summary>
        /// 查找位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.RefItemData.FindLocation();
        }

        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateShortcutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.RefItemData.CreateShortcut();
        }

        /// <summary>
        /// 移出项目的菜单项响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveOutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MoveOut();
        }

        /// <summary>
        /// 查看属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttributeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenAttributeWindow(this);
        }

        #endregion

        /// <summary>
        /// 将项目移出主窗体独立出去
        /// </summary>
        private void MoveOut(bool move=false)
        {
            if (!this.IsOut)
            {
                wndDrag drag = new wndDrag();

                if (this.Parent is WrapPanel)
                {
                    drag.IParent = this.Parent as WrapPanel;
                    (this.Parent as WrapPanel).Children.Remove(this);
                }

                this.Bdr.Style = this.FindResource("BdrStyleOut") as Style;

                drag.InnerObj = this;

                drag.Width = this.iLength;
                drag.Height = this.iLength;

                if (move)
                {
                    drag.Left = Mouse.GetPosition((IInputElement)Manage.WindowMain).X+Manage.WindowMain.Left-Math.Round((this.ActualWidth/2),0);
                    drag.Top = Mouse.GetPosition((IInputElement)Manage.WindowMain).Y+Manage.WindowMain.Top- Math.Round((this.ActualHeight / 2), 0);
                }
                else
                {
                    drag.Left = 0;
                    drag.Top = 0;
                }
                
                IsOut = true;
                drag.Show();
            }

        }

        private void Me_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                this.refItemData.Execute();

        }

        private void TxtWrite_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoneName();
                e.Handled = true;
            }
        }

        private void TxtWrite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                e.Handled = true;
        }

        private void ME_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TimeSpan ts = new TimeSpan();
            ts = (DateTime.Now - this.dtDown);

            if (ts.TotalMilliseconds<=200)
            {
                BaseExecute(null,null);
            }
            this.dtDown = this.dtZero;
            e.Handled = true;
        }

        private void ME_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.dtDown = DateTime.Now;
            e.Handled = true;

        }

        private void Txt_MouseEnter(object sender, MouseEventArgs e)
        {
            this.IsInTxt = true;
            if (this.timer==null)
            {
                timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Tick += Timer_Tick;
            }
            this.timer.Start();
            e.Handled = true;
        }

        private void Txt_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IsInTxt = false;
            Manage.tipForItem.HideMe();
        }

        private void ME_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = this.iLength;
            this.Height = this.iLength;
        }
    }
    public class cIcon : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetIcon.ByteArrayToIS((byte[])value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
