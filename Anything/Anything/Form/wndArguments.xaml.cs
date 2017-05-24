using System.Windows;
using System.Windows.Input;
using Anything.Class;

namespace Anything.Form
{
    /// <summary>
    /// wndArguments.xaml 的交互逻辑
    /// </summary>
    public partial class wndArguments : Window
    {

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(wndArguments), new PropertyMetadata((string)"Name"));

        public string Arguments
        {
            get { return (string)GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string), typeof(wndArguments), new PropertyMetadata((string)""));

        private ItemData it = null;
        public ItemData It
        {
            get
            {
                return it;
            }

            set
            {
                it = value;
            }
        }

        public wndArguments()
        {
            InitializeComponent();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (It != null)
            {
                it.Arguments = Arguments;
                this.Close();
            }
        }
    }
}
