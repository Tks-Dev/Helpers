using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using Draw = System.Drawing;

namespace TksHelpers
{
    public static class Steganography
    {
        public delegate void PixelHiddenEventHandler(object sender, EventArgs args);

        public static event PixelHiddenEventHandler PixelHidden;
        private static int? _key;
        private static int HiddingHeight { get; set; }
        private static Draw.Bitmap HiddingResult { get; set; }
        private static Draw.Color[,] Tcolors { get; set; }

        public static void OnPixeHidden(EventArgs e)
        {
            PixelHidden?.Invoke(null, e);
        }

        private static Bits _lsb;
        private static Bits _dec;
        /// <summary>
        /// Replace [numberOfBits] bits at the end of this by the [numberOfBits] first bit of pixeltoHide
        /// </summary>
        /// <param name="basePixel">this</param>
        /// <param name="pixelToHide">The pixel to hide in this</param>
        /// <param name="numberOfBits">The number of bits hidden in a pixel</param>
        /// <param name="inverted">Not Implemented Yet</param>
        /// <returns>A pixel with a parts of the other pixel hidden</returns>
        public static Draw.Color Hide(this Draw.Color basePixel, Draw.Color pixelToHide, int numberOfBits, bool inverted = false)
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
            //if (inverted)
            //{
            //    rBits = rBits.GetInverted();
            //    gBits = gBits.GetInverted();
            //    bBits = bBits.GetInverted();
            //}
            OnPixeHidden(EventArgs.Empty);
            return Draw.Color.FromArgb(rBits, gBits, bBits);
        }

        /// <summary>
        /// Hide a picture in another
        /// </summary>
        /// <param name="baseImg">The host image</param>
        /// <param name="toHide">The image to hide</param>
        /// <param name="bit">the number of bits of each pixel from de secret image to use</param>
        /// <param name="inverted">Not Implemented Yet</param>
        /// <returns>Merged picture with the smallest dimension (Min(Height),Min(Width))</returns>
        public static Draw.Bitmap Hide(this Draw.Bitmap baseImg, Draw.Bitmap toHide, int bit, bool inverted = false)
        {
            var img = new Draw.Bitmap(Math.Min(baseImg.Width, toHide.Width), Math.Min(baseImg.Height, toHide.Height));
            for (var i = 0; i < img.Width; i++)
                for (var j = 0; j < img.Height; j++)
                    img.SetPixel(i, j, baseImg.GetPixel(i, j).Hide(toHide.GetPixel(i, j), bit, inverted));
            return img;
        }

        /// <summary>
        /// Hide a picture in another Using multithreading. Huge gain of performance on largeimage
        /// </summary>
        /// <param name="baseImg">The host image</param>
        /// <param name="toHide">The image to hide</param>
        /// <param name="bit">the number of bits of each pixel from de secret image to use</param>
        /// <param name="inverted">Not Implemented Yet</param>
        /// <returns>Merged picture with the smallest dimension (Min(Height),Min(Width))</returns>
        public static Draw.Bitmap HideAsync(this Draw.Bitmap baseImg, Draw.Bitmap toHide, int bit, bool inverted = false)
        {
            HiddingResult = new Draw.Bitmap(Math.Min(baseImg.Width, toHide.Width), Math.Min(baseImg.Height, toHide.Height));
            Tcolors = new Draw.Color[HiddingResult.Width, HiddingResult.Height];
            var hiddingActions = new List<Task>();
            HiddingHeight = HiddingResult.Height;
            for (var i = 0; i < HiddingResult.Width; i++)
            {
                var i1 = i;
                var bimgClone = (Draw.Bitmap)baseImg.Clone();
                var thClone = (Draw.Bitmap)toHide.Clone();
                hiddingActions.Add(Task.Run(() =>
                {
                    var i2 = i1;
                    var bclone = (Draw.Bitmap)bimgClone.Clone();

                    for (var j = 0; j < HiddingHeight; j++)
                        Dispatcher.CurrentDispatcher.Invoke(
                            () =>
                                Tcolors[i2, j] =
                                    bclone.GetPixel(i2, j).Hide(thClone.GetPixel(i2, j), bit, inverted));
                }));
            }
            var taskArray = hiddingActions.ToArray();
            Task.WaitAll(taskArray);
            for (var w = 0; w < HiddingResult.Width; w++)
                for (var h = 0; h < HiddingResult.Height; h++)
                    HiddingResult.SetPixel(w, h, Tcolors[w, h]);
            return HiddingResult;
        }

        /// <summary>
        /// Get the hidden picture
        /// </summary>
        /// <param name="toretrieve">this</param>
        /// <param name="bit">the number of bits used to hide the secret image</param>
        /// <param name="inverted">Not implemented yet</param>
        /// <returns>The secret piture</returns>
        public static Draw.Bitmap Retrieve(this Draw.Bitmap toretrieve, int bit, bool inverted = false)
        {
            var img = new Draw.Bitmap(toretrieve.Width, toretrieve.Height);
            var lsb = string.Empty;
            for (var i = Bits.BYTE_BITS; i > 0; i--)
                if (i > bit)
                    lsb += "0";
                else
                    lsb += "1";
            _lsb = new Bits();
            var decb = new Bits();
            decb.FillWithTrue(Bits.BYTE_BITS - bit);
            _dec = decb;
            _lsb.FillFromString(lsb);

            for (var i = 0; i < img.Width; i++)
                for (var j = 0; j < img.Height; j++)
                    img.SetPixel(i, j, toretrieve.GetPixel(i, j).Retrieve(bit, inverted));

            return img;
        }

        /// <summary>
        /// Get the hidden pixel in this
        /// </summary>
        /// <param name="pixel">this</param>
        /// <param name="bit">the number of bits used to hide the secret image</param>
        /// <param name="inverted">Not implmented yet</param>
        /// <returns>The secret pixel</returns>
        public static Draw.Color Retrieve(this Draw.Color pixel, int bit, bool inverted = false)
        {
            var r = (pixel.R & _lsb);
            var g = (pixel.G & _lsb);
            var b = (pixel.B & _lsb);
            //if (inverted)
            //{
            //    r = (pixel.R.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
            //    g = (pixel.G.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
            //    b = (pixel.B.AsBits().GetLastBits(Bits.BYTE_BITS - bit)).GetInverted();
            //}
            return Draw.Color.FromArgb(r * _dec, g * _dec, b * _dec);
        }

        /// <summary>
        /// Set the encryption key to hide a picture
        /// </summary>
        /// <param name="key">Encryption key</param>
        public static void SetKey(int key)
        {
            _key = key;
        }

        /// <summary>
        /// Remove the Encryption key
        /// </summary>
        public static void ResetKey()
        {
            _key = null;
        }

        private static int Rotate(int num, int rot, int numberOfBits)
        {
            if (rot == 0)
                return num;
            rot = rot % numberOfBits;
            var dec = (numberOfBits - rot);
            var mask = (2 ^ rot - 1) << dec;
            var r = (num & mask) >> dec;
            num = (num << rot) | r;
            num = num & (2 ^ numberOfBits - 1);
            return num;
        }
    }
}
