using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            LoadUsers();
            LoadBets();
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

        private void LoadBets()
        {
            using (var context = new BettingContext())
            {
                BetsDataGrid.ItemsSource = context.Bets
                    .Select(b => new
                    {
                        b.BetID,
                        b.Team1,
                        b.Team2,
                        b.MatchTime,
                        b.Sport,
                        b.Description,
                        b.Team1Win,
                        b.Team2Win,
                        b.Draw
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
                    .Select(ub => new
                    {
                        ub.UserBetID,
                        User = ub.User.Username,
                        Match = $"{ub.Bet.Team1} vs {ub.Bet.Team2}",
                        ub.Amount,
                        ub.DatePlaced,
                        ub.Status
                    }).ToList();
            }
        }

        private void ArchiveUsersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var users = context.Users.Include("Role").ToList();
                var csv = new StringBuilder();

                // Обновлённый заголовок
                csv.AppendLine("UserID,Username,Email,PasswordHash,Role,Balance,BetsCount");

                foreach (var user in users)
                {
                    csv.AppendLine($"{user.UserID},{user.Username},{user.Email},{user.PasswordHash},{user.Role?.RoleName},{user.Balance},{user.BetsCount}");
                }

                File.WriteAllText("UsersArchive.csv", csv.ToString());
                File.WriteAllText("UsersArchive.txt", csv.ToString());
                MessageBox.Show("Пользователи успешно заархивированы.");
            }
        }

        private void ArchiveBetsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var bets = context.Bets.ToList();
                var sb = new StringBuilder();
                sb.AppendLine("BetID,Team1,Team2,MatchTime,Sport,Description,Team1Win,Team2Win,Draw");

                foreach (var bet in bets)
                {
                    sb.AppendLine($"{bet.BetID},{bet.Team1},{bet.Team2},{bet.MatchTime:yyyy-MM-dd HH:mm},{bet.Sport},{bet.Description},{bet.Team1Win},{bet.Team2Win},{bet.Draw}");
                }

                File.WriteAllText("BetsArchive.csv", sb.ToString());
                File.WriteAllText("BetsArchive.txt", sb.ToString());
                MessageBox.Show("Ставки заархивированы.");
            }
        }

        private void ArchiveUserBetsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var userBets = context.UserBets.Include("User").Include("Bet").ToList();
                var sb = new StringBuilder();
                sb.AppendLine("UserBetID,UserID,Username,BetID,Team1,Team2,Amount,DatePlaced,Status");

                foreach (var ub in userBets)
                {
                    sb.AppendLine($"{ub.UserBetID},{ub.UserID},{ub.User.Username},{ub.BetID},{ub.Bet.Team1},{ub.Bet.Team2},{ub.Amount},{ub.DatePlaced:yyyy-MM-dd HH:mm},{ub.Status}");
                }

                File.WriteAllText("UserBetsArchive.csv", sb.ToString());
                File.WriteAllText("UserBetsArchive.txt", sb.ToString());
                MessageBox.Show("Ставки пользователей заархивированы.");
            }
        }

        private void UnarchiveUsersButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = "UsersArchive.csv";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл архива пользователей не найден.");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
            {
                MessageBox.Show("Файл архива пуст или повреждён.");
                return;
            }

            var tempUsers = new List<User>();

            using (var context = new BettingContext())
            {
                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(',');
                    if (values.Length >= 7)
                    {
                        string roleName = values[4].Trim();
                        var role = context.Roles.FirstOrDefault(r => r.RoleName == roleName);

                        if (role == null)
                            continue;

                        // Безопасный разбор
                        if (int.TryParse(values[0], out int userId) &&
                            decimal.TryParse(values[5], out decimal balance) &&
                            int.TryParse(values[6], out int betsCount))
                        {
                            var user = new User
                            {
                                UserID = userId,
                                Username = values[1],
                                Email = values[2],
                                PasswordHash = values[3],
                                RoleID = role.RoleID,
                                Balance = balance,
                                BetsCount = betsCount,
                                CreatedDate = DateTime.Now
                            };

                            tempUsers.Add(user);
                        }
                    }
                }

                if (tempUsers.Count == 0)
                {
                    MessageBox.Show("Не удалось восстановить ни одного пользователя. Проверьте содержимое архива.");
                    return;
                }

                // Удаляем старых пользователей и добавляем новых
                context.Users.RemoveRange(context.Users);
                context.Users.AddRange(tempUsers);
                context.SaveChanges();
            }

            MessageBox.Show("Пользователи успешно восстановлены из архива.");
            LoadUsers();
        }

        private void UnarchiveBetsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("BetsArchive.csv"))
            {
                MessageBox.Show("Файл архива ставок не найден.");
                return;
            }

            var lines = File.ReadAllLines("BetsArchive.csv").Skip(1);
            using (var context = new BettingContext())
            {
                context.Bets.RemoveRange(context.Bets);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 9 && DateTime.TryParse(parts[3], out DateTime parsedDate) &&
                        decimal.TryParse(parts[6], out decimal team1Win) &&
                        decimal.TryParse(parts[7], out decimal team2Win) &&
                        decimal.TryParse(parts[8], out decimal draw))
                    {
                        context.Bets.Add(new Bet
                        {
                            Team1 = parts[1],
                            Team2 = parts[2],
                            MatchTime = parsedDate,
                            Sport = parts[4],
                            Description = parts[5],
                            Team1Win = team1Win,
                            Team2Win = team2Win,
                            Draw = draw
                        });
                    }
                }
                context.SaveChanges();
                LoadBets();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
