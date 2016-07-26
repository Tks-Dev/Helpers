using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YuGiOhCardCreator.Helpers;

namespace Picture_Merger
{
    /// <summary>
    /// Logique d'interaction pour FileNameWindow.xaml
    /// </summary>
    public partial class FileNameWindow : Window
    {
        public MainWindow MW { get; set; }
        private string s;
        public FileNameWindow()
        {
            InitializeComponent();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            MW.savename = System.IO.Path.Combine(s, TextBox.Text);
            DialogResult = true;
        }

        private void FolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            s = Helper.FolderFromBrowser("pouet");
        }
    }
}
