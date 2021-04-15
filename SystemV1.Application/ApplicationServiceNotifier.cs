using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.Interfaces;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.Application
{
    public class ApplicationServiceNotifier : IApplicationServiceNotifier
    {
        private readonly INotifier _notifier;

        public ApplicationServiceNotifier(INotifier notifier)
        {
            _notifier = notifier;
        }

        public List<Notification> GetNotifications()
        {
            return _notifier.GetNotifications();
        }

        public void Handle(Notification notification)
        {
            _notifier.Handle(notification);
        }

        public bool HasNotification()
        {
            return _notifier.HasNotification();
        }
    }
}