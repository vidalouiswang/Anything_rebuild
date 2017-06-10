using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anything.Class.FileType
{
    class ImageHandler : TypeHandler
    {
        public ImageHandler(InputInfo input)
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
            Image image = new Bitmap(this.Path);

            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else if (format.Equals(ImageFormat.MemoryBmp))
                {
                    image.Save(ms, ImageFormat.MemoryBmp);
                }

                image = image.GetThumbnailImage(256, 256, null, IntPtr.Zero);

                byte[] buffer = new byte[ms.Length];
                
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                this.Icon = buffer;
            }


        }

    }
}
