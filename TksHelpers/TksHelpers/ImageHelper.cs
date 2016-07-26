using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TksHelpers
{
    public static class ImageHelper
    {
        public static ImageSource ImageSourceFromPath(string path)
        {
            var imgsrc = new BitmapImage();
            imgsrc.BeginInit();
            imgsrc.UriSource = new Uri(path, UriKind.Relative);
            imgsrc.CacheOption = BitmapCacheOption.OnLoad;
            imgsrc.EndInit();
            return imgsrc;
        }
    }
}