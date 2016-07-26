using System;
using System.Windows;
using TksHelpers;

namespace TextAnimation
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool b;
        private Gif gif;
        public MainWindow()
        {
            InitializeComponent();
            b = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!b)
            {
                Grid1.Slide(525, 0, 1, UiElementExtension.OffsetDirection.Left, () => b = true);
                Grid2.Slide(525, 0, 1, UiElementExtension.OffsetDirection.Left, () => b = true);
            }
            else
            {
                Grid1.Slide(525, 0, 1, UiElementExtension.OffsetDirection.Right, () => b = false);
                Grid2.Slide(525, 0, 1, UiElementExtension.OffsetDirection.Right, () => b = false);
            }

            ExplorerHelper.Notification("Le titre", "Le text", 10000, System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Users\TOSHIBA\Documents\Visual Studio 2015\Projects\TestHelper\TextAnimation\icon.ico"));
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            if (gif == null)
            {
                gif = new Gif()
                {
                    AutoRestart = true
                };
                gif.Load(@"C:\Users\TOSHIBA\Downloads\error-funny-title-not-found-please-call-windows-amp-039-meme-support-center_webm_6708873 converted.gif");
            }
            gif.Play(15, GifContainer);
        }
    }
}
