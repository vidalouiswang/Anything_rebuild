using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class UniversalHandler : TypeHandler
    {
        public UniversalHandler(InputInfo input)
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
            base.GetIcon();
        }
    }
}
