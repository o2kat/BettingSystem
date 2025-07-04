using System;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class AddBetWindow : Window
    {
        public AddBetWindow()
        {
            InitializeComponent();
            MatchTimeDatePicker.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Team1TextBox.Text) ||
                string.IsNullOrWhiteSpace(Team2TextBox.Text) ||
                MatchTimeDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(SportTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(Team1WinTextBox.Text) ||
                string.IsNullOrWhiteSpace(Team2WinTextBox.Text) ||
                string.IsNullOrWhiteSpace(DrawTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!decimal.TryParse(Team1WinTextBox.Text, out decimal team1Win) ||
                !decimal.TryParse(Team2WinTextBox.Text, out decimal team2Win) ||
                !decimal.TryParse(DrawTextBox.Text, out decimal draw))
            {
                MessageBox.Show("Пожалуйста, введите корректные коэффициенты.");
                return;
            }

            if (team1Win <= 0 || team2Win <= 0 || draw <= 0)
            {
                MessageBox.Show("Коэффициенты должны быть больше нуля.");
                return;
            }

            var bet = new Bet
            {
                Team1 = Team1TextBox.Text.Trim(),
                Team2 = Team2TextBox.Text.Trim(),
                MatchTime = MatchTimeDatePicker.SelectedDate.Value,
                Sport = SportTextBox.Text.Trim(),
                Description = DescriptionTextBox.Text.Trim(),
                Team1Win = team1Win,
                Team2Win = team2Win,
                Draw = draw
            };

            using (var context = new BettingContext())
            {
                context.Bets.Add(bet);
                context.SaveChanges();
            }

            MessageBox.Show("Ставка успешно добавлена.");
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 