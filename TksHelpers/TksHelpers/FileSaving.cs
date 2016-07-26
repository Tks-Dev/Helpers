using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TksHelpers
{
    public static class FileSaving
    {
        public static void SaveAsJpg(this ImageSource img, string fullpath, int quality = 100)
        {
            var bmp = img as BitmapSource;
            var encoder = new JpegBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = quality;

            using (var file = File.OpenWrite(fullpath + ".jpg"))
            {
                encoder.Save(file);
            }
        }

        public static string SaveAsPng(this ImageSource img, string fullpath)
        {
            var bmp = img as BitmapSource;
            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite(fullpath + ".png"))
            {
                encoder.Save(file);
            }

            return fullpath + ".png";
        }
    }
}
