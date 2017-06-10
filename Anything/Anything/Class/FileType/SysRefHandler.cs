using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class SysRefHandler : TypeHandler
    {
        public SysRefHandler(InputInfo input)
        {
            this.Name = input.Name;
            this.Path = input.Path;
            this.TagName = input.TagName;
            this.Arguments = input.Arguments;
        }
        protected override void GetBaseInfo()
        {
            base.GetBaseInfo();
        }
        protected override void GetIcon()
        {
            if (Manage.reSysRef.IsMatch(this.Path))
            {
                switch (Path)
                {
                    case Manage.MyComputer:
                        this.Icon = GetResourcesIcon("computer.png");
                        break;
                    case Manage.ControlPanel:
                        this.Icon = GetResourcesIcon("controlpanel.png");
                        break;
                    case Manage.MyDocument:
                        this.Icon = GetResourcesIcon("mydocument.png");
                        break;
                    case Manage.RecycleBin:
                        this.Icon = GetResourcesIcon("recyclebin.png");
                        break;
                    case Manage.NetworkNeighborhood:
                        this.Icon = GetResourcesIcon("networkneighborhood.png");
                        break;
                }
            }
        }
    }
}
