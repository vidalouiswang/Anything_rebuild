using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Anything.Class;
using Anything.UserControls;
using ApplicationInformations.Anything;

namespace Anything.Form
{
    /// <summary>
    /// wndSettings.xaml 的交互逻辑
    /// </summary>
    public partial class wndSettings : Window
    {

        /// <summary>
        /// 最大不透明度
        /// </summary>
        public double MaxOpacity
        {
            get { return (double)GetValue(MaxOpacityProperty); }
            set { SetValue(MaxOpacityProperty, value); }
        }
        public static readonly DependencyProperty MaxOpacityProperty =
            DependencyProperty.Register("MaxOpacity", typeof(double), typeof(wndSettings), new PropertyMetadata((double)1.0, PropertyChanged));





        /// <summary>
        /// 最小不透明度
        /// </summary>
        public double MinOpacity
        {
            get { return (double)GetValue(MinOpacityProperty); }
            set { SetValue(MinOpacityProperty, value); }
        }
        public static readonly DependencyProperty MinOpacityProperty =
            DependencyProperty.Register("MinOpacity", typeof(double), typeof(wndSettings), new PropertyMetadata((double)0.1, PropertyChanged));





        /// <summary>
        /// 淡入时长
        /// </summary>
        public double Fadein
        {
            get { return (double)GetValue(FadeinProperty); }
            set { SetValue(FadeinProperty, value); }
        }
        public static readonly DependencyProperty FadeinProperty =
            DependencyProperty.Register("Fadein", typeof(double), typeof(wndSettings), new PropertyMetadata((double)0.3, PropertyChanged));





        /// <summary>
        /// 淡出时长
        /// </summary>
        public double Fadeout
        {
            get { return (double)GetValue(FadeoutProperty); }
            set { SetValue(FadeoutProperty, value); }
        }
        public static readonly DependencyProperty FadeoutProperty =
            DependencyProperty.Register("Fadeout", typeof(double), typeof(wndSettings), new PropertyMetadata((double)0.3, PropertyChanged));



        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }
        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(double), typeof(wndSettings), new PropertyMetadata((double)128.0, PropertyChanged));




        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            wndSettings Base = (wndSettings)sender;

            switch (e.Property.ToString())
            {
                case "MaxOpacity":
                    AppInfoOperations.SetMaxOpacity((double)e.NewValue);
                    break;
                case "MinOpacity":
                    AppInfoOperations.SetMinOpacity((double)e.NewValue);
                    break;
                case "Fadein":
                    AppInfoOperations.SetShowTimeSpan((double)e.NewValue);
                    break;
                case "Fadeout":
                    AppInfoOperations.SetHideTimeSpan((double)e.NewValue);
                    break;
                case "ItemSize":
                    AppInfoOperations.SetItemSize((double)e.NewValue);

                    foreach (object obj in Manage.WindowMain.Recent.Children)
                    {
                        if (obj is ExpanderEx)
                        {
                            WrapPanel tmp = (WrapPanel)((ExpanderEx)obj).Content;
                            foreach (object i in tmp.Children)
                            {
                                if (i is Item)
                                {
                                    ((Item)i).iLength = (double)e.NewValue;
                                }
                            }
                        }
                        else
                        {
                            if (obj is Item)
                            {
                                ((Item)obj).iLength = (double)e.NewValue;
                            }
                        }
                    }

                    break;
                default:
                    break;

            }
        }


        /// <summary>
        /// 构造
        /// </summary>
        public wndSettings()
        {
            LoadData();
            InitializeComponent();

        }

        private void txtOpacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbox = sender as TextBox;
            string src = tbox.Text;
            Match ma = Regex.Match(src, "[0-9\\.]+");
            tbox.Text = ma.ToString();
            tbox = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //MessageBox.Show(exp.Parent.ToString());
        }
        private void LoadData()
        {
            this.MaxOpacity = AppInfoOperations.GetMaxOpacity();
            this.MinOpacity = AppInfoOperations.GetMinOpacity();
            this.Fadein = AppInfoOperations.GetShowTimeSpan();
            this.Fadeout = AppInfoOperations.GetHideTimeSpan();
            this.ItemSize = AppInfoOperations.GetItemSize();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                e.Handled = true;
            }
        }

        private void txt_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void sldr_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manage.mSearchEngine.Insert(this.txtSEName.Text, this.txtSEContent.Text);
            Manage.LoadSearchEngine();
            Manage.TipPublic.ShowFixed(this, "Done", 10, 10);
        }

        private void txtSEName_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSEContent_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void HotKeyVisualItem_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void btnAddHotKey_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
