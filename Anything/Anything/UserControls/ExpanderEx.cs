namespace Anything.UserControls
{
    class ExpanderEx : System.Windows.Controls.Expander
    {
        public ExpanderEx(string TagName)
        {
            this.tagName = TagName;
            this.Header = tagName;

            this.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            this.FontSize = 16;
            this.IsExpanded = true;
            this.Content = new System.Windows.Controls.WrapPanel();
            this.Margin = new System.Windows.Thickness(3);
            this.Focusable = false;
        }

        public string tagName { get; set; } = "";
    }
}
