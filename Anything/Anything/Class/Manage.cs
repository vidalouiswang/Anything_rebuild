﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Anything.Form;
using Anything.UserControls;
using IWshRuntimeLibrary;

namespace Anything.Class
{
    class Manage
    {
        //不允许实例化使用
        private Manage() { }

        #region 成员变量

        #region Path

        //指示当前路径
        public static string CurrentPath = Environment.CurrentDirectory + "\\";

        //图标文件的存放路径
        public static string IconPath = CurrentPath + "icon\\";

        //存放插件的目录
        public static string PluginsPath = CurrentPath + "Plugins\\";

        //存放语言文件的目录
        public static string LanguagePath = CurrentPath + "Language\\";

        #endregion

        #region Anoicess

        //主库，存储其他子库的信息
        public static Anoicess.Anoicess.Anoicess mMAIN = new Anoicess.Anoicess.Anoicess("mData");

        //用于保存近期搜索关键字的库
        public static Anoicess.Anoicess.Anoicess mKeywordRecent = new Anoicess.Anoicess.Anoicess("mKeywordRecent");

        //用于保存搜索引擎的库
        public static Anoicess.Anoicess.Anoicess mSearchEngine = new Anoicess.Anoicess.Anoicess("mSearchEngine");

        //用于保存热键的库
        public static Anoicess.Anoicess.Anoicess mHoyKey = new Anoicess.Anoicess.Anoicess("mHotKey");

        //语言信息
        public static Anoicess.Anoicess.Anoicess mLanguage = new Anoicess.Anoicess.Anoicess("mLanguage");

        #endregion

        #region List

        //数据类对象数据集合
        public static List<ItemData> listOfInnerData = new List<ItemData>();

        //用于保存搜索引擎的内部列表
        public static List<Anoicess.Anoicess.Anoicess._Content> listOfSearchEngineInnerData = new List<Anoicess.Anoicess.Anoicess._Content>();

        //用于保存近期搜索关键字的内部列表
        public static List<string> listOfRecentKeyword = new List<string>();

        ////待删除项目的集合
        //public static List<Item> listOfRemoveItem = new List<Item>();

        //存储搜索引擎可视化资源的集合
        public static List<SearchEngineItem> listOfSearchEnginesVisualElement = new List<SearchEngineItem>();

        //存储应用程序申请的全局热键
        public static List<HotKeyItem> listOfApplicationHotKey = new List<HotKeyItem>();

        public static List<ResourceDictionary> listOfLanguage = new List<ResourceDictionary>();

        #endregion

        #region MO
        //接管信息结构
        public struct ManagedOperation
        {
            public bool IsUsed;
            public string MdlName;
            public ManagedOperation(bool used, string name)
            {
                IsUsed = used;
                MdlName = name;
            }
        }

        //是否接管网络浏览器功能
        public static ManagedOperation MOWeb = new ManagedOperation(false, "");

        //是否接管文件夹浏览功能
        public static ManagedOperation MOFolder = new ManagedOperation(false, "");

        #endregion

        #region lnk
        //Lnk数据结构，仅主要信息
        public struct _Link
        {
            public string Name;
            public string WorkingDirectory;
            public string TargetPath;
            public string Arguments;
        }

        //lnk文件信息
        private static _Link lnkInfo = new _Link();

        #endregion

        #region Window
        //提示窗体
        public static wndTip TipPublic = new wndTip();

        //参数窗体
        public static wndArguments WindowArgs = new wndArguments();

        //手动添加窗体
        public static wndAdd WindowAdd = new wndAdd();

        //主窗体引用
        public static wndMain WindowMain;
        public static IntPtr WindowMainHandle = IntPtr.Zero;


        //加载窗体引用
        public static wndLoading WindowLoading;


        public static wndTip tipForItem = new wndTip();

        #endregion

        #region Others

        ////用于延迟移除项目的timer
        //public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        //用于保存主窗体的位置信息
        public static RECT WindowMainRect = new RECT();

        //匹配网址的正则
        public static Regex reURL = new Regex(@"^((http|https|ftp)://)?\S+?\S+\.\S+.+$", RegexOptions.IgnoreCase);

        //匹配系统引用
        public static Regex reSysRef = new Regex(@"::\{\S+\}");

        public static Regex reKeyword = new Regex(@"([^:]*)?:{1}([^:]*)?:?([^:]*)?");


        public static string LoadingInnerDataHeader = Application.Current.FindResource("VEManageLoadingInnerDataHeader") as string;

        public static string LoadingInnerDataFooter = Application.Current.FindResource("VEManageLoadingInnerDataFooter") as string;


        public static string ItemExists = Application.Current.FindResource("VEManageItemExists") as string;


        public static string DefaultTagName = Application.Current.FindResource("VEManageDefaultTagName") as string;


        public static string rSPLIT = "#SPLIT#";

        public static string rANY = ".*";


        #endregion

        #region System Reference
        //系统引用
        public const string MyComputer = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
        public const string MyDocument = "::{450D8FBA-AD25-11D0-98A8-0800361B1103}";
        public const string ControlPanel = "::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
        public const string RecycleBin = "::{645FF040-5081-101B-9F08-00AA002F954E}";
        public const string NetworkNeighborhood = "::{208D2C60-3AEA-1069-A2D7-08002B30309D}";

        #endregion

        #endregion

        #region public



        public static void LoadBackground()
        {
            string path = "";

            if (System.IO.File.Exists(CurrentPath + "background.jpg"))
            {
                path = CurrentPath + "background.jpg";
            }

            if (System.IO.File.Exists(CurrentPath + "background.png"))
            {
                path = CurrentPath + "background.png";
            }


            if (!string.IsNullOrEmpty(path))
            {

                FileStream fs = new FileStream(path,FileMode.Open);

                BitmapImage bi = new BitmapImage();

                bi.BeginInit();

                bi.StreamSource = fs;

                bi.EndInit();

                ImageBrush ib = new ImageBrush(bi);

                Manage.WindowMain.bdrMainForm.Background = ib;
            }
        }

        public static void ExpanderExOperation(bool Expanded = true)
        {
            if (WindowMain!=null)
            {
                foreach (object obj in WindowMain.Recent.Children)
                {
                    if (obj is ExpanderEx)
                    {
                        ((ExpanderEx)obj).IsExpanded = Expanded;
                    }
                }
            }
        }


        /// <summary>
        /// 根据指定的语言名称切换语言
        /// </summary>
        /// <param name="LanguageName"></param>
        /// <returns></returns>
        public static int ChangeLanguage(string LanguageName)
        {
            if (!string.IsNullOrEmpty(LanguageName))
            {
                //从已加载的语言资源集合中找出指定的资源字典
                ResourceDictionary rdSelected = null;

                foreach (ResourceDictionary rd in listOfLanguage)
                {
                    foreach (string str in rd.Values)
                    {
                        if (str.Contains(LanguageName))
                        {
                            rdSelected = rd;
                            break;
                        }
                    }
                }

                //检查是否已找到对应的语言资源
                if (rdSelected!=null)
                {

                    //定义用于寻找字典集合中的语言字典名字的正则
                    Regex re = new Regex("[a-zA-Z]{2}-[a-zA-Z]{2}");

                    //用于暂存资源字典的变量
                    ResourceDictionary listOfDic = null;

                    //从程序字典集合中寻找
                    foreach (ResourceDictionary item in Application.Current.Resources.MergedDictionaries)
                    {
                        if (re.IsMatch(item.Source.ToString()))
                        {
                            listOfDic = item;
                            break;
                        }
                    }


                    //从字典集合中移除
                    if (listOfDic != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(listOfDic);
                    }


                    //将指定的语言字典添加到资源字典集合
                    Application.Current.Resources.MergedDictionaries.Insert(0, rdSelected);

                    
                    //更新部分非绑定的字符串值
                    WindowMain.KeywordTip = Application.Current.FindResource("VEwndMainKeywordTip") as string;

                    WindowMain.HotKeyFailed = Application.Current.FindResource("VEwndMainHotKeyFailed") as string;

                    WindowMain.CloseTip = Application.Current.FindResource("VEwndMainCloseTip") as string;

                    WindowMain.AllLoadHeader = Application.Current.FindResource("VEwndMainAllLoadHeader") as string;

                    WindowMain.AllLoadFooter = Application.Current.FindResource("VEwndMainAllLoadFooter") as string;

                    LoadingInnerDataHeader = Application.Current.FindResource("VEManageLoadingInnerDataHeader") as string;

                    LoadingInnerDataFooter = Application.Current.FindResource("VEManageLoadingInnerDataFooter") as string;

                    ItemExists = Application.Current.FindResource("VEManageItemExists") as string;

                    DefaultTagName = Application.Current.FindResource("VEManageDefaultTagName") as string;

                    WindowMain.ClearSearch();

                    //存储语言信息选择
                    ApplicationInformations.Anything.AppInfoOperations.SetLanguage(LanguageName);


                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
                return -2;
        }


        /// <summary>
        /// 从语言目录下找到语言资源并加载到内存
        /// </summary>
        public static void LoadingLanguage()
        {

            //从程序字典集合中寻找
            foreach (ResourceDictionary item in Application.Current.Resources.MergedDictionaries)
            {

                //定义用于寻找字典集合中的语言字典名字的正则
                Regex re = new Regex("[a-zA-Z]{2}-[a-zA-Z]{2}");

                //添加默认的English到菜单
                if (re.IsMatch(item.Source.ToString()))
                {
                    listOfLanguage.Add(item);
                }
            }

            if (Directory.Exists(LanguagePath))
            {
                string[] lanFiles = Directory.GetFiles(LanguagePath);
                if (lanFiles.Length > 0)
                {
                    foreach (string str in lanFiles)
                    {
                        ResourceDictionary rd = System.Windows.Markup.XamlReader.Load(new FileStream(str, FileMode.Open)) as ResourceDictionary;

                        rd.Source = new Uri(str, UriKind.Absolute);

                        listOfLanguage.Add(rd);
                    }
                }

                System.Windows.Controls.MenuItem mi;

                foreach (ResourceDictionary rd in listOfLanguage)
                {
                    System.Collections.ICollection values = rd.Values;

                    foreach (string str in values)
                    {
                        if (str.Contains("%LanguageName%:"))
                        {
                            mi = new System.Windows.Controls.MenuItem();

                            mi.Header = str.Replace("%LanguageName%:", "");

                            mi.Click += Mi_Click;

                            WindowMain.Languages.Items.Add(mi);
                        }
                    }

                }

                if (listOfLanguage.Count>0)
                {
                    ChangeLanguage(ApplicationInformations.Anything.AppInfoOperations.GetLanguage());
                }
            }
            else
            {
                Directory.CreateDirectory(LanguagePath);
            }
        }

        private static void Mi_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;

            ChangeLanguage(mi.Header.ToString());
        }

        public static void FindEmptyExpander()
        {
            foreach (object obj in WindowMain.Recent.Children)
            {
                if (obj is ExpanderEx)
                {
                    WrapPanel tmp = (WrapPanel)((ExpanderEx)obj).Content;

                    if (tmp.Children.Count <= 0)
                    {
                        WindowMain.Recent.Children.Remove((ExpanderEx)obj);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 插入item到expander或wrappanel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int FindAndInsert(Item item)
        {
            
            if (!string.IsNullOrEmpty(item.TagName))
            {
                SelectInsert(item);
            }
            else
            {
                item.TagName = DefaultTagName;
                SelectInsert(item);
            }

            FindEmptyExpander();

            return 0;
        }

        private static void SelectInsert(Item item)
        {

            //检查tagName
            bool IsAdded = false;

            foreach (object obj in WindowMain.Recent.Children)
            {
                //查找子元素
                if (obj is ExpanderEx)
                {
                    //获取到expander
                    ExpanderEx exTmp = (ExpanderEx)obj;

                    exTmp.IsExpanded = true;

                    //若标签名相同
                    if (exTmp.tagName == item.TagName)
                    {
                        System.Windows.Controls.WrapPanel wpTmp = (System.Windows.Controls.WrapPanel)exTmp.Content;

                        wpTmp.Children.Add(item);

                        IsAdded = true;

                    }
                }
            }

            //如果未找到相同标签名的分组
            if (!IsAdded)
            {
                ExpanderEx ex = new ExpanderEx(item.TagName);

                ex.Style = Application.Current.FindResource("ExpanderExStyle") as Style;

                System.Windows.Controls.WrapPanel wp = new System.Windows.Controls.WrapPanel();

                wp.Children.Add(item);

                ex.Content = wp;

                WindowMain.Recent.Children.Add(ex);
            }
        }

        /// <summary>
        /// 反注册所有的热键
        /// </summary>
        /// <returns></returns>
        public static int UnregisterAllHotKeys()
        {
            //卸载主要热键
            HotKey.UnregisterHotKey(WindowMainHandle, HotKey.QUICK_SEARCH_HOTKEY_ID);

            int UnregisteredCount = 0;

            //反注册其他热键
            foreach (HotKeyItem i in listOfApplicationHotKey)
            {
                if (HotKey.UnregisterHotKey(WindowMainHandle, i.ID))
                {
                    UnregisteredCount++;
                }
            }

            return UnregisteredCount;

        }

        /// <summary>
        /// 反注册某一热键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int UnregisterHotKey(int id)
        {
            foreach (HotKeyItem item in listOfApplicationHotKey)
            {
                if (item.ID == id)
                {
                    HotKey.UnregisterHotKey(WindowMainHandle, id);
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 检查待注册的热键是否冲突，若不冲突则注册，否则返回false
        /// </summary>
        /// <param name="HKVI">HotKeyVisualItem</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckAndRegisterHotKey(HotKeyVisualItem HKVI, object iParent, int id = -65536)
        {
            bool StatusAvailable = true;
            if (HKVI.Available)
            {
                if (id == -65536)
                    id = ++HotKey.CurrentID;

                HotKeyItem hki = new HotKeyItem(iParent, HKVI.KeyValue, HKVI.ModifiersValue, id, HotKeyItem.HotKeyParentType.Item);

                foreach (HotKeyItem h in listOfApplicationHotKey)
                {
                    if (hki.ID == h.ID)
                    {
                        StatusAvailable = false;
                        break;
                    }
                }

                if (StatusAvailable)
                {
                    listOfApplicationHotKey.Add(hki);
                    HotKey.RegisterHotKey(new System.Windows.Interop.WindowInteropHelper(WindowMain).Handle, hki.ID, hki.ModifiersValue, (uint)hki.KeyValue);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 检测是否冲突并注册热键
        /// </summary>
        /// <param name="HKI">HotKeyItem</param>
        /// <returns></returns>
        public static bool CheckAndRegisterHotKey(HotKeyItem HKI)
        {
            if (HKI.ID == 0x0000)
            {
                HKI.ID = ++HotKey.CurrentID;
            }

            if (HotKey.TestHotKey(HKI.ModifiersValue, HKI.KeyValue, HKI.ID, false))
            {
                listOfApplicationHotKey.Add(HKI);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查找都应的快捷键响应
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool FindHotKeyAndExecute(int id)
        {
            foreach (HotKeyItem i in listOfApplicationHotKey)
            {
                if (id == i.ID)
                {
                    if (i.IParent is ItemData)
                    {
                        (i.IParent as ItemData).Execute();
                        break;
                    }
                    else if (i.IParent is string)
                    {
                        Class.Plugins.Run((string)i.IParent);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 使用搜索引擎搜索
        /// </summary>
        /// <param name="Keyword"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static int SearchOnWeb(string Keyword, string URL)
        {
            //检查参数的正确性
            if (string.IsNullOrEmpty(Keyword) || string.IsNullOrEmpty(URL))
                return -1;

            if (URL.IndexOf("%keyword%") < 0)
                return -2;

            //执行搜索
            if (!Manage.MOWeb.IsUsed)
            {
                System.Diagnostics.Process.Start(URL.Replace("%keyword%", Keyword));
            }
            else
            {
                Class.Plugins.Run(MOWeb.MdlName, URL.Replace("%keyword%", Keyword));
            }

            return 0;
        }

        /// <summary>
        /// 打开属性窗口
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int OpenAttributeWindow(Item item)
        {
            wndItemInformation wndInfo = new wndItemInformation(item);

            return 0;
        }

        /// <summary>
        /// 打开搜索引擎选择窗口
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static int OpenSearchWindow(string Keyword)
        {
            wndSE wndse = new wndSE(Keyword);
            wndse.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //wndse.ShowActivated = true;
            wndse.ShowDialog();
            return 0;
        }

        /// <summary>
        /// 用于设置搜索框的显示相关
        /// </summary>
        /// <param name="txtMain"></param>
        /// <param name="Show"></param>
        /// <param name="FillText"></param>
        /// <returns></returns>
        public static int ClearOrFillText(ref System.Windows.Controls.TextBox txtMain, bool Show, string FillText = "Use keyword to search")
        {
            if (txtMain != null)
            {
                if (Show)
                {
                    if (txtMain.Text.Trim() == WindowMain.KeywordTip)
                        txtMain.Text = "";
                }
                else
                {
                    if (string.IsNullOrEmpty(txtMain.Text.Trim()))
                        txtMain.Text = FillText;
                }
            }
            else
                return -1;
            return 0;
        }

        /// <summary>
        /// 打开手动添加窗体
        /// </summary>
        /// <returns></returns>
        public static int OpenAddWindow()
        {
            WindowAdd = new wndAdd();
            if (WindowAdd != null)
            {
                WindowAdd.ShowDialog();
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 打开参数窗体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int OpenArgumentsWindow(ItemData itemdata)
        {

            if (itemdata == null)
                return -1;

            WindowArgs = new wndArguments();
            if (WindowArgs != null)
            {
                WindowArgs.It = itemdata;
                WindowArgs.ItemName = itemdata.Name;
                WindowArgs.Arguments = itemdata.Arguments;
                WindowArgs.ShowDialog();
            }
            else
            {
                return -1;
            }
            return 0;
        }


        public static List<Item> MultiSearch(string keyword)
        {

            if (!string.IsNullOrEmpty(keyword))
            {
                //声明用于存放结果的数组
                List<Item> rtnValue = new List<Item>();

                //临时pattern
                string paTmp = "";

                //临时regex
                Regex reTmp;

                //查找分隔符
                if (keyword.IndexOf(":") >= 0)
                {

                    //分割关键字
                    Match ma = reKeyword.Match(keyword);

                    //取值
                    string Name = ma.Groups[1].Value.Trim();
                    string Path = ma.Groups[2].Value.Trim();
                    string Tag = ma.Groups[3].Value.Trim();

                    
                    //填充正则表达式
                    paTmp = !string.IsNullOrEmpty(Name) ? (rANY + Name + rANY) : rANY;

                    paTmp += rSPLIT;

                    paTmp += !string.IsNullOrEmpty(Path) ? (rANY + Path + rANY) : rANY;

                    paTmp += rSPLIT;

                    paTmp += !string.IsNullOrEmpty(Tag) ? (rANY + Tag + rANY) : rANY;

                    reTmp = new Regex(paTmp, RegexOptions.IgnoreCase);

                    innerSearch(rtnValue, reTmp);
                }
                else
                {
                    paTmp = rANY + keyword + rANY;

                    reTmp = new Regex(paTmp,RegexOptions.IgnoreCase);

                    innerSearch(rtnValue, reTmp);
                }

                return rtnValue;
            }
            else
            {
                return null;
            }
        }

        private static void innerSearch(List<Item> rtnValue,Regex reTmp)
        {

            string nptUnion = "";

            foreach (object obj in Manage.WindowMain.Recent.Children)
            {
                if (obj is ExpanderEx)
                {
                    WrapPanel wpTmp = (WrapPanel)((Expander)obj).Content;

                    foreach (Item i in wpTmp.Children)
                    {
                        nptUnion = i.Name_Property + rSPLIT + i.Path + rSPLIT + i.TagName;

                        if (reTmp.IsMatch(nptUnion))
                        {
                            rtnValue.Add(i);
                        }
                    }
                }
                else
                {
                    Item item = (Item)obj;

                    nptUnion = item.Name + rSPLIT + item.Path + rSPLIT + item.TagName;

                    if (reTmp.IsMatch(nptUnion))
                    {
                        rtnValue.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="wnd_"></param>
        /// <param name="wp"></param>
        public static void InitializeData()
        {
            //保存主窗体相关信息
            WindowMainRect.left = (int)WindowMain.Left;
            WindowMainRect.right = (int)(WindowMain.Left + WindowMain.ActualWidth);
            WindowMainRect.top = (int)WindowMain.Top;
            WindowMainRect.bottom = (int)(WindowMain.Top + WindowMain.ActualHeight);

            //创建进度窗体实例
            wndProgressBar wndpb = new wndProgressBar(LoadingInnerDataHeader, LoadingInnerDataFooter, mMAIN.GetAllChild().Count);

            //wnd_.Opacity = 0.000001;
            WindowMain.WindowState = System.Windows.WindowState.Normal;

            int ChildCount = mMAIN.GetAllChild().Count;

            //获取项目尺寸
            double ItemSize = ApplicationInformations.Anything.AppInfoOperations.GetItemSize();

            //开始加载数据
            for (int i = 0; i < ChildCount; i++)
            {

                ItemData itemdata = new ItemData(mMAIN.GetAllChild()[i]);

                listOfInnerData.Add(itemdata);

                Item item = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource, ItemSize, itemdata.TagName);

                item.Path = itemdata.Path;

                item.RefItemData = itemdata;

                item.Margin = new System.Windows.Thickness(5);

                item.Click += Item_Click;

                FindAndInsert(item);

                wndpb.Increase();
            }

            WindowMain.Opacity = ApplicationInformations.Anything.AppInfoOperations.GetMaxOpacity();

            List<string> tmp = mKeywordRecent.ReadAllString();

            if (tmp != null)
                listOfRecentKeyword = tmp;

            LoadSE();

            wndpb.Increase();
        }

        /// <summary>
        /// 加载搜索引擎资源
        /// </summary>
        public static void LoadSE()
        {

            listOfSearchEngineInnerData.Clear();
            listOfSearchEnginesVisualElement.Clear();
            List<Anoicess.Anoicess.Anoicess._Content> t = mSearchEngine.ReadAllContent();
            if (t != null)
            {
                if (t.Count > 0)
                    listOfSearchEngineInnerData = t;

                foreach (Anoicess.Anoicess.Anoicess._Content item in t)
                {
                    if (item.IsUsed == 1)
                    {
                        listOfSearchEnginesVisualElement.Add(new SearchEngineItem(item.Name, item.Content, ""));
                    }
                }
            }

        }

        /// <summary>
        /// 用于刷新项目的图标
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemdata"></param>
        public static void RefreshSingle(Item item, ItemData itemdata)
        {
            //从原有集合中删除
            ((System.Windows.Controls.WrapPanel)item.Parent).Children.Remove(item);

            //构造新的Item对象
            Item newOne = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource, ApplicationInformations.Anything.AppInfoOperations.GetItemSize(), itemdata.TagName);

            //填充基本信息
            newOne.Path = itemdata.Path;
            newOne.Margin = new System.Windows.Thickness(5);
            newOne.RefItemData = itemdata;

            //添加消息处理
            newOne.Click += Item_Click;

            FindAndInsert(newOne);

        }

        /// <summary>
        /// 添加新项目
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Name"></param>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public static Item AddItem(String Path, string Name = "", string Arguments = "", string tagName = "")
        {
            //try
            {
                //检查路径
                CheckPath(Path);

                string subPath = "";

                if (!string.IsNullOrEmpty(lnkInfo.Name))
                {
                    Path = lnkInfo.TargetPath;
                    if (string.IsNullOrEmpty(Name))
                    {
                        Name = lnkInfo.Name;
                        if (string.IsNullOrEmpty(Arguments))
                            Arguments = lnkInfo.Arguments;

                        if (string.IsNullOrEmpty(lnkInfo.WorkingDirectory))
                        {
                            subPath = GetSubPath(Path);
                        }
                        else
                        {
                            subPath = lnkInfo.WorkingDirectory;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Name))
                        Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(Path));

                    subPath = GetSubPath(Path);

                    if (string.IsNullOrEmpty(Arguments))
                    {
                        if (!new Regex("::\\{.+\\}").IsMatch(Path))
                        {
                            Arguments = GetArgumentsFromFullPath(Path);
                        }
                    }
                }



                //获取图标
                byte[] b;

                if (FileOperation.IsFile(Path) == -1)
                {
                    b = GetResourcesIcon("folder.png");
                }
                else
                {
                    //系统引用
                    if (Path.IndexOf("{") > 0 && Path.IndexOf("}") > 0)
                    {
                        switch (Path)
                        {
                            case MyComputer:
                                b = GetResourcesIcon("computer.png");
                                break;
                            case ControlPanel:
                                b = GetResourcesIcon("controlpanel.png");
                                break;
                            case MyDocument:
                                b = GetResourcesIcon("mydocument.png");
                                break;
                            case RecycleBin:
                                b = GetResourcesIcon("recyclebin.png");
                                break;
                            case NetworkNeighborhood:
                                b = GetResourcesIcon("networkneighborhood.png");
                                break;
                            default:
                                b = GetIcon.GetIconByteArray(Path);
                                break;
                        }
                    }
                    else
                    {
                        b = GetIcon.GetIconByteArray(Path);
                    }
                }

                string id = ClsMD5.ClsMD5.Encrypt(Name + Path);

                ImageSource IS = GetIcon.ByteArrayToIS(b);

                //构造UI对象
                Item item = new Item(id, Name, IS, ApplicationInformations.Anything.AppInfoOperations.GetItemSize(), tagName);

                item.Path = Path;

                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name, Path, id, IS, b, "", 1, 0, 0));

                bool itemexists = false;

                foreach (ItemData idd in listOfInnerData)
                {
                    if(idd.ID==itemdata.ID)
                    {
                        itemexists = true;
                        break;
                    }
                }

                if (itemexists)
                {
                    TipPublic.ShowFixed(WindowMain, ItemExists);
                    return null;
                }
                    

                if (!string.IsNullOrEmpty(tagName))
                    itemdata.TagName = tagName;

                //有参数则填充参数
                if (!string.IsNullOrEmpty(Arguments.Trim()))
                    itemdata.Arguments = Arguments;

                //填充工作路径
                if (!string.IsNullOrEmpty(subPath.Trim()))
                    itemdata.WorkingDirectory = subPath;


                Manage.listOfInnerData.Add(itemdata);

                item.RefItemData = itemdata;

                item.Margin = new System.Windows.Thickness(5);

                item.Click += Item_Click;

                return item;
            }

        }

        ///// <summary>
        ///// 移除项目
        ///// </summary>
        ///// <param name="Parent"></param>
        ///// <param name="ID"></param>
        //public static void RemoveItem(object Parent, String ID)
        //{
        //    System.Windows.Controls.WrapPanel wp = (System.Windows.Controls.WrapPanel)Parent;

        //    Item tmp = null;

        //    foreach (Item i in wp.Children)
        //    {
        //        if (i.ID == ID)
        //        {
        //            wp.Children.Remove(i);
        //            mMAIN.RemoveChild(i.Name_Property);
        //            i.RefItemData.Dispose();
        //            listOfInnerData.Remove(i.RefItemData);
        //            i.Dispose();
        //            tmp = i;
        //            break;
        //        }
        //    }
        //    tmp = null;
        //}

        /// <summary>
        /// 添加我的电脑
        /// </summary>
        /// <returns></returns>
        public static Item AddComputer()
        {
            return AddItem(MyComputer, "My Computer");
        }

        /// <summary>
        /// 添加控制面板
        /// </summary>
        /// <returns></returns>
        public static Item AddControlPanel()
        {
            return AddItem(ControlPanel, "Control Panel");
        }

        /// <summary>
        /// 添加回收站
        /// </summary>
        /// <returns></returns>
        public static Item AddRecycleBin()
        {
            return AddItem(RecycleBin, "Recycle Bin");
        }

        /// <summary>
        /// 添加家我的文档
        /// </summary>
        /// <returns></returns>
        public static Item AddMyDocument()
        {
            return AddItem(MyDocument, "My Document");
        }

        /// <summary>
        /// 添加网络邻居
        /// </summary>
        /// <returns></returns>
        public static Item AddNetworkNeighbor()
        {
            return AddItem(NetworkNeighborhood, "Network Neighborhood");
        }

        /// <summary>
        /// 用于保存搜索关键字，暂未完成
        /// </summary>
        /// <param name="str"></param>
        public static void SaveKeyword(string str)
        {
            if (!string.IsNullOrEmpty(str) && str != "Use keyword to search")
            {
                listOfRecentKeyword.Add(str);
                mKeywordRecent.Insert(str, str);
            }
        }
        #endregion

        #region private

        /// <summary>
        /// 检查提供的路径，判断是否lnk文件
        /// </summary>
        /// <param name="path"></param>
        private static void CheckPath(string path)
        {
            if (path.ToLower().IndexOf(".lnk") >= 0)
            {
                WshShell shell = new WshShell();
                IWshShortcut iss = (IWshShortcut)shell.CreateShortcut(path);
                lnkInfo.Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(path));
                lnkInfo.TargetPath = iss.TargetPath;
                lnkInfo.WorkingDirectory = iss.WorkingDirectory;
                lnkInfo.Arguments = iss.Arguments;
            }
            else
            {
                lnkInfo.Name = "";
                lnkInfo.TargetPath = path;
                lnkInfo.WorkingDirectory = "";
                lnkInfo.Arguments = "";
            }

        }

        /// <summary>
        /// 处理Item的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Item_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Item tmp = (Item)sender;
            int errCode = 0;
            if ((errCode = tmp.RefItemData.Execute()) < 0)
            {
                if (errCode == -1)
                    TipPublic.ShowFixed(WindowMain, "File or folder doesn't exist.");
                else
                    TipPublic.ShowFixed(WindowMain, "Unknown error.");
            }
            else
                WindowMain.txtMain.Text = "";

            SaveKeyword(WindowMain.txtMain.Text);

        }

        /// <summary>
        /// 从资源中获取指定的图片
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static byte[] GetResourcesIcon(string Name)
        {
            byte[] b;
            System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("/Resources/" + Name, UriKind.Relative));

            BinaryReader br = new BinaryReader(sri.Stream);

            b = br.ReadBytes((int)sri.Stream.Length);

            br.Close();
            br = null;
            return b;

        }

        /// <summary>
        /// 获取子路径
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static string GetSubPath(string Path)
        {
            int LastPos = Path.LastIndexOf("\\");
            if (LastPos > 0)
            {
                return Path.Substring(0, LastPos + 1);
            }
            else
                return Path;
        }

        /// <summary>
        /// 从路径获取参数
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static string GetArgumentsFromFullPath(string Path)
        {
            string FileNameWithExtension = FileOperation.GetName(Path);
            if (!string.IsNullOrEmpty(FileNameWithExtension.Trim()))
            {
                int LastPos = Path.LastIndexOf(FileNameWithExtension);
                if (LastPos >= 0)
                {
                    int spaceIndex = FileNameWithExtension.LastIndexOf(" ");
                    int MinusIndex = FileNameWithExtension.LastIndexOf("-");
                    int speatorIndex = FileNameWithExtension.LastIndexOf("/");

                    if (spaceIndex >= 0)
                    {
                        if (MinusIndex >= 0)
                        {
                            LastPos += MinusIndex;
                        }
                        else
                        {
                            LastPos += spaceIndex;
                        }
                    }
                    else if (MinusIndex >= 0)
                    {
                        LastPos += MinusIndex;
                    }
                    else if (speatorIndex >= 0)
                    {
                        LastPos += speatorIndex;
                    }

                    return Path.Substring(LastPos, Path.Length - LastPos);
                }
                else return "";
            }
            else return "";
        }

        #endregion

    }
}
