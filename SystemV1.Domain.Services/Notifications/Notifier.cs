using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Domain.Services.Notifications
{
    public class Notifier : INotifier
    {
        public List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public void Dispose()
        {
            _notifications = new List<Notification>();
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