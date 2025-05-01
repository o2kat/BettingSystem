using System;
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
            MessageBox.Show("Пользователь удалён.");
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var userReportPath = "UserReport.txt";
            var eventReportPath = "EventReport.txt";

            using (var context = new BettingContext())
            {
                // ---------- Пользователи ----------
                var users = context.Users.Include("Role").ToList();
                var userReport = new StringBuilder();
                userReport.AppendLine("ОТЧЁТ О ПОЛЬЗОВАТЕЛЯХ");
                userReport.AppendLine("ID\tЛогин\tEmail\tРоль\tДата создания");
                foreach (var user in users)
                {
                    userReport.AppendLine($"{user.UserID}\t{user.Username}\t{user.Email}\t{user.Role?.RoleName ?? "Не назначена"}\t{user.CreatedDate:yyyy-MM-dd}");
                }
                File.WriteAllText(userReportPath, userReport.ToString());

                // ---------- События ----------
                var events = context.Events.ToList();
                var eventReport = new StringBuilder();
                eventReport.AppendLine("ОТЧЁТ О СОБЫТИЯХ");
                eventReport.AppendLine("ID\tНазвание события\tДата");
                foreach (var ev in events)
                {
                    eventReport.AppendLine($"{ev.EventID}\t{ev.EventName}\t{ev.EventDate:yyyy-MM-dd}");
                }
                File.WriteAllText(eventReportPath, eventReport.ToString());
            }

            MessageBox.Show("Отчёты успешно созданы:\n- UserReport.txt\n- EventReport.txt", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OpenReportsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userReport = "UserReport.txt";
                string eventReport = "EventReport.txt";

                if (File.Exists(userReport))
                    System.Diagnostics.Process.Start("notepad.exe", userReport);
                else
                    MessageBox.Show("Файл отчёта пользователей не найден.");

                if (File.Exists(eventReport))
                    System.Diagnostics.Process.Start("notepad.exe", eventReport);
                else
                    MessageBox.Show("Файл отчёта событий не найден.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии отчётов:\n{ex.Message}");
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
