using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BettingSystemApp.Models;
using System.Linq;

namespace BettingSystemApp
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            LoadRoles();
        }

        // Метод загрузки ролей в ComboBox
        private void LoadRoles()
        {
            using (var context = new BettingContext())
            {
                // Исключаем роль "User" (предположим, её RoleID = 4)
                RoleComboBox.ItemsSource = context.Roles
                    .Where(r => r.RoleID != 4)
                    .ToList();
            }
        }

        // Метод регистрации пользователя
        private void RegisterUser()
        {
            DateTime currentDate = DateTime.Now;

            // Проверка границ datetime для SQL Server
            if (currentDate < new DateTime(1753, 1, 1))
                currentDate = new DateTime(1753, 1, 1);
            if (currentDate > new DateTime(9999, 12, 31))
                currentDate = new DateTime(9999, 12, 31);

            var newUser = new User
            {
                Username = UsernameTextBox.Text,
                PasswordHash = HashPassword(PasswordBox.Password),
                Email = EmailTextBox.Text,
                RoleID = (int)RoleComboBox.SelectedValue,
                CreatedDate = currentDate
            };

            using (var context = new BettingContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }

            MessageBox.Show("Пользователь успешно зарегистрирован!");

            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // Хэширование пароля с SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите роль.");
                return;
            }

            RegisterUser();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
