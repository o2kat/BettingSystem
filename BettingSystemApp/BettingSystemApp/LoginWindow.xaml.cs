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
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            string hashedPassword = HashPassword(password);

            using (var context = new BettingContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {user.Username}!");

                    switch (user.RoleID)
                    {
                        case 1:
                            new AdminWindow().Show(); break;
                        case 2:
                            new EditorWindow().Show(); break;
                        case 3:
                            new DirectorWindow().Show(); break;
                        default:
                            MessageBox.Show("Роль не распознана."); break;
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверные учётные данные.");
                }
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
