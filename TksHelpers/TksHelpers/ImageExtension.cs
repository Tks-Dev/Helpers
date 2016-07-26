using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TksHelpers
{
    public static class ImageExtension
    {
        /// <summary>
        /// Change cette image avec une transition en fondu
        /// </summary>
        /// <param name="image">L'image this à modifier</param>
        /// <param name="source">La source de la prochaine image</param>
        /// <param name="beginTime">Le délai en seconde après lequel la transition commence</param>
        /// <param name="fadeTime">Le durée de la transition</param>
        public static void ChangeSource(this Image image, ImageSource source, double beginTime, double fadeTime)
        {
            if (Math.Abs(beginTime) < 0.00001 && Math.Abs(fadeTime) < 0.00001)
            {
                //var fromThread = Dispatcher.FromThread(Thread.CurrentThread) ?? Dispatcher.CurrentDispatcher;
                //fromThread.Invoke();
                image.Dispatcher.Invoke(() => image.Source = source, DispatcherPriority.DataBind);
                return;
            }


            var fadeInAnimation = new DoubleAnimation
            {
                To = 1,
                BeginTime = TimeSpan.FromSeconds(fadeTime),
                Duration = TimeSpan.FromSeconds(fadeTime)
            };
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (image.Source != null)
                {
                    var fadeOutAnimation = new DoubleAnimation
                    {
                        To = 0,
                        BeginTime = TimeSpan.FromSeconds(beginTime),
                        Duration = TimeSpan.FromSeconds(fadeTime)
                    };

                    fadeOutAnimation.Completed += (o, e) =>
                    {
                        image.Source = source;
                        image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                    };
                    image.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                }
                else
                {
                    image.Opacity = 0d;
                    image.Source = source;
                    fadeInAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                    image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                }
            });
        }

        public static void ChangeSource(this Image image, string path)
        {
            image.ChangeSource(path, 0, 0);
        }

        public static void ChangeSource(this Image image, string path, double beginTime, double fadeTime)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Picture at : \"" + path + "\" does not exist.", "No image", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var imgsrc = new BitmapImage();
            imgsrc.BeginInit();
            imgsrc.UriSource = new Uri(path, UriKind.Relative);
            imgsrc.CacheOption = BitmapCacheOption.OnLoad;
            imgsrc.EndInit();
            image.ChangeSource(imgsrc, beginTime, fadeTime);
        }
    }
}
