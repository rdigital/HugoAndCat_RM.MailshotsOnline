using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;

namespace HC.RM.Common.Images
{
    public class ImageResizer
    {
        private readonly ILogger _logger = new Logger();

        /// <summary>
        /// Converts the bytes provided into an Image
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>An Image.</returns>
        public Image GetImage(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Image.FromStream(stream);
            }
        }

        public byte[] GetBytes(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);
                var resizedBytes = stream.GetBuffer();

                return resizedBytes;
            }
        }

        /// <summary>
        /// Resizes an Image to a specific width and returns a byte array
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="width">THe width</param>
        /// <returns>The byte array of the resized image</returns>
        public byte[] ResizeImageBytes(Image image, int width)
        {
            try
            {
                byte[] resizedBytes = null;

                var targetH = checked((int)(image.Height * ((float)width / (float)image.Width)));
                var targetW = width;

                // no change in width and height => return original image
                if (image.Height == targetH && image.Width == targetW)
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, image.RawFormat);
                        resizedBytes = stream.GetBuffer();

                        return resizedBytes;
                    }
                }

                // Graphics objects can not be created from bitmaps with an Indexed Pixel Format, use RGB instead.
                var format = image.PixelFormat;
                if (format.ToString().Contains("Indexed") || format == PixelFormat.Format16bppArgb1555 ||
                    format == PixelFormat.Format16bppGrayScale || (int)format == 8207)
                {
                    format = PixelFormat.Format24bppRgb;
                }

                using (var bitmap = new Bitmap(targetW, targetH, format))
                {
                    try
                    {
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            g.CompositingMode = CompositingMode.SourceCopy;

                            // use an image attribute in order to remove the black/gray border around image after resize
                            // (most obvious on white images), see this post for more information:
                            // http://www.codeproject.com/KB/GDI-plus/imgresizoutperfgdiplus.aspx
                            using (var attribute = new ImageAttributes())
                            {
                                attribute.SetWrapMode(WrapMode.TileFlipXY);

                                // draws the resized image to the bitmap
                                g.DrawImage(image, new Rectangle(new Point(0, 0), new Size(targetW, targetH)), 0, 0,
                                    image.Width, image.Height, GraphicsUnit.Pixel, attribute);
                            }

                            using (var stream = new MemoryStream())
                            {
                                bitmap.Save(stream, image.RawFormat);
                                resizedBytes = stream.GetBuffer();
                            }
                        }
                    }
                    catch (OutOfMemoryException e)
                    {
                        _logger.Error(this.GetType().Name, "Resize",
                            "OutOfMemoryException Image format: {0} Message: {1}, StackTrace: {2}", format, e.Message,
                            e.StackTrace);
                    }
                }

                return resizedBytes;
            }
            catch (ExternalException e)
            {
                _logger.Error(this.GetType().Name, "Resize", "ExternalException Message: {0}, StackTrace: {1}",
                    e.Message, e.StackTrace);
            }
            catch (ArgumentNullException e)
            {
                _logger.Error(this.GetType().Name, "Resize", "ArgumentNullException Message: {0}, StackTrace: {1}",
                    e.Message, e.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// Resizes an Image to a specific width and resturns a new Image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Image ResizeImage(Image image, int width)
        {
            var resized = ResizeImageBytes(image, width);

            using (var stream = new MemoryStream(resized))
            {
                image = Image.FromStream(stream);
            }

            return image;
        }
    }
}
