using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace TKControls.ClickableProgressBar
{
    public delegate void OnCompleteEventHandler(object sender, EventArgs args);
    public delegate void OnClickEventHandler(object sender, EventArgs args);
    /// <summary>
    /// Interaction logic for ClickableProgress.xaml
    /// </summary>
    public partial class ClickableProgress : UserControl
    {
        public const float Step = 10;
        public event OnClickEventHandler Click;

        public object LabelContent
        {
            get { return Label.Content; }
            set { Label.Content = value; }
        }

        /// <summary>
        /// The time that to the bar load in ms
        /// </summary>
        public double Time
        {
            get { return ProgressBar.Maximum * 1000; }
            set { ProgressBar.Maximum = value/1000; }
        }

        private Timer _timer;
        public bool Auto { get; set; }

        public Brush TextColor
        {
            get { return Label.Foreground; }
            set { Label.Foreground = value; }
        }

        public Brush BarColor
        {
            get { return ProgressBar.Foreground; }
            set { ProgressBar.Foreground = value; }
        }

        public event OnCompleteEventHandler Completed;

        public ClickableProgress()
        {
            InitializeComponent();
            _timer = new Timer(Step);
            _timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ProgressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
                          new Action(
                            delegate
                            {
                                if (!(Time <= Step/1000))
                                    ProgressBar.Value += Step/1000;

                                if (!(ProgressBar.Value >= ProgressBar.Maximum)) return;
                                OnCompleted(EventArgs.Empty);
                                ProgressBar.Value = Time <= Step ? ProgressBar.Maximum : 0;
                                if (!Auto)
                                    _timer.Enabled = false;
                            }));
        }

        public virtual void OnCompleted(EventArgs e)
        {
            Completed?.Invoke(this, e);
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
