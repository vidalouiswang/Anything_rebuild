using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class InputInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
        public string TagName { get; set; }

        public InputInfo(string Path,string Name="Default Name",string Arguments="",string TagName="")
        {
            this.Name = Name;
            this.Path = Path;
            this.Arguments = Arguments;
            this.TagName = TagName;
        }
    }
}
