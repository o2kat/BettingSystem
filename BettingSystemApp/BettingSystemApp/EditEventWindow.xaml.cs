using System;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class EditEventWindow : Window
    {
        private readonly Event _event;

        public EditEventWindow(Event ev)
        {
            InitializeComponent();
            _event = ev;

            EventNameTextBox.Text = ev.EventName;
            EventDatePicker.SelectedDate = ev.EventDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = EventNameTextBox.Text;
            DateTime? date = EventDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(name) || date == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (date < DateTime.Today)
            {
                MessageBox.Show("Дата не может быть в прошлом.");
                return;
            }

            using (var context = new BettingContext())
            {
                var eventToUpdate = context.Events.Find(_event.EventID);
                if (eventToUpdate != null)
                {
                    eventToUpdate.EventName = name;
                    eventToUpdate.EventDate = date.Value;
                    context.SaveChanges();
                    MessageBox.Show("Событие обновлено.");
                }
                else
                {
                    MessageBox.Show("Событие не найдено.");
                }
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
