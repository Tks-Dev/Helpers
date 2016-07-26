using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CloneKiller.Business;
using CloneKiller.Helpers;

namespace CloneKiller
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RedundantFiles RedundantFiles { get; set; }
        public int Percent { get; set; }
        public RedundantFile SelectedFile { get; set; }
        public string BaseTitle { get; set; }
        private WindowState _windowState;
        private bool _userChangedState;

        public MainWindow()
        {
            Percent = 0;
            InitializeComponent();
            BaseTitle = Title;
            _windowState = WindowState;
            _userChangedState = true;

        }

        private async void MitemSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            RedundantFiles = new RedundantFiles() { Percent = Percent, _chn = new ChangeCountDelegate(ChangeCount), MainWindow = this};
            _userChangedState = false;
            var p = Helper.FolderFromBrowser(string.Empty);
            var time = new Stopwatch();
            Title = BaseTitle + " (Work in progress)";
            time.Start();
            if (CboxMinimize.IsChecked ?? false)
                WindowState = WindowState.Minimized;
            if (!string.IsNullOrEmpty(p))
                    await RedundantFiles.FillFromFolder(p);
            var strAr = RedundantFiles.ToStringArrayList().OrderBy(arr => arr[0]);
            time.Stop();
            Title = BaseTitle + " ( Done " + time.ElapsedMilliseconds + " ms. " + (RedundantFiles.Error ? "Some files weren't tested. " : string.Empty) +")";
            if (CboxRestore.IsChecked ?? false)
                WindowState = _windowState;
            DgridClones.ItemsSource = (from tab in strAr
                                       select new
                                       {
                                           Filename = tab[0],
                                           Count = int.Parse(tab[1]),
                                           Weight = LongToString(tab[2]),
                                           Total = LongToString(tab[3]),
                                           BytesLong = long.Parse(tab[2]),
                                           TotalBytes = long.Parse(tab[3])
                                       });
        }

        private void UpDoPercentError_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Percent = UpDoPercentError.Value ?? Percent;
        }

        private static string LongToString(string p)
        {
            //var s = string.Empty;
            //var l = p.Length - 1;
            //for (var i = p.Length - 1; i >= 0; i--)
            //    s = p[i] + ((l - i) % 3 == 0 ? " " : string.Empty) + s;
            //return s;
            var val = long.Parse(p);
            var unit = "B";
            if (val / 1024 <= 0) return val + " " + unit;
            val = val / 1024;
            unit = "KB";
            if (val / 1024 <= 0) return val + " " + unit;
            val = val / 1024;
            unit = "MB";
            if (val / 1024 <= 0) return val + " " + unit;
            val = val / 1024;
            unit = "GB";
            return val + " " + unit;
        }

        private void MItemShowClone_OnClick(object sender, RoutedEventArgs e)
        {
            if (!((new Report(SelectedFile)).ShowDialog() ?? false)) return;
            var strAr = RedundantFiles.ToStringArrayList();
            DgridClones.ItemsSource = (from tab in strAr
                select new
                {
                    Filename = tab[0],
                    Count = int.Parse(tab[1]),
                    Weight = LongToString(tab[2]),
                    Total = LongToString(tab[3]),
                    BytesLong = long.Parse(tab[2]),
                    TotalBytes = long.Parse(tab[3])
                });
        }

        private void DgridClones_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedFile = RedundantFiles.Find(rf => rf.FileName == DgridClones.GetValueAt("Filename"));
            if (SelectedFile != null)
            {
                MItemShowClone.IsEnabled = true;
                MItemShowClone.Opacity = 1;
            }
            else
            {
                MItemShowClone.IsEnabled = false;
                MItemShowClone.Opacity = 0;
            }
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            if(_userChangedState)
                _windowState = WindowState;
        }

        public delegate void ChangeCountDelegate(string count);

        public void ChangeCount(string count)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => LblCount.Content = "Processed : " + count));
            }
            catch
            {}
        }

        public void ChangeFileName(string filename)
        {
            try
            {
                Dispatcher.Invoke(new Action(() => LblFileName.Content = filename));
            }
            catch
            { }
        }
    }
}
