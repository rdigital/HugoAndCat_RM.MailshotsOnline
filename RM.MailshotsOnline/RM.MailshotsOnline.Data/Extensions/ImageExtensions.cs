using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Gets the file extension (including leaing dot) for a given image format
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/14159148/get-extension-from-system-drawing-imaging-imageformat-c"/>
        public static string ToFileExtension(this ImageFormat format)
        {
            try
            {
                return ImageCodecInfo.GetImageEncoders()
                        .First(x => x.FormatID == format.Guid)
                        .FilenameExtension
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .First()
                        .Trim('*')
                        .ToLower();
            }
            catch (Exception)
            {
                return "." + new ImageFormatConverter().ConvertToString(format).ToLower();
            }
        }
    }
}
