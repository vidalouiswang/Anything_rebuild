using System.IO;
using System.Windows;
using System.Windows.Media;
using Anything.Class;
using Anything.UserControls;

namespace Anything.Form
{
    /// <summary>
    /// wndItemInformation.xaml 的交互逻辑
    /// </summary>
    public partial class wndItemInformation : Window
    {

        private Item item = null;
        private ItemData itemdata = null;

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(wndItemInformation), new PropertyMetadata((string)""));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(wndItemInformation), new PropertyMetadata((string)""));

        public string Arguments
        {
            get { return (string)GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string), typeof(wndItemInformation), new PropertyMetadata((string)""));


        public string TagName
        {
            get { return (string)GetValue(TagNameProperty); }
            set { SetValue(TagNameProperty, value); }
        }
        public static readonly DependencyProperty TagNameProperty =
            DependencyProperty.Register("TagName", typeof(string), typeof(wndItemInformation), new PropertyMetadata((string)""));

        public ImageSource ItemIcon
        {
            get { return (ImageSource)GetValue(ItemIconProperty); }
            set { SetValue(ItemIconProperty, value); }
        }
        public static readonly DependencyProperty ItemIconProperty =
            DependencyProperty.Register("ItemIcon", typeof(ImageSource), typeof(wndItemInformation), new PropertyMetadata(new System.Windows.Media.Imaging.BitmapImage()));


        public string WorkingDirectory
        {
            get { return (string)GetValue(WorkingDirectoryProperty); }
            set { SetValue(WorkingDirectoryProperty, value); }
        }
        public static readonly DependencyProperty WorkingDirectoryProperty =
            DependencyProperty.Register("WorkingDirectory", typeof(string), typeof(wndItemInformation), new PropertyMetadata((string)""));


        public ItemData Itemdata
        {
            get
            {
                return itemdata;
            }

            set
            {
                itemdata = value;
            }
        }

        public Item Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public wndItemInformation()
        {
            InitializeComponent();
            this.vtnMain.GetTags(Manage.WindowMain.Recent);
        }

        public wndItemInformation(Item item)
        {
            InitializeComponent();

            //填充信息
            this.Item = item;
            this.Itemdata = item.refItemData;
            this.ItemName = this.Itemdata.Name;
            this.Path = this.Itemdata.Path;
            this.Arguments = this.Itemdata.Arguments;
            this.ItemIcon = GetIcon.ByteArrayToIS(this.Itemdata.Icon);
            this.WorkingDirectory = this.Itemdata.WorkingDirectory;
            this.TagName = this.Itemdata.TagName;
            this.txtHotKey.InputKeyString(this.Itemdata.HotKey);



            this.vtnMain.GetTags(Manage.WindowMain.Recent);

            this.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();

        }


        private void SaveSettings()
        {
            if (!string.IsNullOrEmpty(this.ItemName))
                itemdata.Name = this.ItemName;

            if (!string.IsNullOrEmpty(this.Path))
                itemdata.Path = this.Path;

            if (!string.IsNullOrEmpty(this.Arguments))
                itemdata.Arguments = this.Arguments;

            if (!string.IsNullOrEmpty(this.WorkingDirectory))
                itemdata.WorkingDirectory = this.WorkingDirectory;

            if (this.ItemIcon != null && this.itemdata.IconChanged)
            {
                itemdata.Icon = GetIcon.ImageSourceToByteArray(this.ItemIcon);
            }

            
            itemdata.TagName = TagName;
            

            if (this.txtHotKey.Available)
            {
                this.itemdata.AddHotKey(this.txtHotKey);
            }
            else
            {
                this.itemdata.RemoveHotKey();
            }

            Manage.RefreshSingle(item, itemdata);

            this.Close();
        }
        private void btnChangeIcon_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "PNG文件|*.png";
            ofd.ShowDialog();


            string FileName = ofd.FileName;
            if (System.IO.File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);

                byte[] b = null;
                fs.Position = 0;
                using (BinaryReader br = new BinaryReader(fs))
                {
                    b = br.ReadBytes((int)fs.Length);
                }

                this.imgIcon.Source = GetIcon.ByteArrayToIS(b);
                itemdata.IconChanged = true;
            }
            else
            {
                Manage.TipPublic.ShowFixed(this, "File doesn't exists.");
            }
        }

        private void Border_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Enter)
            {
                SaveSettings();
                e.Handled = true;
            }
        }

        private void vtnMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.txtTagName.Text = this.vtnMain.TagNameSelected;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton==System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
