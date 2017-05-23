﻿using System;
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
    public partial class Item : UserControl, IDisposable
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
        public Item(String ID, String Name, ImageSource IS, double Length = 128, string TagName = "")
        {
            InitializeComponent();
            Init(ID, Name, IS, Length, TagName);

        }

        /// <summary>
        /// 初始化过程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="IS"></param>
        private void Init(String ID, String Name = "Default Name", ImageSource IS = null, double Length = 128, string TagName = "")
        {
            this.Name_Property = Name;
            this.ID = ID;
            this.Img_Property = IS;
            this.Length = Length;
            this.TagName = TagName;
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
        //项目的可视化资源
        private ImageSource img_Property = null;

        //项目名称属性
        private String name_Property = "";

        //项目的唯一标识符
        private String iD = "";

        //路径
        private string _Path = "";

        //边长
        private double length = 0;

        //旧名称，用于比对是否改变了名称
        private string OldName = "";

        //后台数据对象的引用
        public ItemData refItemData = null;
        //private bool IsMouseDown

        private DateTime dtZero = Convert.ToDateTime("00:00:00 1/1/2000");
        private DateTime dtDown = Convert.ToDateTime("00:00:00 1/1/2000");

        private bool IsInTxt = false;
        private DispatcherTimer timer;

        public bool IsOut { get; set; } = false;


        //热度，暂时不用
        //public int Levels
        //{
        //    get { return (int)GetValue(LevelsProperty); }
        //    set { SetValue(LevelsProperty, value); }
        //}
        //public static readonly DependencyProperty LevelsProperty =
        //    DependencyProperty.Register("Levels", typeof(int), typeof(Item), new PropertyMetadata(0));

        private string tagName = "";

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

        /// <summary>
        /// Img_Property属性的包装器
        /// </summary>
        public ImageSource Img_Property
        {
            get
            {
                return img_Property;
            }

            set
            {
                img_Property = value;
                this.Img.Source = this.img_Property;

            }
        }

        /// <summary>
        /// ID属性的包装器
        /// </summary>
        public string ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

        /// <summary>
        /// Name属性的包装器
        /// </summary>
        public string Name_Property
        {
            get
            {
                return name_Property;
            }

            set
            {
                name_Property = value;
                this.Txt.Text = value;
                this.TxtWrite.Text = value;
            }
        }

        /// <summary>
        /// 边长属性的包装器
        /// </summary>
        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                this.Width = value;
                this.Height = value;
            }
        }

        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                _Path = value;
            }
        }

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

        public string TagName
        {
            get
            {
                return tagName;
            }

            set
            {
                tagName = value;
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
            this.Name_Property = this.TxtWrite.Text;
            this.TxtWrite.Visibility = Visibility.Hidden;
            this.Txt.Visibility = Visibility.Visible;

            if (!String.IsNullOrEmpty(this.TxtWrite.Text) && !String.IsNullOrEmpty(this.iD))
            {
                if (OldName != Name_Property)
                {
                    string path = this.RefItemData.Path;
                    this.RefItemData.Rename = true;
                    this.RefItemData.Name = this.TxtWrite.Text;

                    this.ID = ClsMD5.ClsMD5.Encrypt(this.name_Property + path);

                }
            }
            Manage.WindowMain.NowReName = false;

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

        /// <summary>
        /// 销毁数据
        /// </summary>
        public void Dispose()
        {
            this.iD = null;
            this.img_Property = null;
            this.name_Property = null;
            this.length = 0;
            GC.Collect();
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
            Manage.mMAIN.RemoveChild(this.ID);

            Manage.listOfInnerData.Remove(this.refItemData);

            FileOperation.DeleteFile(Manage.IconPath + this.ID + ".ib");

            foreach (object obj in Manage.WindowMain.Recent.Children)
            {
                if (obj is Expander)
                {
                    WrapPanel wp = (WrapPanel)((Expander)obj).Content;

                    foreach (Item item in wp.Children)
                    {
                        if (item.iD==this.iD)
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
                        if (item.iD==this.iD)
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

                drag.Width = this.length;
                drag.Height = this.length;

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
    }
}
