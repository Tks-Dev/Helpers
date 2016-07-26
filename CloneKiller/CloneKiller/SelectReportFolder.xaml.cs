using System.Windows;
using CloneKiller.Helpers;
using ZetaLongPaths;

namespace CloneKiller
{
    /// <summary>
    /// Logique d'interaction pour SelectReportFolder.xaml
    /// </summary>
    public partial class SelectReportFolder : Window
    {
        public string Folder { get; set; }

        public SelectReportFolder()
        {
            InitializeComponent();
        }

        private void BtnBrowse_OnClick(object sender, RoutedEventArgs e)
        {
            Folder = Helper.FolderFromBrowser(string.Empty);
            TboxPath.Text = Folder;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            var f = new ZlpDirectoryInfo(Folder);
            DialogResult = f.Exists;
        }
    }
}
