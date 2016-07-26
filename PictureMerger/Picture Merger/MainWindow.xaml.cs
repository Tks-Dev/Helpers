using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using SIO = System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Draw = System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TksHelpers;

namespace Picture_Merger
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int log;
        public string savename;
        public string debugPath;
        public Bits LSB;
        public byte dec;
        public MainWindow()
        {
            InitializeComponent();
            debugPath = SIO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MergeDebug");
            
        }

        public byte[] ImageToByteArray(Draw.Image imageIn)
        {
            var ms = new SIO.MemoryStream();
            imageIn.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }

        public Draw.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new SIO.MemoryStream(byteArrayIn);
            var returnImage = Draw.Image.FromStream(ms);
            return returnImage;
        }

        private void SelectPic_Click(object sender, RoutedEventArgs e)
        {
            var str = ExplorerHelper.FileFromBrowser(string.Empty, string.Empty);
            original.ChangeSource(str);
        }

        public Draw.Color Hide(Draw.Color basePixel, Draw.Color pixelToHide, int numberOfBits)
        {
            var rBits = new Bits();
            var gBits = new Bits();
            var bBits = new Bits();
            rBits.CreateFromBits(basePixel.R.AsBits().GetFirstBits(Bits.BYTE_BITS - numberOfBits),
                    pixelToHide.R.AsBits().GetFirstBits(numberOfBits));
            gBits.CreateFromBits(basePixel.G.AsBits().GetFirstBits(Bits.BYTE_BITS - numberOfBits),
                pixelToHide.G.AsBits().GetFirstBits(numberOfBits));
            bBits.CreateFromBits(basePixel.B.AsBits().GetFirstBits(Bits.BYTE_BITS - numberOfBits),
                pixelToHide.B.AsBits().GetFirstBits(numberOfBits));
            if (ChkBoxInvert?.IsChecked ?? false)
            {
                rBits = rBits.GetInverted();
                gBits = gBits.GetInverted();
                bBits = bBits.GetInverted();
            }
            //var ba = ApplyMask(rBits, gBits, bBits, Convert.ToUInt16(Seed.Value ?? 0));
            //var r = (byte)ba[0];
            //var g = (byte)ba[1];
            //var b = (byte)ba[2];
            log++;
            if (log % 500 == 0)
                Dispatcher.Invoke(
                    () =>
                    {
                        CryptBar.Value = log;

                    },
                    DispatcherPriority.Render);
            return Draw.Color.FromArgb(rBits, gBits, bBits);
        }

        public Draw.Color ClearLSB(Draw.Color basePixel)
        {
            var MSB = new Bits();
            MSB.FillFromString("11110000");
            return Draw.Color.FromArgb(
                basePixel.R.AsBits() & MSB,
                basePixel.G.AsBits() & MSB,
                basePixel.B.AsBits() & MSB);
        }

        public Draw.Bitmap Hide(Draw.Bitmap baseImg, Draw.Bitmap hide, int bit)
        {
            var img = new Draw.Bitmap(Math.Min(baseImg.Width, hide.Width), Math.Min(baseImg.Height, hide.Height));
            for (var i = 0; i < img.Width; i++)
                for (var j = 0; j < img.Height; j++)
                    img.SetPixel(i, j, Hide(baseImg.GetPixel(i, j), hide.GetPixel(i, j), bit));
            return img;
        }

        public Draw.Bitmap ClearLSB(Draw.Bitmap basImg)
        {
            var img = new Draw.Bitmap(basImg.Width, basImg.Height);
            for (var i = 0; i < basImg.Width; i++)
                for (var j = 0; j < basImg.Height; j++)
                    img.SetPixel(i, j, ClearLSB(basImg.GetPixel(i, j)));

            return img;
        }

        public ImageSource ImageToSource(Draw.Image img)
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

        public static Draw.Bitmap BitmapSourceToBitmap2(BitmapSource source)
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

        private void SelectHidden_OnClick(object sender, RoutedEventArgs e)
        {
            var str = ExplorerHelper.FileFromBrowser(string.Empty, string.Empty);
            hidden.ChangeSource(str);
        }

        private void Encrypt_OnClick(object sender, RoutedEventArgs e)
        {

            var b = BitmapSourceToBitmap2(original.Source as BitmapSource);
            var h = BitmapSourceToBitmap2(hidden.Source as BitmapSource);
            log = 0;
            Dispatcher.Invoke(
                new Action(() => CryptBar.Maximum = Math.Min(b.Width, h.Width) * Math.Min(b.Height, h.Height)),
                DispatcherPriority.Render);
            var c = b.HideAsync(h, bits.Value ?? 1);
            changed.Source = ImageToSource(c);
            //Debug();
        }
        
        public Draw.Bitmap Retrieve(Draw.Bitmap toretrieve, int bit)
        {
            var img = new Draw.Bitmap(toretrieve.Width, toretrieve.Height);
            var lsb = string.Empty;
            for (var i = Bits.BYTE_BITS; i > 0; i--)
                if (i > bit)
                    lsb += "0";
                else
                    lsb += "1";
            LSB = new Bits();
            var decb = new Bits();
            decb.FillWithTrue(Bits.BYTE_BITS - bit);
            dec = decb;
            LSB.FillFromString(lsb);
            
            for (var i = 0; i < img.Width; i++)
                for (var j = 0; j < img.Height; j++)
                    img.SetPixel(i, j, Retrieve(toretrieve.GetPixel(i, j), bit));
            Dispatcher.Invoke(
                    () =>
                    {
                        CryptBar.Value = CryptBar.Maximum;
                    },
                    DispatcherPriority.Render);
            return img;
        }

        public Draw.Color Retrieve(Draw.Color pixel, int bit)
        {
            
            var r = (pixel.R & LSB);
            var g = (pixel.G & LSB);
            var b = (pixel.B & LSB);
            //var ba = ApplyMask(r, g, b, Convert.ToUInt16(Seed.Value ?? 0));
            //r = ba[0];
            //g = ba[1];
            //b = ba[2];
            if (ChkBoxInvert?.IsChecked ?? false)
            {
                r = (pixel.R.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
                g = (pixel.G.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
                b = (pixel.B.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
            }

            log++;
            if (log % 10000 == 0)
                Dispatcher.Invoke(
                    () =>
                    {
                        CryptBar.Value = log;
                    },
                    DispatcherPriority.Render);
            return Draw.Color.FromArgb(r * dec, g * dec, b * dec);
        }

        private void Decrypt_OnClick(object sender, RoutedEventArgs e)
        {
            var b = BitmapSourceToBitmap2(original.Source as BitmapSource);
            log = 0;
            Dispatcher.Invoke(
                new Action(() => CryptBar.Maximum = b.Width * b.Height),
                DispatcherPriority.Render);
            var c = Retrieve(b, bits.Value ?? 1);
            changed.Source = ImageToSource(c);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var fnw = new FileNameWindow { MW = this };

            if (!(fnw.ShowDialog() ?? false))
                return;
            changed.Source.SaveAsPng(savename);
        }

        private void Debug()
        {
            var b = BitmapSourceToBitmap2(original.Source as BitmapSource);
            var h = BitmapSourceToBitmap2(hidden.Source as BitmapSource);
            var c = Hide(b, h, 4);
            changed.Source = ImageToSource(c);
            var p = changed.Source.SaveAsPng(SIO.Path.Combine(debugPath, DateTime.Now.Ticks.ToString()));
            original.ChangeSource(p);
            var nberrorBase = CompareBytes(h, c);
            var nberrorafEncode = CompareBytes(h, BitmapSourceToBitmap2(original.Source as BitmapSource));
            MessageBox.Show("Il y a " + nberrorBase + " dans l'image modifiée.\n" +
                            "Il y a " + nberrorafEncode + " erreurs après l'encodage.");
        }

        public long CompareBytes(Draw.Bitmap source, Draw.Bitmap modification)
        {
            var diffCount = 0L;
            var w = Math.Min(source.Width, modification.Width);
            var h = Math.Min(source.Height, modification.Height);
            for (var i = 0; i < w; i++)
                for (var j = 0; j < h; j++)
                    if (CompareBytes(source.GetPixel(i, j), modification.GetPixel(i, j)))
                        diffCount++;
            return diffCount;
        }

        private static bool CompareBytes(Draw.Color source, Draw.Color mod)
        {
            var rsrc = source.R;
            var gsrc = source.G;
            var bsrc = source.B;
            var r = mod.R;
            var g = mod.G;
            var b = mod.B;
            return rsrc.AsBits().GetMSQ() == r.AsBits().GetLSQ()
                   && gsrc.AsBits().GetMSQ() == g.AsBits().GetLSQ()
                   && bsrc.AsBits().GetMSQ() == b.AsBits().GetLSQ();
        }

        //public Bits[] ApplyMask(Bits b1, Bits b2, Bits b3, ushort mask)
        //{
        //    if (mask == 0)
        //        return new[] { b1, b2, b3 };
        //    var bMask = mask.AsBits().GetLastBits(12);
        //    var m1 = bMask.GetFirstBits(4);
        //    var m2 = bMask.GetFirstBits(8).GetLastBits(4);
        //    var m3 = bMask.GetLastBits(4);
        //    return new[] { ApplyMask(b1, m1), ApplyMask(b2, m2), ApplyMask(b3, m3) };
        //}

        private static Bits ApplyMask(Bits bit, Bits mask)
        {
            var bab = bit.BitArray;
            var bam = mask.BitArray;
            var min = Math.Min(bab.Length, bam.Length);
            for (var i = 0; i < min; i++)
                if (bam[i])
                    bab[1] = !bab[i];
            var b = new Bits();
            b.FillFromList(bab);
            return b;
        }
    }
}
