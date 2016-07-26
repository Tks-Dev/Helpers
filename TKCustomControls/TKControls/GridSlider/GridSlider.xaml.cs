using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TksHelpers;

namespace TKControls.GridSlider
{
    /// <summary>
    /// Logique d'interaction pour GridSlider.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class GridSlider : UserControl/*, IList<Grid>*/
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(typeof(GridSlider));

        public IEnumerable<Grid> ItemsSource
        {
            get { return (IEnumerable<Grid>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        //private ICollection<Grid> items; 
        //public ICollection<Grid> Items { get { return items; } set { items = value; } } 
        private int _position;
        private int _delay;
        private int _duration;

        private int _percent = 20;

        
        public int ButtonsOccupation
        {
            get { return _percent; }
            set
            {
                _percent = value;
                GridButtons.ColumnDefinitions[0].Width = new GridLength(_percent / 2, GridUnitType.Star);
                GridButtons.ColumnDefinitions[2].Width = new GridLength(_percent / 2, GridUnitType.Star);
                GridButtons.ColumnDefinitions[1].Width = new GridLength(100 - _percent, GridUnitType.Star);
            }
        }

        public VerticalAlignment VerticalButtonsAlignements
        {
            get { return MoveLeft.VerticalAlignment; }
            set
            {
                MoveLeft.VerticalAlignment = MoveRight.VerticalAlignment = value;
            }
        }
        public double ButtonsOpacity
        {
            get { return MoveLeft.Opacity; }
            set
            {
                MoveLeft.Opacity = MoveRight.Opacity = value;
            }
        }

        public Brush ButtonsBackgroundBrush
        {
            get { return MoveLeft.Background; }
            set { MoveLeft.Background = MoveRight.Background = value; }
        }

        public Button LeftButton => MoveLeft;
        public Button RightButton => MoveRight;


        public ItemCollection Items => _itemsControl.Items;
        //private List<Grid> items;
        public int Delay
        {
            get { return _delay; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("The delay must be >= 0");
                _delay = value;
            }
        }

        public int Duration
        {
            get { return _duration; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("The delay must be >= 0");
                _duration = value;
            }
        }

        public GridSlider()
        {
            //items = new List<FrameworkElement>();
            ItemsSource = new List<Grid>();
            _position = -1;
            InitializeComponent();
            CheckEnability();
        }

        private void MoveLeft_OnClick(object sender, RoutedEventArgs e)
        {
            MoveToLeft();
        }

        private void MoveRight_OnClick(object sender, RoutedEventArgs e)
        {
            MoveToRight();
        }

        private void MoveToLeft()
        {

            MessageBox.Show(_position + " -- L\n" + string.Join("\n", ItemsSource.Count()));
            if (_position <= 0) return;
            foreach (var frameworkElement in ItemsSource)
            {
                frameworkElement.Slide((uint)ActualWidth, _delay, _duration, UiElementExtension.OffsetDirection.Right);
            }
            _position--;
            CheckEnability();
        }

        public void CheckEnability()
        {
            MoveLeft.IsEnabled = _position > 0;
            MoveRight.IsEnabled = _position < ItemsSource.Count() - 1;
            //MessageBox.Show(_position + "\n" + ItemsSource.Count());
        }

        private void MoveToRight()
        {
            if (_position == -1 && ItemsSource.Count() > 0)
                _position = 0;
            MessageBox.Show(_position + " -- R\n" + string.Join("\n", ItemsSource.Count()));
            if (_position == ItemsSource.Count() - 1) return;
            foreach (var frameworkElement in ItemsSource)
            {
                frameworkElement.Slide((uint)ActualWidth, _delay, _duration, UiElementExtension.OffsetDirection.Left, new Action(()=> ms(frameworkElement)));
            }
            _position++;
            Debug();
            CheckEnability();
        }

        public void Add(Grid grid)
        {
            var itemList = ItemsSource.ToList();
            //itemList.Add();
            for (var i = 0; i < itemList.Count; i++)
            {
                var element = itemList[i];
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.VerticalAlignment = VerticalAlignment.Stretch;
                element.Margin = new Thickness(0 + ActualWidth * (i - _position), 0, 0 - ActualWidth * (i - _position), 0);
            }
            ItemsSource = itemList;
        }

        private void GridSlider_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var itemList = ItemsSource.ToList();
            for (var i = 0; i < itemList.Count; i++)
            {
                var element = itemList[i];
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.VerticalAlignment = VerticalAlignment.Stretch;
                element.Margin = new Thickness(0 + ActualWidth * (i - (_position > -1 ? _position : 0)), 0, 0 - ActualWidth * (i - (_position > -1 ? _position : 0)), 0);
            }
        }

        public void Debug()
        {
            MessageBox.Show(string.Join("\n", ItemsSource.Select(i => "- " + i.ActualWidth + "; " + i.ActualHeight + "; [" + i.Margin + "]; " + i.Background)));
        }

        private void ms(Grid grd)
        {
            MessageBox.Show(grd.Margin.ToString());
        }
    }
}
