using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class DirectorWindow : Window
    {
        public DirectorWindow()
        {
            InitializeComponent();
            LoadUsers();
            LoadUserBets();
        }

        private void LoadUsers()
        {
            using (var context = new BettingContext())
            {
                UsersDataGrid.ItemsSource = context.Users
                    .Select(u => new
                    {
                        u.UserID,
                        u.Username,
                        u.Email,
                        u.Balance,
                        u.BetsCount,
                        Role = u.Role.RoleName
                    }).ToList();
            }
        }

        private void LoadUserBets()
        {
            using (var context = new BettingContext())
            {
                UserBetsDataGrid.ItemsSource = context.UserBets
                    .Include("User")
                    .Include("Bet")
                    .ToList()
                    .Select(ub => new
                    {
                        ub.UserBetID,
                        User = ub.User.Username,
                        Match = string.Format("{0} vs {1}", ub.Bet.Team1, ub.Bet.Team2),
                        ub.Amount,
                        ub.Coefficient,
                        ub.TeamWin,
                        ub.DatePlaced,
                        ub.Status
                    }).ToList();
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.");
                return;
            }

            dynamic selected = UsersDataGrid.SelectedItem;
            int id = selected.UserID;

            using (var context = new BettingContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserID == id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }

            LoadUsers();
            LoadUserBets();
            MessageBox.Show("Пользователь удалён.");
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var userReportPath = "UserReport.txt";
            var betReportPath = "BetReport.txt";
            var userBetReportPath = "UserBetReport.txt";
            var directorUserReportPath = "DirectorUserReport.csv";

            using (var context = new BettingContext())
            {
                // ---------- Пользователи ----------
                var users = context.Users.Include("Role").ToList();
                var userReport = new StringBuilder();
                userReport.AppendLine("ОТЧЁТ О ПОЛЬЗОВАТЕЛЯХ");
                userReport.AppendLine("ID\tЛогин\tEmail\tРоль\tБаланс\tКоличество ставок\tДата создания");
                foreach (var user in users)
                {
                    userReport.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4:F2}\t{5}\t{6}", 
                        user.UserID, user.Username, user.Email, 
                        user.Role?.RoleName ?? "Не назначена", user.Balance, user.BetsCount ?? 0, 
                        user.CreatedDate.ToString("yyyy-MM-dd")));
                }
                File.WriteAllText(userReportPath, userReport.ToString());

                // ---------- Ставки ----------
                var bets = context.Bets.ToList();
                var betReport = new StringBuilder();
                betReport.AppendLine("ОТЧЁТ О СТАВКАХ");
                betReport.AppendLine("ID\tКоманда 1\tКоманда 2\tВремя матча\tСпорт\tОписание\tК1\tК2\tКX");
                foreach (var bet in bets)
                {
                    betReport.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6:F2}\t{7:F2}\t{8}", 
                        bet.BetID, bet.Team1, bet.Team2, bet.MatchTime.ToString("yyyy-MM-dd HH:mm"), 
                        bet.Sport, bet.Description, bet.Team1Win, bet.Team2Win, bet.Draw.HasValue ? bet.Draw.Value.ToString("F2") : "N/A"));
                }
                File.WriteAllText(betReportPath, betReport.ToString());

                // ---------- Ставки пользователей ----------
                var userBets = context.UserBets.Include("User").Include("Bet").ToList();
                var userBetReport = new StringBuilder();
                userBetReport.AppendLine("ОТЧЁТ О СТАВКАХ ПОЛЬЗОВАТЕЛЕЙ");
                userBetReport.AppendLine("ID\tПользователь\tМатч\tСумма\tКоэффициент\tКоманда\tДата ставки\tСтатус");
                foreach (var ub in userBets)
                {
                    userBetReport.AppendLine(string.Format("{0}\t{1}\t{2} vs {3}\t{4:F2}\t{5:F2}\t{6}\t{7}\t{8}", 
                        ub.UserBetID, ub.User.Username, ub.Bet.Team1, ub.Bet.Team2, 
                        ub.Amount, ub.Coefficient, ub.TeamWin, ub.DatePlaced.ToString("yyyy-MM-dd HH:mm"), ub.Status));
                }
                File.WriteAllText(userBetReportPath, userBetReport.ToString());

                // ---------- CSV отчёт для директора ----------
                var csvReport = new StringBuilder();
                csvReport.AppendLine("UserID,Username,Email,Role,Balance,BetsCount,CreatedDate");
                foreach (var user in users)
                {
                    csvReport.AppendLine(string.Format("{0},{1},{2},{3},{4:F2},{5},{6}", 
                        user.UserID, user.Username, user.Email, 
                        user.Role?.RoleName ?? "Не назначена", user.Balance, user.BetsCount ?? 0, 
                        user.CreatedDate.ToString("yyyy-MM-dd")));
                }
                File.WriteAllText(directorUserReportPath, csvReport.ToString());
            }

            MessageBox.Show("Отчёты успешно созданы:\n- UserReport.txt\n- BetReport.txt\n- UserBetReport.txt\n- DirectorUserReport.csv", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenReportsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userReport = "UserReport.txt";
                string betReport = "BetReport.txt";
                string userBetReport = "UserBetReport.txt";

                if (File.Exists(userReport))
                    System.Diagnostics.Process.Start("notepad.exe", userReport);
                else
                    MessageBox.Show("Файл отчёта пользователей не найден.");

                if (File.Exists(betReport))
                    System.Diagnostics.Process.Start("notepad.exe", betReport);
                else
                    MessageBox.Show("Файл отчёта ставок не найден.");

                if (File.Exists(userBetReport))
                    System.Diagnostics.Process.Start("notepad.exe", userBetReport);
                else
                    MessageBox.Show("Файл отчёта ставок пользователей не найден.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка при открытии отчётов:\n{0}", ex.Message));
            }
        }

        private void GoToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
