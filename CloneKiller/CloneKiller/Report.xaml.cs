using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using CloneKiller.Business;
using CloneKiller.Helpers;
using ZetaLongPaths;

namespace CloneKiller
{
    /// <summary>
    /// Logique d'interaction pour Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public RedundantFile RedundantFile { get; set; }
        public List<ZlpFileInfo> Saved { get; private set; }
        public List<ZlpFileInfo> Edited { get; private set; }
        public bool DeleteCompleted { get; set; }

        public Report(RedundantFile r)
        {
            RedundantFile = r;
            Saved = r.Files;
            Edited = new List<ZlpFileInfo>();
            InitializeComponent();
            UpdateDataGrids();
            DeleteCompleted = false;
        }

        private void UpdateDataGrids()
        {
            DGridSavedFiles.ItemsSource = (from tab in Saved
                                           select new
                                           {
                                               Path = tab.FullName,
                                               Weight = tab.Length
                                           });

            DGridEditedFiles.ItemsSource = (from tab in Edited
                                            select new
                                            {
                                                Path = tab.FullName,
                                                Weight = tab.Length
                                            });
            Edit.IsEnabled = Saved.Count > 0;
            BtnDeleteFiles.IsEnabled = Save.IsEnabled = Edited.Count > 0;
        }

        private void Report_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DGridSavedFiles.Margin = new Thickness(0,0,Width/2 + 50,0);
            DGridEditedFiles.Margin = new Thickness(Width / 2 + 50, 0, 0, 0);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var path = DGridSavedFiles.GetValueAt("Path");
            if (path == string.Empty) return;
            var fil = Saved.Find(zlpf => zlpf.FullName == path);
            Edited.Add(fil);
            Saved.Remove(fil);
            UpdateDataGrids();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var path = DGridEditedFiles.GetValueAt("Path");
            if (path == string.Empty) return;
            var fil = Edited.Find(zlpf => zlpf.FullName == path);
            Saved.Add(fil);
            Edited.Remove(fil);
            UpdateDataGrids();
        }

        private void Report_StateChanged(object sender, EventArgs e)
        {
            DGridSavedFiles.Margin = new Thickness(0, 0, Width / 2 + 50, 0);
            DGridEditedFiles.Margin = new Thickness(Width / 2 + 50, 0, 0, 0);   
        }

        private void BtnDeleteFiles_Click(object sender, RoutedEventArgs e)
        {
            if (ChkBoxGenerateReport.IsChecked ?? false)
                if (!GenerateReport()) return;
            foreach (var zlpFileInfo in Edited.Where(zlpFileInfo => zlpFileInfo.Exists))
            {
                zlpFileInfo.Delete();
            }
            DeleteCompleted = true;
            Close();
        }

        private bool GenerateReport()
        {
            var srf = new SelectReportFolder();
            var result = srf.ShowDialog();
            if (!(result ?? false)) return false;

            var fileName = DateTime.Now.ToString("yyyy MM dd - HH mm ss") + " - Clone Killer Report.txt";
            fileName = srf.Folder + @"\" + fileName;
            using (var sw = File.CreateText(fileName))
            {
                try
                {
                    sw.WriteLine("Saved files (" + (Saved.Sum(zlpFileInfo => zlpFileInfo.Length)) + " bytes) : ");
                    foreach (var zlpFileInfo in Saved)
                        sw.WriteLine("Path : " + zlpFileInfo.FullName + " -- Size (bytes) : " + zlpFileInfo.Length);
                    sw.WriteLine("Deleted files (" + Edited.Sum(zlpFileInfo => zlpFileInfo.Length) + " bytes) : ");
                    foreach (var zlpFileInfo in Edited)
                        sw.WriteLine("Path : " + zlpFileInfo.FullName + " -- Size (bytes) : " + zlpFileInfo.Length);
                    sw.Close();
                    sw.Dispose();
                    MessageBox.Show("Report generated at : " + fileName, "Report Generated", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Report not generated due of an exception.\nDeletion canceled ",
                        "Report not Generated",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private void Report_OnClosing(object sender, CancelEventArgs e)
        {
            if (DeleteCompleted)
                DialogResult = true;
            for (var index = 0; index < Edited.Count; index++)
            {
                var zlpFileInfo = Edited[index];
                Saved.Add(zlpFileInfo);
            }
            DialogResult = false;
        }
    }
}
