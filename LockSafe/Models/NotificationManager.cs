using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LockSafe.Models
{
    public class NotificationManager
    {
        private DispatcherTimer? _notificationTimer;
        private bool _isNotificationOpen;

        public Popup NotificationPopup { get; set; }
        public TextBlock NotificationText { get; set; }
        public Grid MainGrid { get; set; }


        public NotificationManager(ref Popup popup, ref TextBlock text, ref Grid window) 
        {
            NotificationPopup = popup;
            NotificationText = text;
            MainGrid = window;            

            _notificationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _notificationTimer.Tick += OnNotificationTimerTick;
        }

        public void ShowNotification(string message)
        {
            NotificationText.Text = message;

            // If a notification is already open, do not open a new one
            if (_isNotificationOpen)
            {
                return;
            }            

            NotificationPopup.IsOpen = true;
            _isNotificationOpen = true;

            // Center the popup horizontally
            NotificationPopup.HorizontalOffset = (MainGrid.ActualWidth * 0.6D / -2D) + NotificationText.ActualWidth / 2D; // 0.6 because right column is 60% width

            _notificationTimer!.Start();
        }

        private void OnNotificationTimerTick(object? sender, EventArgs e)
        {
            NotificationPopup.IsOpen = false;
            _isNotificationOpen = false;
            _notificationTimer!.Stop();
        }
    }

}
