using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoBlogApp.Core.Helpers
{
    public class ImageHelper
    {
        public static Image ResizeImage(Stream file, Size size)
        {
            var imgToResize = new Bitmap(file);

            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (size.Width / (float)sourceWidth);
            nPercentH = (size.Height / (float)sourceHeight);

            nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        public static Image CropImage(Image img, Rectangle cropArea)
        {
            using (var bmpImage = new Bitmap(img))
            {
                return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            }
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }

        public static ImageProps BuildImage(HttpPostedFileBase file, string pathFull, string fileName, int fW, int fH)
        {
            var fullImage = ResizeImage(file.InputStream, new Size(fW, fH));
            var imageProps = new ImageProps
            {
                W = fullImage.Width,
                H = fullImage.Height
            };

            var savePath = Path.Combine(pathFull, fileName);

            SaveJpeg(savePath, (Bitmap)fullImage, 88);
            
            return imageProps;
        }

        public static void SaveJpeg(string path, Bitmap img, long quality)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var jpegCodec = GetEncoderInfo("image/jpeg");
            if (jpegCodec == null) return;

            var encoderParams = new EncoderParameters(1)
            {
                Param = {[0] = qualityParam}
            };

            img.Save(path, jpegCodec, encoderParams);
            img.Dispose();
        }
    }

    public class ImageProps
    {
        public int W { get; set; }
        public int H { get; set; }
    }
}