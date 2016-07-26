using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
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

namespace ControlsTester
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double Money
        {
            get { return _money; }
            set
            {
                _money = value;
                _money = Math.Round(_money, 2);
                OnMoneyUpdated();
            }
        }

        private void OnMoneyUpdated()
        {
            Dispatcher.Invoke(() => LblMoney.Content = _money);
        }

        private double _money;
        public double ClickMoney;

        #region Mimi Fields
        public int Mimis;
        public double MimiMoney;
        public double MimiSpeedPrix;
        public double MimiPrix;
        #endregion
        #region Baloo Fields
        public int Baloos;
        public double BalooMoney;
        public double BalooSpeedPrix;
        public double BalooPrix;
        #endregion
        #region Tigroune Fields
        public int Tigrounes;
        public double TigrouneMoney;
        public double TigrouneSpeedPrix;
        public double TigrounePrix;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Money = 0;
            ClickMoney = 1;

            #region Mimi init

            MimiMoney = 5;
            MimiPrix = 3;
            MimiSpeedPrix = 8;

            #endregion

            #region Baloo Init

            BalooPrix = 150;
            BalooMoney = 1200;
            BalooSpeedPrix = 160;

            #endregion

            #region Tigroune Init

            TigrounePrix = 60000;
            TigrouneMoney = 1000000;
            TigrouneSpeedPrix = 70000;

            #endregion

            var grds = new List<Grid>(new[] {
                new Grid { Background = Brushes.Black },
                new Grid { Background = Brushes.Red } });
            GridSlider.ItemsSource = grds;
            GridSlider.CheckEnability();

        }

        #region Mimi Actions
        private void MimiBar_OnCompleted(object sender, EventArgs args)
        {
            Money += Mimis * MimiMoney;
        }

        private void MimiBar_OnClick(object sender, EventArgs args)
        {
            if (!CanBuy(MimiPrix))
                return;
            Mimis++;
            MimiBar.Start();
            Money -= MimiPrix;
            Money = Math.Round(Money, 2);
            MimiPrix = Math.Round((MimiPrix * 1.2), 2);
            Dispatcher.Invoke(() =>
            {
                MimiBar.LabelContent = "Acheter une Mimi pour " + MimiPrix + " papillons";
                MimiBar.ToolTip = "Donne " + MimiMoney * Mimis + " papillons";
            });
            if (Mimis % 10 == 0)
                ClickMoney *= 2;
            if (Mimis % 20 == 0)
                MimiBar.Time *= 0.3;
        }

        private void SpeedMimi_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CanBuy(MimiSpeedPrix))
                return;
            Money -= MimiSpeedPrix;
            MimiBar.Time *= 0.95;
            MimiSpeedPrix *= 2;
            if (MimiBar.Time < 5)
                SpeedMimi.IsEnabled = false;
            SpeedMimi.ToolTip = SpeedMimi.IsEnabled ? (MimiSpeedPrix + " Papillons") : "Vous ne pouvez plus accélérer Mimi";
        }
        #endregion

        #region Baloo Actions
        private void SpeedBaloo_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CanBuy(BalooSpeedPrix))
                return;
            Money -= BalooSpeedPrix;
            BalooBar.Time *= 0.95;
            BalooSpeedPrix *= 2;
            if (BalooBar.Time < 2)
                SpeedBaloo.IsEnabled = false;
            SpeedBaloo.ToolTip = SpeedBaloo.IsEnabled ? (BalooSpeedPrix + " Papillons") : "Vous ne pouvez plus accélérer Baloo";

        }

        private void BalooBar_OnCompleted(object sender, EventArgs args)
        {
            Money += Baloos * BalooMoney;
        }

        private void BalooBar_OnClick(object sender, EventArgs args)
        {
            if (!CanBuy(BalooPrix))
                return;
            Baloos++;
            BalooBar.Start();
            Money -= BalooPrix;
            BalooPrix = Math.Round((BalooPrix * 1.2), 2);
            Dispatcher.Invoke(() =>
            {
                BalooBar.LabelContent = "Acheter un Baloo pour " + BalooPrix + " papillons";
                BalooBar.ToolTip = "Donne " + BalooMoney * Baloos + " papillons";
            });
            if (Baloos % 10 == 0)
                ClickMoney *= 2;
            if (Baloos % 20 == 0)
                BalooBar.Time *= 0.75;
        }

        #endregion

        #region Tigroune Actions
        private void SpeedTigroune_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CanBuy(TigrouneSpeedPrix))
                return;
            Money -= TigrouneSpeedPrix;
            TigrouneBar.Time *= 0.95;
            TigrouneSpeedPrix *= 2;
            if (TigrouneBar.Time < 2)
                SpeedTigroune.IsEnabled = false;
            SpeedTigroune.ToolTip = SpeedTigroune.IsEnabled ? (TigrouneSpeedPrix + " Papillons") : "Vous ne pouvez plus accélérer Tigroune";

        }

        private void TigrouneBar_OnCompleted(object sender, EventArgs args)
        {
            Money += Tigrounes * TigrouneMoney;
        }

        private void TigrouneBar_OnClick(object sender, EventArgs args)
        {
            if (!CanBuy(TigrounePrix))
                return;
            Tigrounes++;
            TigrouneBar.Start();
            Money -= TigrounePrix;
            TigrounePrix = Math.Round((TigrounePrix * 1.05), 2);
            Dispatcher.Invoke(() =>
            {
                TigrouneBar.LabelContent = "Acheter un Tigroune pour " + TigrounePrix + " papillons";
                TigrouneBar.ToolTip = "Donne " + TigrouneMoney * Tigrounes + " papillons";
            });
            if (Tigrounes % 10 == 0)
                ClickMoney *= 2;
            if (Tigrounes % 20 == 0)
                TigrouneBar.Time *= 0.75;
        }

        #endregion

        private bool CanBuy(double prix)
        {
            return Money >= prix;
        }

        private void JackPot_OnClick(object sender, RoutedEventArgs e)
        {
            Money += ClickMoney;
        }

        private void Debug_OnClick(object sender, RoutedEventArgs e)
        {

            //GridSlider.Debug();
        }
    }
}
