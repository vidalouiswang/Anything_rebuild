using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Anything.Class
{

    #region 背景类
    public class ControlBackground
    {
        #region 成员变量
        //存放图片的路径
        private string image_path = "";

        //目标对象
        private Packer obj = null;

        //自动应用标识
        //private bool AutoApply = true;

        //计时器时间间隔，毫秒
        private long Interval = 0;

        //计时器
        private System.Windows.Threading.DispatcherTimer timer = null;

        //是否使用动画过渡
        private bool using_animation = true;

        private bool Multi = false;

        //存放画刷资源的数组
        private List<ImageBrush> ibs = new List<ImageBrush>();

        //当前索引
        private int current = 0;

        //指示当前背景对象是否可用的标识
        public bool Available = false;

        //用于标识上次使用的画刷和本次将要使用的画刷
        private ImageBrush ibCurrent = null;
        private ImageBrush ibLast = null;

        //切换画刷使得过度效果
        private DoubleAnimation daShow = new DoubleAnimation(1,TimeSpan.FromMilliseconds(ApplicationInformations.Anything.AppInfoOperations.GetBackgroundAnimationIntervalMilliseconds()));
        private DoubleAnimation daHide = new DoubleAnimation(0.5, TimeSpan.FromMilliseconds(ApplicationInformations.Anything.AppInfoOperations.GetBackgroundAnimationIntervalMilliseconds()));

        #endregion

        #region 构造函数
        public ControlBackground(string ImagePath,Packer obj=null,long Interval=0,bool UsingAnimation=true)
        {
            //赋值图片路径
            this.image_path = ImagePath;

            //加载图片到ibs
            LoadResources();

            //检查可用性
            if (this.Available)
            {
                //赋值应用对象
                this.obj = obj;

                //赋值时间间隔
                this.Interval = Interval;

                //初始化计时器
                InitTimer();

                //赋值是否使用计时器标识
                this.using_animation = UsingAnimation;

                if (this.using_animation)
                {
                    this.daHide.Completed += DaHide_Completed;
                }
            }
        }

        
        #endregion

        #region 外部获取画刷资源
        public ImageBrush Get()
        {

            return null;
        }
        #endregion

        #region 索引检查
        private void RangeCheck()
        {
            if (this.current>=this.ibs.Count)
            {
                this.current = 0;
            }
        }
        #endregion

        #region 切换图片
        private void ChangeImage()
        {
            this.ibLast = this.ibCurrent;
            this.ibCurrent = this.ibs[this.current++];
            RangeCheck();

        }
        #endregion

        #region 加载图片资源
        private void LoadResources()
        {
            //检查目录是否存在
            if (Directory.Exists(this.image_path))
            {

                //获取文件
                string[] files = Directory.GetFiles(this.image_path);

                //检查文件数量
                if (files.Length>0)
                {
                    //指示可用
                    this.Available = true;

                    //便利文件数组加载图片
                    foreach (string img in files)
                    {
                        try
                        {
                            BitmapImage bi = new BitmapImage(new Uri(img, UriKind.Absolute));

                            bi.CacheOption = BitmapCacheOption.OnLoad;

                            ImageBrush ib = new ImageBrush(bi);

                            //透明度改为零
                            ib.Opacity = 0.5;

                            //添加到成员变量数组
                            this.ibs.Add(ib);
                        }
                        catch
                        {

                        }
                    }
                    this.ibLast = this.ibs[this.current++];
                    RangeCheck();
                    this.ibCurrent = this.ibs[this.current++];
                    RangeCheck();
                }

                if (files.Length >= 2)
                    this.Multi = true;

            }
        }
        #endregion

        #region 初始化计时器
        private void InitTimer()
        {
            if (Interval > 0 && this.Multi)
            {
                this.timer = new System.Windows.Threading.DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(this.Interval);
                this.timer.Tick += Timer_Tick;
                this.obj.Background(this.ibLast);
                this.ibLast.BeginAnimation(ImageBrush.OpacityProperty, this.daShow);
                this.timer.Start();
            }
        }

        #region 计时器响应函数
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Available)
            {
                if (this.using_animation)
                {
                    ChangeUsingAnimation();
                }
                else
                {
                    ChangeUsingAnimation();
                }
            }
        }
        #endregion

        #endregion

        #region 使用动画过度更改

        private void ChangeUsingAnimation()
        {
            this.ibLast.BeginAnimation(ImageBrush.OpacityProperty, this.daHide);
        }

        #endregion

        #region 动画结束事件响应

        private void DaHide_Completed(object sender, EventArgs e)
        {
            this.ibLast.BeginAnimation(ImageBrush.OpacityProperty, null);

            this.obj.Background(this.ibCurrent);

            this.ibCurrent.BeginAnimation(ImageBrush.OpacityProperty, this.daShow);

            ChangeImage();
        }
        #endregion
    }
    #endregion

    #region 包装类
    public class Packer : DependencyObject
    {
        private Window wnd = null;
        private Border bdr = null;
        private Button btn = null;
        private TextBlock tb = null;
        private TextBox txt = null;



        public Packer(Window wnd)
        {
            this.wnd = wnd;
        }
        public Packer(Border bdr)
        {
            this.bdr = bdr;
        }
        public Packer(Button btn)
        {
            this.btn = btn;
        }
        public Packer(TextBlock tb)
        {
            this.tb = tb;
        }
        public Packer(TextBox txt)
        {
            this.txt = txt;
        }



        public void Background(Brush brush)
        {
            if (this.wnd!=null)
            {
                this.wnd.Background = brush;
            }
            else if (this.bdr!=null)
            {
                this.bdr.Background = brush;
            }
            else if (this.btn!=null)
            {
                this.btn.Background = brush;
            }
            else if (this.tb!=null)
            {
                this.tb.Background = brush;
            }
            else if (this.txt!=null)
            {
                this.txt.Background = brush;
            }
        }

        

    }
    #endregion
}
