using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface INotifier : IDisposable
    {
        void Handle(Notification notification);

        List<Notification> GetNotifications();

        bool HasNotification();
    }
}