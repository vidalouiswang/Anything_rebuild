using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class
{

    class Keywords
    {
        private static Keywords Instance;

        private Keywords()
        {
            List<Anoicess.Anoicess.Anoicess._Content> data = this.mKeywordRecent.ReadAllContent();

            this.Count = data.Count;

            if (this.Count>0)
            {
                foreach (var item in data)
                {
                    this.KeywordItems.Add(new KeywordItem(item.Name, Convert.ToInt32(item.Content)));
                }
            }
        }

        public static Keywords GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Keywords();

                return Instance;
            }

            return Instance;
        }

        public Anoicess.Anoicess.Anoicess mKeywordRecent = new Anoicess.Anoicess.Anoicess("mKeywordRecent");

        public const int MAX_COUNT= 10;
        public List<KeywordItem> KeywordItems { get; set; } = new List<KeywordItem>();

        public int Count { get; set; } = 0;

        public void Insert(string Keyword)
        {
            if (KeywordItems.Count>0)
            {
                bool exist = false;
                foreach (KeywordItem item in KeywordItems)
                {
                    if (item.Equals(Keyword))
                    {
                        item.Usage++;
                        exist = true;
                    }
                }
                if (!exist)
                {
                    if (Count<MAX_COUNT)
                    {
                        KeywordItems.Add(new KeywordItem(Keyword, 0));
                        Count++;
                    }
                    else
                    {
                        Remove(GetLastItem());
                        KeywordItems.Add(new KeywordItem(Keyword, 0));
                        Count++;
                    }
                }
            }
            else
            {
                KeywordItems.Add(new KeywordItem(Keyword, 0));
                Count++;
            }
        }

        public void Remove(KeywordItem ki)
        {
            if (ki == null)
                return;

            KeywordItems.Remove(ki);

            this.mKeywordRecent.Remove(ki.Keyword);

            Count--;

        }

        public void Flush()
        {
            if (this.KeywordItems.Count>0)
            {
                foreach (KeywordItem item in this.KeywordItems)
                {
                    this.mKeywordRecent.Insert(item.Keyword, item.Usage.ToString());
                }
            }
        }

        private KeywordItem GetLastItem()
        {
            if (KeywordItems.Count <= 0)
                return null;

            KeywordItem ki = KeywordItems[0];

            foreach (KeywordItem item in KeywordItems)
            {
                if (item.Usage < ki.Usage)
                    ki = item;
            }
            return ki;
        }

    }




#pragma warning disable CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
    class KeywordItem
#pragma warning restore CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
    {
        #region 成员变量
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 利用率
        /// </summary>
        public int Usage { get; set; }

        #endregion

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="Keyword"></param>
        /// <param name="Usage"></param>
        public KeywordItem(string Keyword,int Usage=0)
        {
            this.Keyword = Keyword;
            this.Usage = Usage;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetType() == typeof(KeywordItem) ? (((KeywordItem)obj).Keyword == this.Keyword ? true : false) : (obj.GetType()==typeof(string) ? ((string)obj==this.Keyword ? true : false) : false));
        }
    }
}
