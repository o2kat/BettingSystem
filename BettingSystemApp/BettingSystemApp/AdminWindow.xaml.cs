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
            LoadEvents();
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
                        Role = u.Role.RoleName
                    }).ToList();
            }
        }

        private void LoadEvents()
        {
            using (var context = new BettingContext())
            {
                UsersDataGrid.ItemsSource = context.Events.ToList();
            }
        }

        private void ArchiveUsersButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var users = context.Users.Include("Role").ToList();
                var csv = new StringBuilder();

                // Обновлённый заголовок
                csv.AppendLine("UserID,Username,Email,PasswordHash,Role");

                foreach (var user in users)
                {
                    csv.AppendLine($"{user.UserID},{user.Username},{user.Email},{user.PasswordHash},{user.Role?.RoleName}");
                }

                File.WriteAllText("UsersArchive.csv", csv.ToString());
                File.WriteAllText("UsersArchive.txt", csv.ToString());
                MessageBox.Show("Пользователи успешно заархивированы.");
            }
        }


        private void ArchiveEventsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var events = context.Events.ToList();
                var sb = new StringBuilder();
                sb.AppendLine("EventID,EventName,EventDate");

                foreach (var ev in events)
                {
                    sb.AppendLine($"{ev.EventID},{ev.EventName},{ev.EventDate:yyyy-MM-dd}");
                }

                File.WriteAllText("EventsArchive.csv", sb.ToString());
                File.WriteAllText("EventsArchive.txt", sb.ToString());
                MessageBox.Show("События заархивированы.");
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
                    if (values.Length >= 5)
                    {
                        string roleName = values[4].Trim();
                        var role = context.Roles.FirstOrDefault(r => r.RoleName == roleName);

                        if (role == null)
                            continue;

                        // Безопасный разбор
                        if (int.TryParse(values[0], out int userId))
                        {
                            var user = new User
                            {
                                UserID = userId,
                                Username = values[1],
                                Email = values[2],
                                PasswordHash = values[3],
                                RoleID = role.RoleID,
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




        private void UnarchiveEventsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("EventsArchive.csv"))
            {
                MessageBox.Show("Файл архива событий не найден.");
                return;
            }

            var lines = File.ReadAllLines("EventsArchive.csv").Skip(1);
            using (var context = new BettingContext())
            {
                context.Events.RemoveRange(context.Events);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 && DateTime.TryParse(parts[2], out DateTime parsedDate))
                    {
                        context.Events.Add(new Event
                        {
                            EventName = parts[1],
                            EventDate = parsedDate
                        });
                    }
                }
                context.SaveChanges();
                LoadEvents();
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
