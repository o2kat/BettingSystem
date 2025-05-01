using System;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class AddEventWindow : Window
    {
        public AddEventWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = EventNameTextBox.Text;
            DateTime? date = EventDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(name) || date == null)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (date.Value < DateTime.Today)
            {
                MessageBox.Show("Нельзя создать событие с прошедшей датой.");
                return;
            }

            using (var context = new BettingContext())
            {
                var newEvent = new Event
                {
                    EventName = name,
                    EventDate = date.Value
                };

                context.Events.Add(newEvent);
                context.SaveChanges();
            }

            MessageBox.Show("Событие добавлено.");
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Просто закрываем окно без перехода
        }
    }
}
