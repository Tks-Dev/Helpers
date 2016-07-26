using System.Collections.Generic;
using System.Linq;

namespace TksHelpers
{
    public class MultifileSelector
    {

        public static readonly string[] ImagesExt = { "bmp", "jpg", "jpeg", "png" };
        public static readonly string[] SoundExt = { "mp3", "wma", "flac", "ogg", "wav" };

        public List<string> Result { get; private set; }
        public string WindowTitle { get; set; }
        public string Directory { get; set; }

        public static MultifileSelector Show(string windowTitle, string initialDirectory, FileType filter = FileType.ALL)
        {
            var mfs = new MultifileSelector();
            var f = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Title = windowTitle,
                Multiselect = true,
                Filter = GetFilter(filter)
            };
            f.ShowDialog();
            mfs.Result = f.FileNames.ToList();
            return mfs;
        }

        public void Show(FileType filter = FileType.ALL)
        {
            var f = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = Directory,
                Title = WindowTitle,
                Multiselect = true,
                Filter = GetFilter(filter)
            };
            f.ShowDialog();
            Result = f.FileNames.ToList();
        }

        private static string GetFilter(FileType fileType)
        {
            var strar = new[] { string.Empty };
            var str = string.Empty;
            switch (fileType)
            {
                case FileType.IMAGES:
                    strar = ImagesExt;
                    str = "Imges ";
                    break;
                case FileType.SOUND:
                    strar = SoundExt;
                    str = "Sounds ";
                    break;
                default:
                    strar = null;
                    break;
            }
            if (strar != null)
                str += "|*." + string.Join(";*.", strar) + "|";
            str += "All Files (*.*)|*.*";
            return str;
        }

        private MultifileSelector() { }
    }

    public enum FileType
    {
        IMAGES,
        SOUND,
        ALL
    }
}