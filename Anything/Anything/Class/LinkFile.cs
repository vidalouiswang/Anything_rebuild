using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace Anything.Class
{
    public class LinkFile
    {
        //Lnk数据结构，仅主要信息
        public struct _Link
        {
            public string Name;
            public string WorkingDirectory;
            public string TargetPath;
            public string Arguments;
        }

        //lnk文件信息
        private _Link lnkInfo = new _Link();

        public _Link LnkInfo
        {
            get
            {
                return lnkInfo;
            }

            set
            {
                lnkInfo = value;
            }
        }

        public bool Available
        {
            get
            {
                return available;
            }

            set
            {
                available = value;
            }
        }

        private bool available = false;

        public LinkFile(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                if (Path.ToLower().IndexOf(".lnk") >= 0)
                {
                    GetLnkFileInfo(Path);
                    this.available = true;
                }
            }
        }

        /// <summary>
        /// 获取Lnk文件信息
        /// </summary>
        /// <param name="path"></param>
        private void GetLnkFileInfo(string path)
        {

            WshShell shell = new WshShell();
            IWshShortcut iss = (IWshShortcut)shell.CreateShortcut(path);
            lnkInfo.Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(path));
            lnkInfo.TargetPath = iss.TargetPath;
            lnkInfo.WorkingDirectory = iss.WorkingDirectory;
            lnkInfo.Arguments = iss.Arguments;

        }
    }
}
