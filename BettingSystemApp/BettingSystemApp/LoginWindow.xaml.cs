using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Linq;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text?.Trim();
            string password = PasswordBox.Password;

            // Валидация входных данных
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (var context = new BettingContext())
                {
                    // Сначала ищем пользователя по username (регистронезависимо)
                    var user = context.Users
                        .Include("Role")
                        .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

                    if (user == null)
                    {
                        MessageBox.Show("Пользователь с таким именем не найден.");
                        return;
                    }

                    // Проверяем пароль
                    if (user.PasswordHash != hashedPassword)
                    {
                        MessageBox.Show("Неверный пароль.");
                        return;
                    }

                    MessageBox.Show(string.Format("Добро пожаловать, {0}!", user.Username));

                    // Открываем соответствующее окно в зависимости от роли
                    switch (user.RoleID)
                    {
                        case 1: // Admin
                            new AdminWindow().Show();
                            break;
                        case 2: // Editor
                            new EditorWindow().Show();
                            break;
                        case 3: // Director
                            new DirectorWindow().Show();
                            break;
                        case 4: // User
                            MessageBox.Show("Доступ для обычных пользователей пока не реализован.");
                            return;
                        default:
                            MessageBox.Show("Роль не распознана.");
                            return;
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка при входе в систему: {0}", ex.Message));
            }
        }

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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
