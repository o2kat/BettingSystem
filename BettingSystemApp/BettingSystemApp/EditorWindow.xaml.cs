using System;
using System.Linq;
using System.Timers;
using System.Windows;
using BettingSystemApp.Models;

namespace BettingSystemApp
{
    public partial class EditorWindow : Window
    {
        private Timer _clearOldEventsTimer;

        public EditorWindow()
        {
            InitializeComponent();
            LoadEvents();
            SetupClearOldEventsTimer();
        }

        private void LoadEvents()
        {
            using (var context = new BettingContext())
            {
                EventsListView.ItemsSource = context.Events.ToList();
            }
        }

        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            var addEventWindow = new AddEventWindow();
            addEventWindow.ShowDialog();
            LoadEvents();
        }

        private void EditEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventsListView.SelectedItem is Event selectedEvent)
            {
                var editWindow = new EditEventWindow(selectedEvent);
                editWindow.ShowDialog();
                LoadEvents();
            }
            else
            {
                MessageBox.Show("Выберите событие для редактирования.");
            }
        }

        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventsListView.SelectedItem is Event selectedEvent)
            {
                using (var context = new BettingContext())
                {
                    context.Events.Attach(selectedEvent);
                    context.Events.Remove(selectedEvent);
                    context.SaveChanges();
                }
                LoadEvents();
            }
            else
            {
                MessageBox.Show("Выберите событие для удаления.");
            }
        }

        private void ClearPastEvents_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BettingContext())
            {
                var oldEvents = context.Events.Where(ev => ev.EventDate < DateTime.Today).ToList();
                context.Events.RemoveRange(oldEvents);
                context.SaveChanges();
            }

            MessageBox.Show("Прошедшие события удалены.");
            LoadEvents();
        }

        private void SetupClearOldEventsTimer()
        {
            _clearOldEventsTimer = new Timer(86400000); // 24 часа
            _clearOldEventsTimer.Elapsed += (s, e) => ClearPastEventsAuto();
            _clearOldEventsTimer.Start();
        }

        private void ClearPastEventsAuto()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                using (var context = new BettingContext())
                {
                    var oldEvents = context.Events.Where(e => e.EventDate < DateTime.Today).ToList();
                    context.Events.RemoveRange(oldEvents);
                    context.SaveChanges();
                }
                LoadEvents();
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
