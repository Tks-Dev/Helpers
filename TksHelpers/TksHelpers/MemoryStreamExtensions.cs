using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TksHelpers
{
    public static class MemoryStreamExtensions
    {
        public static ImageSource ToImageSource(this System.IO.MemoryStream memoryStream)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = memoryStream;
            bi.EndInit();
            return bi;
        }
    }
}