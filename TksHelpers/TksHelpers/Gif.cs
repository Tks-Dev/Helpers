using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows.Threading;

namespace TksHelpers
{
    public class Gif
    {
        private Image gifImage;

        public Image GifImage
        {
            get
            {
                return gifImage;
            }
            set
            {
                gifImage = value;
                dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
                frameCount = gifImage.GetFrameCount(dimension);
            }
        }

        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame = 0;
        public bool ReverseAtEnd { get; set; }
        public bool AutoRestart { get; set; }
        private int step = 1;

        public Gif() { }

        public Gif(string path)
        {
            Load(path);
        }

        public void Load(string path)
        {
            gifImage = Image.FromFile(path);
            //initialize
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            //gets the GUID
            //total frames in the animation
            frameCount = gifImage.GetFrameCount(dimension);
        }

        public Image GetNextFrame()
        {

            currentFrame += step;

            //if the animation reaches a boundary...
            if (currentFrame >= frameCount || currentFrame < 1)
            {
                if (ReverseAtEnd)
                {
                    step *= -1;
                    //...reverse the count
                    //apply it
                    currentFrame += step;
                }
                else {
                    currentFrame = 0;
                    //...or start over
                }
            }
            return GetFrame(currentFrame);
        }

        public void Play(int fps, System.Windows.Controls.Image container)
        {

            var delay = 1000 / (double)fps;
            var timer = new Timer(delay);
            container.Dispatcher.Invoke(() => container.ChangeSource(GetFrame(currentFrame).ToImageSource(), 0, 0), DispatcherPriority.Render);
            timer.Elapsed += (sender, args) =>
            {
                currentFrame += step;
                if (currentFrame >= frameCount || currentFrame < 1)
                {
                    if (AutoRestart)
                        if (ReverseAtEnd)
                        {
                            step *= -1;
                            currentFrame += step;
                        }
                        else
                            currentFrame = 0;
                    else
                        timer.Enabled = false;

                }
                container.Dispatcher.Invoke(() =>
                    container.ChangeSource(GetFrame(currentFrame).ToImageSource(), 0, 0), DispatcherPriority.Render);

            };
            timer.Enabled = true;
        }

        public void Reset()
        {
            currentFrame = 0;
        }

        public int Count => frameCount;


        public Image GetFrame(int index)
        {
            gifImage.SelectActiveFrame(dimension, index);
            //find the frame
            return (Image)gifImage.Clone();
            //return a copy of it
        }
    }
}