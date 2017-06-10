using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class LinkHandler : TypeHandler
    {
        public LinkHandler(InputInfo input)
        {
            this.Name = input.Name;
            this.Path = input.Path;
            this.TagName = input.TagName;
            this.Arguments = input.Arguments;
        }
        protected override void GetBaseInfo()
        {
            LinkFile lnkfile = new LinkFile(this.Path);
            if (lnkfile.Available)
            {
                //填充名称
                if (string.IsNullOrEmpty(this.Name))
                    this.Name = lnkfile.LnkInfo.Name;

                //填充参数
                if (string.IsNullOrEmpty(this.Arguments))
                    this.Arguments = lnkfile.LnkInfo.Arguments;

                //填充工作路径
                if (!string.IsNullOrEmpty(lnkfile.LnkInfo.WorkingDirectory))
                    this.SubPath = lnkfile.LnkInfo.WorkingDirectory;
                else
                    this.SubPath = GetSubPath(this.Path);

            }
            else
            {
                //若未获取到lnk文件信息则按照普通文件处理
                base.GetBaseInfo();
            }
        }
        protected override void GetIcon()
        {
            base.GetIcon();
        }
    }
}
