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
using TKControls;
using Xceed.Wpf.Toolkit;
using YuGiOhCardCreator.Business;
using YuGiOhCardCreator.Helpers;
using Coordonates = YuGiOhCardCreator.Helpers.Coordonates;
using ExtensionsMethods = YuGiOhCardCreator.Helpers.ExtensionsMethods;
using Path = System.IO.Path;

namespace YuGiOhCardCreator
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CardType> CardsType { get; set; }
        public List<SpecificCardType> SpecificCardsTypes { get; set; }
        private bool _onSpec;
        private bool _onType;
        private bool _filled;
        public string Pictures { get; private set; }

        // Elements de l'interface dependant du type de carte
        public ComboBox CboxAttributes;
        public Grid GridAttributes;
        public IntegerUpDown UpDoLvl;
        public AutoSizedLabel MagicTrapLabel;

        public MainWindow()
        {
            Pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            InitializeComponent();
            //CardBack.ChangeSource(@"I:\Programs\mse3\data\yugioh-standard.mse-style\card-dragons.png");
            CardsType = new List<CardType>()
            {
                CardType.Spell,
                CardType.Monster,
                CardType.Trap
            };

            SpecificCardsTypes = new List<SpecificCardType>
            {
                SpecificCardType.NormalMonster,
                SpecificCardType.EffectMonster,
                SpecificCardType.FusionMonster,
                SpecificCardType.RitualMonster,
                SpecificCardType.SynchroMonster,
                SpecificCardType.XyzMonster,
                SpecificCardType.PendulumMonster,
                SpecificCardType.NormalSpell,
                SpecificCardType.SpeedSpell,
                SpecificCardType.ContinuousSpell,
                SpecificCardType.RitualSpell,
                SpecificCardType.EquipmentSpell,
                SpecificCardType.FieldSpell,
                SpecificCardType.NormalTrap,
                SpecificCardType.CounterTrap,
                SpecificCardType.ContinuousTrap
            };

            MagicTrapLabel = new AutoSizedLabel { FontFamily = new FontFamily("MatrixRegularSmallCaps"), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };

            CboxCardType.ItemsSource = CardsType;
            CboxCardSpecType.ItemsSource = SpecificCardsTypes;


            GridInnerCard.Height = CardBack.ActualHeight;
        }


        private void UpdateEnabilitiesAndOpacities(SpecificCardType specificCard)
        {
            if (specificCard.Parent != CardType.Monster)
            {
                UpdateEnabilitiesAndOpacities(specificCard.Parent);
                return;
            }
            CboxSubTypes.ItemsSource = specificCard.Subtypes;
            //GboxMagicDetails.Opacity = 0;
            GboxMonster.Opacity = specificCard != SpecificCardType.NormalMonster ? 1 : 0;

            //GboxNormalMonster.Opacity = specificCard == SpecificCardType.NormalMonster ? 1 : 0;
            //GboxTrapDetails.Opacity = 0;
            CboxSubTypes.IsEnabled = specificCard.Subtypes != null;
            GboxMonster.Focus();
            GridInnerCard.Height = CardBack.ActualHeight;
            UpdateElements(specificCard.Parent);
        }

        private void UpdateEnabilitiesAndOpacities(CardType cardType)
        {
            //if (cardType == CardType.Monster) return;
            var isSpell = cardType == CardType.Spell;
            //GboxMagicDetails.Opacity = isSpell ? 1 : 0;
            GboxMonster.Opacity = 1;
            //GboxNormalMonster.Opacity = 0;
            //GboxTrapDetails.Opacity = isSpell ? 0 : 1;
            //if (isSpell)
            //    GboxMagicDetails.Focus();
            //else
            //    GboxTrapDetails.Focus();
            GridInnerCard.Height = CardBack.ActualHeight;
            UpdateElements(cardType);
        }

        private void UpdateElements(CardType cardType)
        {
            #region Init Cbox Attribute
            if (CboxAttributes == null)
            {
                CboxAttributes = new ComboBox
                {
                    ItemsSource = new List<Image>()
                    {
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Light.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Dark.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Fire.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Water.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Earth.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Wind.png"))
                        },
                        new Image()
                        {
                            Source = Helper.ImageSourceFromPath(Path.Combine(Pictures, @"YCC\Attributes\Divine.png"))
                        }
                    },
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(80, 10, 10, 5),
                    Width = 65
                };
                foreach (Image image in CboxAttributes.ItemsSource)
                {
                    image.Height = image.Width = 25;
                }
                CboxAttributes.SelectionChanged += CboxAttributes_SelectedItemchanged;
            }
            #endregion
            #region Init IntegerUpDown Level
            if (UpDoLvl == null)
            {
                UpDoLvl = new IntegerUpDown() { Minimum = 1, Maximum = 12, Margin = new Thickness(60, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center, Height = 26, DefaultValue = 1 };
                UpDoLvl.ValueChanged += UpDoLvl_valueChanged;
                UpDoLvl.Value = 1;
            }
            #endregion
            #region Init GridAttributes
            if (GridAttributes == null)
            {
                GridAttributes = new Grid();
                GridAttributes.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                GridAttributes.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                var gridAttribute = new Grid();
                var LblAttributes = new Label()
                {
                    Content = "Attribute : ",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(10, 0, 0, 0),
                    Foreground = Brushes.White
                };
                gridAttribute.Children.Add(LblAttributes);
                gridAttribute.Children.Add(CboxAttributes);
                var gridLvl = new Grid();
                var lblLvl = new Label
                {
                    Content = "Level : ",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                gridLvl.Children.Add(lblLvl);
                gridLvl.Children.Add(UpDoLvl);
                GridAttributes.Children.Add(gridAttribute);
                Grid.SetColumn(gridAttribute, 0);
                GridAttributes.Children.Add(gridLvl);
                Grid.SetColumn(gridLvl, 1);

            }
            #endregion
            #region Mise en place grid attributes
            if (cardType == CardType.Monster)
            {
                if (!StckPanel.Children.Contains(GridAttributes))
                    StckPanel.Children.Add(GridAttributes);
                UpDoLvl_valueChanged(null, null);
            }
            else
            {
                try
                {
                    StckPanel.Children.Clear();
                    StckLevel.Children.Clear();
                    MagicTrapLabel.Height = StckLevel.ActualHeight;
                    StckLevel.Children.Add(MagicTrapLabel);
                    MagicTrapLabel.Content = cardType == CardType.Spell ? "[Spell Card]" : "[Trap Card]";
                }
                catch
                {
                    // ignored
                }

                MagicTrapLabel.Height = StckLevel.ActualHeight;
            }
            #endregion
        }

        private void ToggleDebug()
        {
            GridInnerCard.Height = CardBack.ActualHeight;
            if (!_filled)
            {
                r1.Fill = Brushes.Blue;
                //r3.Fill = Brushes.Red;
                //r5.Fill = Brushes.White;
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                r1.Fill /*= r5.Fill*/ = Brushes.Transparent));
            }


            LabelBottom.Opacity = LabelLeft.Opacity = LabelRight.Opacity = LabelTop.Opacity = _filled ? 0 : 1;

            _filled = !_filled;
            GridInnerCard.Height = CardBack.ActualHeight;
        }

        #region User Events
        private void TboxCardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TblockCardName.Text = TboxCardName.Text;
            GridInnerCard.Height = CardBack.ActualHeight;
        }

        private void CboxAttributes_SelectedItemchanged(object sender, SelectionChangedEventArgs e)
        {
            var image = CboxAttributes.SelectedItem as Image;
            if (image != null)
                ImgAttribute.ChangeSource(image.Source.ToString());
            GridInnerCard.Height = CardBack.ActualHeight;
        }

        private void UpDoLvl_valueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            StckLevel.Children.Clear();
            for (var i = 0; i < UpDoLvl.Value; i++)
            {
                var img = new Image() { Height = StckLevel.ActualHeight };
                ExtensionsMethods.ChangeSource(img, Path.Combine(Pictures, @"YCC\Level.png"));
                StckLevel.Children.Add(img);
                GridInnerCard.Height = CardBack.ActualHeight;
            }
        }

        private void CardImage_OnClick(object sender, EventArgs args)
        {
            var imagePath = Helper.FileFromBrowser(string.Empty, Pictures);
            if (imagePath == null) return;
            try
            {
                CardImage.Stretch = Stretch.Fill;
                CardImage.ChangeSource(imagePath);
                LblCardImage.Content = imagePath;
            }
            catch
            {
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //var srcCoords = string.Empty;
            //var coords = CardBack.GetSourceCoordonates();
            //srcCoords = coords.Keys.Aggregate(srcCoords, (current, k) => current + (k.ToString() + " : " + coords[k] + "; "));
            /*MessageBox.Show("L : " + CardBack.Margin.Left
                            + "\nT : " + CardBack.Margin.Top
                            + "\nR : " + CardBack.Margin.Right
                            + "\nB : " + CardBack.Margin.Bottom
                            + "\nStretch Direction : " + CardBack.StretchDirection
                            + "\nImage Coordantes : " + srcCoords);*/
            ToggleDebug();
            //CardBack.GetSourceCoordonates();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var c = ExtensionsMethods.GetSourceCoordonates(CardBack);
                    LabelBottom.Content = "Bottom : " + c[Coordonates.Bottom];
                    LabelTop.Content = "Top : " + c[Coordonates.Top];
                    LabelLeft.Content = "Left : " + c[Coordonates.Left];
                    LabelRight.Content = "Right : " + c[Coordonates.Right];
                    GridInnerCard.Margin = c.AsThickness();
                    GridInnerCard.Height = CardBack.ActualHeight;
                });
            }
            catch
            {
                // ignored
            }
        }

        private void CboxCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_onSpec) return;
            _onType = true;
            var t = CboxCardType.SelectedValue as CardType;
            CboxCardSpecType.ItemsSource = SpecificCardsTypes.Where(sct => sct.Parent.ToString() == t.ToString());
            _onType = false;
            if (t == CardType.Spell)
            {
                ExtensionsMethods.ChangeSource(CardBack, Path.Combine(Pictures,
                    @"YCC\card-spell.png"));
                ImgAttribute.ChangeSource(Path.Combine(Pictures, @"YCC\Attributes\spell.png"));
            }
            else if (t == CardType.Trap)
            {
                ExtensionsMethods.ChangeSource(CardBack, Path.Combine(Pictures,
                    @"YCC\card-trap.png"));
                ImgAttribute.ChangeSource(Path.Combine(Pictures, @"YCC\Attributes\trap.png"));
            }

            UpdateEnabilitiesAndOpacities(t);
            GridInnerCard.Height = CardBack.ActualHeight;
        }

        private void CboxCardSpecType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_onType) return;
            _onSpec = true;
            var selectedItem = CboxCardSpecType.SelectedValue as SpecificCardType;
            CboxCardType.SelectedValue = selectedItem.Parent;
            CboxCardSpecType.ItemsSource =
                SpecificCardsTypes.Where(sct => sct.Parent.ToString() == selectedItem.Parent.ToString());
            CboxCardSpecType.SelectedValue = selectedItem;
            if (selectedItem.GetLayoutName() != null)
            {
                CardBack.Source =
                    Helper.ImageSourceFromPath(Path.Combine(Pictures,
                        @"YCC\" + selectedItem.GetLayoutName()));
            }
            //}
            //else
            //{
            //    if (selectedItem.Subtypes != null)
            //    {
            //        CboxSubTypes.ItemsSource = selectedItem.Subtypes;
            //        GboxMagicDetails.Opacity = 0;
            //        GboxMonster.Opacity = 1;
            //        GboxMonster.Focus();
            //        GboxNormalMonster.Opacity = 0;
            //        GboxTrapDetails.Opacity = 0;
            //        CboxSubTypes.IsEnabled = true;
            //    }
            //    else
            //    {
            //        CboxSubTypes.ItemsSource = null;
            //        CboxSubTypes.IsEnabled = false;
            //    }
            //}

            UpdateEnabilitiesAndOpacities(selectedItem);
            GridInnerCard.Height = CardBack.ActualHeight;

            _onSpec = false;
        }

        private void CboxSubTypes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = CboxSubTypes.SelectedItem as SpecificCardType;
            if (selectedItem == null) return;
            CardBack.Source =
                    Helper.ImageSourceFromPath(Path.Combine(Pictures,
                        @"YCC\" + selectedItem.GetLayoutName()));
            GridInnerCard.Height = CardBack.ActualHeight;
        }
        #endregion
    }
}

