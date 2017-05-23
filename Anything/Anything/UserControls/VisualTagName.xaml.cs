using System.Collections.Generic;
using System.Windows.Controls;

namespace Anything.UserControls
{
    /// <summary>
    /// VisualTagName.xaml 的交互逻辑
    /// </summary>
    public partial class VisualTagName : UserControl
    {
        public VisualTagName()
        {
            InitializeComponent();
        }

        private List<string> tags = new List<string>();
        private int count = 0;
        private string tagNameSelected = "";

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }

        public List<string> TagNames
        {
            get
            {
                return tags;
            }

            set
            {
                tags = value;
            }
        }

        public string TagNameSelected
        {
            get
            {
                return tagNameSelected;
            }

            set
            {
                tagNameSelected = value;
            }
        }

        public void GetTags(WrapPanel wp)
        {
            if (wp.Children.Count > 0)
            {
                this.Count = wp.Children.Count;

                string tag = "";

                foreach (object obj in wp.Children)
                {
                    if (obj is ExpanderEx)
                    {
                        tag = ((ExpanderEx)obj).tagName;

                        if (!string.IsNullOrEmpty(tag))
                        {
                            if (!this.tags.Contains(tag))
                            {
                                this.tags.Add(tag);
                            }
                        }
                    }
                }
            }
            ShowTags();
        }

        private void ShowTags()
        {
            if (this.tags.Count>0)
            {
                foreach (string s in this.tags)
                {
                    Button btn = new Button();
                    btn.Style = FindResource("NormalButtonStyle") as System.Windows.Style;
                    btn.Content = s;
                    btn.Click += Btn_Click;
                    this.Tags.Children.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            this.tagNameSelected = btn.Content.ToString();
            System.Windows.Input.MouseButtonEventArgs r = new System.Windows.Input.MouseButtonEventArgs(System.Windows.Input.Mouse.PrimaryDevice,0,System.Windows.Input.MouseButton.Left);
            r.RoutedEvent = MouseDoubleClickEvent;
            r.Source = sender;
            this.RaiseEvent(r);
        }


    }
}
