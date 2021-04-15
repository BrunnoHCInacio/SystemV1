using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Domain.Services.Notifications
{
    public class Notifier : INotifier
    {
        public List<Notification> _notifications;

        public Notifier(List<Notification> notifications)
        {
            _notifications = notifications;
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}