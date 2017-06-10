using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    public class TypeHandler
    {
        protected TypeHandler() { }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
        public string SubPath { get; set; }
        public byte[] Icon { get; set; }
        public string TagName { get; set; }

        public bool Available { get; set; } = false;

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(this.Path))
            {
                this.Available = false;
                return;
            }

            GetBaseInfo();

            GetIcon();

            this.Available = true;
        }

        protected virtual void GetBaseInfo()
        {
            //若未指定名称则自动获取一个默认名称
            if (string.IsNullOrEmpty(this.Name))
                this.Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(this.Path));

            //获取到子路径
            this.SubPath = GetSubPath(this.Path);

            //获取参数
            if (string.IsNullOrEmpty(this.Arguments))
                this.Arguments = GetArgumentsFromFullPath();
        }

        protected virtual void GetIcon()
        {
            if (FileOperation.IsFile(Path) == -1)
            {
                this.Icon = GetResourcesIcon("folder.png");
            }
            else
            {
                this.Icon = Class.GetIcon.GetIconByteArray(Path);
            }
        }


        /// <summary>
        /// 从资源中获取指定的图片
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        protected byte[] GetResourcesIcon(string Name)
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
        protected string GetSubPath(string Path)
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
        protected string GetArgumentsFromFullPath()
        {
            string FileNameWithExtension = FileOperation.GetName(this.Path);
            if (!string.IsNullOrEmpty(FileNameWithExtension.Trim()))
            {
                int LastPos = this.Path.LastIndexOf(FileNameWithExtension);
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

                    return this.Path.Substring(LastPos, this.Path.Length - LastPos);
                }
                else return "";
            }
            else return "";
        }

    }
}
