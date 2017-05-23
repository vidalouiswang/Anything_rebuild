using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Anything.Class;
using Anything.UserControls;

namespace Anything.Form
{
    /// <summary>
    /// wndPreSet.xaml 的交互逻辑
    /// </summary>
    public partial class wndPreSet : Window
    {       
        public wndPreSet()
        {
            InitializeComponent();
            this.vtnMain.GetTags(Manage.WindowMain.Recent);
            //GetTags();
            //ShowTags();
        }

        public wndPreSet(string[] arr)
        {
            InitializeComponent();
            this.arrFiles = arr;

            this.vtnMain.GetTags(Manage.WindowMain.Recent);
            //GetTags();
            //ShowTags();
        }

        private string[] arrFiles;

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            this.txtTagName.Text = btn.Content.ToString();
            this.tagName = this.txtTagName.Text;
        }

        private List<String> arrTags = new List<string>();

        public string tagName
        {
            get { return (string)GetValue(tagNameProperty); }
            set { SetValue(tagNameProperty, value); }
        }

        public static readonly DependencyProperty tagNameProperty =
            DependencyProperty.Register("tagName", typeof(string), typeof(wndPreSet), new PropertyMetadata(""));




        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }


        private void Add()
        {
            wndProgressBar wpb = new wndProgressBar("Add Items...", "Please Wait", arrFiles.Length);

            Item item;
            //添加项目
            foreach (string s in this.arrFiles)
            {
                item = Manage.AddItem(s, null, null, this.tagName);

                if (item!=null)
                {
                    Manage.FindAndInsert(item);
                    
                }
                wpb.Increase();
            }
            wpb.Increase();

        }

        private void btnAdd_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Add();
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            HotKey.SetForegroundWindow(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            this.txtTagName.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.btnAdd.Focus();
                this.Hide();
                Add();
                this.Close();
            }
        }


        private void vtnMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.tagName = this.vtnMain.TagNameSelected;

        }
    }
}
