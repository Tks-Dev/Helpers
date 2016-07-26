using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SIO = System.IO;
using Draw = System.Drawing;

namespace TksHelpers
{
    public static class ImageConversionExtension
    {
        public static ImageSource ToImageSource(this Draw.Image img)
        {
            var ms = new SIO.MemoryStream();
            img.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public static Draw.Bitmap ToDrawingBitmap(this BitmapSource source)
        {
            var bmp = new Draw.Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              Draw.Imaging.PixelFormat.Format32bppPArgb);
            var data = bmp.LockBits(
              new Draw.Rectangle(Draw.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              Draw.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static byte[] ToByteArray(this Draw.Image imageIn)
        {
            var ms = new SIO.MemoryStream();
            imageIn.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }

        public static Draw.Image ToDrawingImage(this byte[] byteArrayIn)
        {
            var ms = new SIO.MemoryStream(byteArrayIn);
            var returnImage = Draw.Image.FromStream(ms);
            return returnImage;
        }
    }
}
