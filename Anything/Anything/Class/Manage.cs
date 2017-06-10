﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Anything.Class.FileType;
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
        public static string CurrentPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

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
        //public static wndLoading WindowLoading;


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


        public static string LoadingInnerDataHeader = Application.Current.TryFindResource("VEManageLoadingInnerDataHeader") as string;

        public static string LoadingInnerDataFooter = Application.Current.TryFindResource("VEManageLoadingInnerDataFooter") as string;


        public static string ItemExists = Application.Current.TryFindResource("VEManageItemExists") as string;


        public static string DefaultTagName = Application.Current.TryFindResource("VEManageDefaultTagName") as string;


        public static double ItemLength = (double)ApplicationInformations.Anything.AppInfoOperations.GetItemSize();

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

        public static void ExpanderExOperation(bool Expanded = true)
        {
            if (WindowMain != null)
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
                if (rdSelected != null)
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
                    WindowMain.KeywordTip = Application.Current.TryFindResource("VEwndMainKeywordTip") as string;

                    WindowMain.HotKeyFailed = Application.Current.TryFindResource("VEwndMainHotKeyFailed") as string;

                    WindowMain.CloseTip = Application.Current.TryFindResource("VEwndMainCloseTip") as string;

                    WindowMain.AllLoadHeader = Application.Current.TryFindResource("VEwndMainAllLoadHeader") as string;

                    WindowMain.AllLoadFooter = Application.Current.TryFindResource("VEwndMainAllLoadFooter") as string;

                    LoadingInnerDataHeader = Application.Current.TryFindResource("VEManageLoadingInnerDataHeader") as string;

                    LoadingInnerDataFooter = Application.Current.TryFindResource("VEManageLoadingInnerDataFooter") as string;

                    ItemExists = Application.Current.TryFindResource("VEManageItemExists") as string;

                    DefaultTagName = Application.Current.TryFindResource("VEManageDefaultTagName") as string;

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

                if (listOfLanguage.Count > 0)
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

            if (!string.IsNullOrEmpty(item.refItemData.TagName))
            {
                SelectInsert(item);
            }
            else
            {
                item.refItemData.TagName = DefaultTagName;
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
                    if (exTmp.tagName == item.refItemData.TagName)
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
                ExpanderEx ex = new ExpanderEx(item.refItemData.TagName);

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






        #region 初始化

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="wnd_"></param>
        /// <param name="wp"></param>
        public static void InitializeProgram()
        {
            InitializeWindowMainInfo();

            LoadData();

            LoadKeywordRecentData();

            LoadSearchEngine();

            WindowMain.Opacity = ApplicationInformations.Anything.AppInfoOperations.GetMaxOpacity();
        }


        /// <summary>
        /// 加载最近使用的关键字
        /// </summary>
        public static void LoadKeywordRecentData()
        {
            List<string> tmp = mKeywordRecent.ReadAllString();

            if (tmp != null)
                listOfRecentKeyword = tmp;
        }

        /// <summary>
        /// 初始化主窗体位置信息
        /// </summary>
        public static void InitializeWindowMainInfo()
        {
            if (WindowMain != null)
            {
                //保存主窗体相关信息
                WindowMainRect.left = (int)WindowMain.Left;
                WindowMainRect.right = (int)(WindowMain.Left + WindowMain.ActualWidth);
                WindowMainRect.top = (int)WindowMain.Top;
                WindowMainRect.bottom = (int)(WindowMain.Top + WindowMain.ActualHeight);

                WindowMain.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                UnregisterAllHotKeys();

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// 加载项目数据
        /// </summary>
        public static void LoadData()
        {
            List<Anoicess.Anoicess.Anoicess> Child = new List<Anoicess.Anoicess.Anoicess>();

            Child = mMAIN.GetAllChild();

            //获取项目尺寸
            double ItemSize = ApplicationInformations.Anything.AppInfoOperations.GetItemSize();

            //开始加载数据
            foreach (Anoicess.Anoicess.Anoicess ai in Child)
            {
                ItemData itemdata = new ItemData(ai);

                listOfInnerData.Add(itemdata);

                Item item = new Item(itemdata);

                item.Click += Item_Click;

                FindAndInsert(item);
            }
        }


        /// <summary>
        /// 加载搜索引擎资源
        /// </summary>
        public static void LoadSearchEngine()
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
        #endregion


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
            Item newOne = new Item(itemdata);

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
            TypeSelector ts = new TypeSelector(new InputInfo(Path, Name, Arguments, tagName));

            if (ts.IsInitialized)
            {
                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(ts.TH.Name, ts.TH.Path, ts.TH.Icon, ts.TH.Arguments, ts.TH.SubPath));

                bool itemexists = false;

                foreach (ItemData idd in listOfInnerData)
                {
                    if (idd.ID == itemdata.ID)
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

                Manage.listOfInnerData.Add(itemdata);

                //构造UI对象
                Item item = new Item(itemdata);

                item.Click += Item_Click;

                return item;
            }
            else
            {
                return null;
            }


        }


        #region 添加系统引用

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

        #endregion


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

       
        #endregion

    }
}
