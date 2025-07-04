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
            // Проверка уникальности username и email
            using (var context = new BettingContext())
            {
                // Проверка на существующий username
                if (context.Users.Any(u => u.Username.ToLower() == UsernameTextBox.Text.ToLower()))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует. Выберите другое имя пользователя.");
                    return;
                }

                // Проверка на существующий email
                if (context.Users.Any(u => u.Email.ToLower() == EmailTextBox.Text.ToLower()))
                {
                    MessageBox.Show("Пользователь с таким email уже существует. Используйте другой email.");
                    return;
                }

                DateTime currentDate = DateTime.Now;

                // Проверка границ datetime для SQL Server
                if (currentDate < new DateTime(1753, 1, 1))
                    currentDate = new DateTime(1753, 1, 1);
                if (currentDate > new DateTime(9999, 12, 31))
                    currentDate = new DateTime(9999, 12, 31);

                var newUser = new User
                {
                    Username = UsernameTextBox.Text.Trim(),
                    PasswordHash = HashPassword(PasswordBox.Password),
                    Email = EmailTextBox.Text.Trim(),
                    RoleID = (int)RoleComboBox.SelectedValue,
                    CreatedDate = currentDate,
                    Balance = 0.00m, // Начальный баланс
                    BetsCount = 0    // Начальное количество ставок
                };

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
            // Базовая валидация
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите роль.");
                return;
            }

            // Валидация длины username
            if (UsernameTextBox.Text.Trim().Length < 3)
            {
                MessageBox.Show("Имя пользователя должно содержать минимум 3 символа.");
                return;
            }

            // Валидация длины пароля
            if (PasswordBox.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.");
                return;
            }

            // Простая валидация email
            if (!EmailTextBox.Text.Contains("@") || !EmailTextBox.Text.Contains("."))
            {
                MessageBox.Show("Пожалуйста, введите корректный email адрес.");
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
