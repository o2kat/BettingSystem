using System;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class EditBetWindow : Window
    {
        private Bet _bet;

        public EditBetWindow(Bet bet)
        {
            InitializeComponent();
            _bet = bet;
            LoadBetData();
        }

        private void LoadBetData()
        {
            Team1TextBox.Text = _bet.Team1;
            Team2TextBox.Text = _bet.Team2;
            MatchTimeDatePicker.SelectedDate = _bet.MatchTime;
            SportTextBox.Text = _bet.Sport;
            DescriptionTextBox.Text = _bet.Description;
            Team1WinTextBox.Text = _bet.Team1Win.ToString("F2");
            Team2WinTextBox.Text = _bet.Team2Win.ToString("F2");
            DrawTextBox.Text = _bet.Draw.HasValue ? _bet.Draw.Value.ToString("F2") : "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Team1TextBox.Text) ||
                string.IsNullOrWhiteSpace(Team2TextBox.Text) ||
                MatchTimeDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(SportTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(Team1WinTextBox.Text) ||
                string.IsNullOrWhiteSpace(Team2WinTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!decimal.TryParse(Team1WinTextBox.Text, out decimal team1Win) ||
                !decimal.TryParse(Team2WinTextBox.Text, out decimal team2Win))
            {
                MessageBox.Show("Пожалуйста, введите корректные коэффициенты.");
                return;
            }

            if (team1Win <= 0 || team2Win <= 0)
            {
                MessageBox.Show("Коэффициенты должны быть больше нуля.");
                return;
            }

            using (var context = new BettingContext())
            {
                var betToUpdate = context.Bets.Find(_bet.BetID);
                if (betToUpdate != null)
                {
                    betToUpdate.Team1 = Team1TextBox.Text.Trim();
                    betToUpdate.Team2 = Team2TextBox.Text.Trim();
                    betToUpdate.MatchTime = MatchTimeDatePicker.SelectedDate.Value;
                    betToUpdate.Sport = SportTextBox.Text.Trim();
                    betToUpdate.Description = DescriptionTextBox.Text.Trim();
                    betToUpdate.Team1Win = team1Win;
                    betToUpdate.Team2Win = team2Win;
                    // Обработка поля Draw (может быть NULL)
                    if (string.IsNullOrWhiteSpace(DrawTextBox.Text))
                    {
                        betToUpdate.Draw = null;
                    }
                    else if (decimal.TryParse(DrawTextBox.Text, out decimal draw))
                    {
                        betToUpdate.Draw = draw;
                    }

                    context.SaveChanges();
                    MessageBox.Show("Ставка успешно обновлена.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ставка не найдена в базе данных.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 