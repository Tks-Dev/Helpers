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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TKControls
{
    /// <summary>
    /// Logique d'interaction pour AutoSizedLabel.xaml
    /// </summary>
    public partial class AutoSizedLabel : UserControl
    {
        public string Text
        { 
            get { return Label.Text; }
            set { Label.Text = value; }
        }

        public AutoSizedLabel()
        {
            InitializeComponent();
        }
    }
}
