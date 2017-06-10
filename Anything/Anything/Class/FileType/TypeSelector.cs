using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class TypeSelector
    {
        public TypeHandler TH { get; set; } = null;
        public bool IsInitialized { get; set; } = false;
        public TypeSelector(InputInfo input)
        {
            switch (GetExtensionName(input.Path))
            {
                case "jpg":
                    TH = new ImageHandler(input);
                    break;
                case "png":
                    TH = new ImageHandler(input);
                    break;
                case "bmp":
                    TH = new ImageHandler(input);
                    break;
                case "lnk":
                    TH = new LinkHandler(input);
                    break;
                case "url":
                    TH = new LinkHandler(input);
                    break;
                case "sys":
                    TH = new SysRefHandler(input);
                    break;
                case "any":
                    TH = new UniversalHandler(input);
                    break;
                default:
                    TH = new UniversalHandler(input);
                    break;
            }

            TH.Initialize();

            this.IsInitialized = this.TH.Available;
        }

        private string GetExtensionName(string Path)
        {
            if (Path.Contains("."))
            {
                int lf = Path.LastIndexOf(".") + 1;
                return Path.Substring(lf, Path.Length - lf).ToLower();
            }
            else
            {
                if (Manage.reSysRef.IsMatch(Path))
                {
                    return "sys";
                }
                else
                {
                    return "any";
                }
            }
        }
    }
}
