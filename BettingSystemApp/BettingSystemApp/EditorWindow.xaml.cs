using System;
using System.Linq;
using System.Timers;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class EditorWindow : Window
    {
        private Timer _clearOldBetsTimer;

        public EditorWindow()
        {
            InitializeComponent();
            LoadBets();
            SetupClearOldBetsTimer();
        }

        private void LoadBets()
        {
            using (var context = new BettingContext())
            {
                BetsListView.ItemsSource = context.Bets
                    .Select(b => new
                    {
                        b.BetID,
                        Match = $"{b.Team1} vs {b.Team2}",
                        b.MatchTime,
                        b.Sport,
                        b.Description,
                        Team1Win = $"{b.Team1Win:F2}",
                        Team2Win = $"{b.Team2Win:F2}",
                        Draw = $"{b.Draw:F2}"
                    }).ToList();
            }
        }

        private void AddBetButton_Click(object sender, RoutedEventArgs e)
        {
            var addBetWindow = new AddBetWindow();
            addBetWindow.ShowDialog();
            LoadBets();
        }

        private void EditBetButton_Click(object sender, RoutedEventArgs e)
        {
            if (BetsListView.SelectedItem != null)
            {
                var selectedItem = BetsListView.SelectedItem;
                var betIdProperty = selectedItem.GetType().GetProperty("BetID");
                if (betIdProperty != null)
                {
                    int betId = (int)betIdProperty.GetValue(selectedItem);
                    using (var context = new BettingContext())
                    {
                        var bet = context.Bets.FirstOrDefault(b => b.BetID == betId);
                        if (bet != null)
                        {
                            var editWindow = new EditBetWindow(bet);
                            editWindow.ShowDialog();
                            LoadBets();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите ставку для редактирования.");
            }
        }

        private void DeleteBetButton_Click(object sender, RoutedEventArgs e)
        {
            if (BetsListView.SelectedItem != null)
            {
                var selectedItem = BetsListView.SelectedItem;
                var betIdProperty = selectedItem.GetType().GetProperty("BetID");
                if (betIdProperty != null)
                {
                    int betId = (int)betIdProperty.GetValue(selectedItem);
                    using (var context = new BettingContext())
                    {
                        var bet = context.Bets.FirstOrDefault(b => b.BetID == betId);
                        if (bet != null)
                        {
                            context.Bets.Remove(bet);
                            context.SaveChanges();
                        }
                    }
                    LoadBets();
                }
            }
            else
            {
                MessageBox.Show("Выберите ставку для удаления.");
            }
        }

        private void ClearPastBets_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var oldBets = context.Bets.Where(b => b.MatchTime < DateTime.Now).ToList();
                context.Bets.RemoveRange(oldBets);
                context.SaveChanges();
            }

            MessageBox.Show("Прошедшие ставки удалены.");
            LoadBets();
        }

        private void SetupClearOldBetsTimer()
        {
            _clearOldBetsTimer = new Timer(86400000); // 24 часа
            _clearOldBetsTimer.Elapsed += (s, e) => ClearPastBetsAuto();
            _clearOldBetsTimer.Start();
        }

        private void ClearPastBetsAuto()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                using (var context = new BettingContext())
                {
                    var oldBets = context.Bets.Where(b => b.MatchTime < DateTime.Now).ToList();
                    context.Bets.RemoveRange(oldBets);
                    context.SaveChanges();
                }
                LoadBets();
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
