using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Anything.UserControls;

namespace Anything.Class
{
    public class Search
    {
        //字段分隔
        private static string rSPLIT = "#SPLIT#";

        //通配
        private static string rANY = ".*";

        //分隔关键字的正则
        private static Regex reKeyword = new Regex(@"([^:]*)?:{1}([^:]*)?:?([^:]*)?");

        //内部实例
        private static Search innerInstance = null;

        private Search() { }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static Search GetInstance()
        {
            if (innerInstance == null)
            {
                innerInstance = new Search();

                return innerInstance;
            }
            else
            {
                return innerInstance;
            }
        }


        #region 搜索

        /// <summary>
        /// 搜索的外部接口
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Item> MultiSearch(string keyword)
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

                    reTmp = new Regex(paTmp, RegexOptions.IgnoreCase);

                    innerSearch(rtnValue, reTmp);
                }

                return rtnValue;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 实际的搜索过程
        /// </summary>
        /// <param name="rtnValue"></param>
        /// <param name="reTmp"></param>
        private void innerSearch(List<Item> rtnValue, Regex reTmp)
        {

            string nptUnion = "";

            foreach (object obj in Manage.WindowMain.Recent.Children)
            {
                if (obj is ExpanderEx)
                {
                    WrapPanel wpTmp = (WrapPanel)((Expander)obj).Content;

                    foreach (Item i in wpTmp.Children)
                    {
                        nptUnion = i.refItemData.Name + rSPLIT + i.refItemData.Path + rSPLIT + i.refItemData.TagName;

                        if (reTmp.IsMatch(nptUnion))
                        {
                            rtnValue.Add(i);
                        }
                    }
                }
                else
                {
                    Item item = (Item)obj;

                    nptUnion = item.refItemData.Name + rSPLIT + item.refItemData.Path + rSPLIT + item.refItemData.TagName;

                    if (reTmp.IsMatch(nptUnion))
                    {
                        rtnValue.Add(item);
                    }
                }
            }
        }

        #endregion
    }
}
