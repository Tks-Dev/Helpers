using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TagLib;
using TFile = TagLib.File;
using SIO = System.IO;

namespace TksHelpers
{
    public class Mp3Music
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public string Artists { get; set; }
        public string Genres { get; set; }
        private ImageSource _cover;
        private TFile _base;
        public Exception SaveException { get; private set; }
        public Exception CopyException { get; private set; }
        public Exception SaveAsException { get; private set; }

        public Mp3Music(TFile file)
        {
            _base = file;
            Title = file.Tag.Title;
            Path = file.Name;
            Album = file.Tag.Album;
            Artists = string.Join("; ", file.Tag.AlbumArtists);
            Genres = string.Join("; ", file.Tag.Genres);
            Duration = file.Properties.Duration;
            _cover = GetCover(file);
            file.Dispose();
        }

        public Mp3Music() { }

        /// <summary>
        /// Get the cover of the selected file
        /// </summary>
        /// <param name="file">The music file</param>
        /// <returns>The cover of the file or null if there is no cover</returns>
        public static ImageSource GetCover(TFile file)
        {
            var timg = file.Tag.Pictures.FirstOrDefault();
            return timg?.Data.Data.ToDrawingImage().ToImageSource();
        }

        /// <summary>
        /// Set the cover of this Mp3Music from a music file
        /// </summary>
        /// <param name="file">The source music file</param>
        public void SetCover(TFile file)
        {
            _cover = GetCover(file);
        }

        /// <summary>
        /// Set the cover of this Mp3Music from an existing ImageSource
        /// </summary>
        /// <param name="nextImage">The image to set as a cover</param>
        public void SetCover(ImageSource nextImage)
        {
            _cover = nextImage;
        }

        /// <summary>
        /// set the cover from a file (Image or MusicFile)
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <param name="isPicturePath">Default true : the path is a path to an image. Set to false if you want to use a musicfile instead</param>
        public void SetCover(string path, bool isPicturePath = true)
        {
            if (isPicturePath)
                _cover = ImageHelper.ImageSourceFromPath(path);
            else
                SetCover(TFile.Create(path));
        }

        /// <summary>
        /// Getter of the cover
        /// </summary>
        /// <returns>The cover of this Mp3Music</returns>
        public ImageSource GetTrackCover() => _cover;

        /// <summary>
        /// Save the datas of this in the original music file. In case of Error, check the SaveExceptionProperty
        /// </summary>
        /// <returns>True if the save works, false else</returns>
        public bool Save()
        {
            try
            {
                _base.Tag.Title = Title;
                _base.Tag.Album = Album;
                _base.Tag.AlbumArtists = Artists.Split(';').Select(a => a.Trim()).ToArray();
                _base.Tag.Genres = Genres.Split(';').Select(g => g.Trim()).ToArray();
                _base.Tag.Pictures = new IPicture[] {new Picture((_cover as BitmapSource).ToDrawingBitmap().ToByteArray())};
                _base.Save();
                _base.Dispose();
                return true;
            }
            catch (Exception e)
            {
                SaveException = e;
                return false;
            }
        }

        /// <summary>
        /// Save this to a new file
        /// </summary>
        /// <param name="path">The path of the new file</param>
        /// <returns>The new file if Save as works, this else</returns>
        public Mp3Music SaveAs(string path)
        {
            try
            {
                if(path == Path)
                    throw new InvalidOperationException("A file can't be saved as himself. Use Save() instead");
                var cp = Copy(path);
                if (cp == null)
                    throw CopyException;
                if (!cp.Save())
                    throw cp.SaveException;
                return cp;
            }
            catch (Exception e)
            {
                SaveAsException = e;
                return this;
            }
        }

        /// <summary>
        /// Copy this in a new file. It won't override existing files.
        /// </summary>
        /// <param name="path">The path of the new file</param>
        /// <returns>The new Mp3Music of succeded, this else</returns>
        public Mp3Music Copy(string path)
        {
            try
            {
                SIO.File.Copy(_base.Name, path);
                return new Mp3Music(TFile.Create(path));
            }
            catch (Exception e)
            {
                CopyException = e;
                return null;
            }
        }
    }
}
