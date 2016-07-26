﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TksHelpers;

namespace TKControls.ClickableImage
{
    public delegate void OnClickEventHandler(object sender, EventArgs args);

    /// <summary>
    /// Logique d'interaction pour ClickableImage.xaml
    /// </summary>
    public partial class ClickableImage : UserControl
    {
        public event OnClickEventHandler Click;

        private bool _alwaysShowLabel;
        public bool AlwaysShowLabel
        {
            get { return _alwaysShowLabel; }
            set
            {
                _alwaysShowLabel = value;
                Label.Visibility = (_alwaysShowLabel || _source == null) ? 
                    Visibility.Visible : Visibility.Hidden;
            }
        }

        public double LabelOpacity
        {
            get { return Label.Opacity; }
            set { Label.Opacity = value; }
        }

        private string _source;

        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                Image.ChangeSource(_source);
                Label.Visibility = AlwaysShowLabel ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public Stretch Stretch
        {
            get { return Image.Stretch; }
            set { Image.Stretch = value; }
        }

        public ClickableImage()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public void ChangeImageSource(string imagePath)
        {
            Source = imagePath;
        }

        public void ChangeImageSource(ImageSource source)
        {
            Source = source.ToString();

        }

        public void ChangeImageSource(ImageSource source, double beginTime, double fadeTime)
        {
            Image.ChangeSource(source, beginTime, fadeTime);
            _source = source.ToString();
        }
    }
}
