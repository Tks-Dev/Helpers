using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TksHelpers
{
    public static class ExplorerHelper
    {
        /// <summary>
        /// Open a folder browser dialog to get the path to a folder
        /// </summary>
        /// <param name="windowTitle">The title of the FolderBrowserDialog</param>
        /// <returns>The path of the selected folder</returns>
        public static string FolderFromBrowser(string windowTitle)
        {
            var f = new FolderBrowserDialog
            {
                Description = windowTitle
            };
            f.ShowDialog();
            return f.SelectedPath;
        }

        /// <summary>
        /// Open a file browser dialog to get the path to a folder
        /// </summary>
        /// <param name="windowTitle">The title of the FileBrowserDialog</param>
        /// <param name="initialDirectory">Path of the start directory</param>
        /// <returns>The path to the selected file</returns>
        public static string FileFromBrowser(string windowTitle, string initialDirectory)
        {
            var f = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Title = windowTitle
            };
            f.ShowDialog();
            return f.FileName;
        }

        public static string SaveFileWindow(string title, string initialDirectory, string extension)
        {
            var f = new SaveFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = extension,
                InitialDirectory = initialDirectory,
                Title = title,
                ValidateNames = true
            };
            f.ShowDialog();
            return f.FileName;
        }

        private static NotifyIcon _notifyIcon;

        public static void Notification(string title, string content, int time, Icon icon = null)
        {
            if (_notifyIcon == null)
                _notifyIcon = new NotifyIcon
                {
                    BalloonTipTitle = title,
                    BalloonTipText = content,
                    Icon = icon ?? SystemIcons.WinLogo,
                    BalloonTipIcon = ToolTipIcon.Info,
                    Visible = true,
                };
            else
            {
                _notifyIcon.BalloonTipText = content;
                _notifyIcon.BalloonTipTitle = title;
                _notifyIcon.Icon = icon ?? SystemIcons.WinLogo;
            }
            _notifyIcon.ShowBalloonTip(time);
        }
    }
}
