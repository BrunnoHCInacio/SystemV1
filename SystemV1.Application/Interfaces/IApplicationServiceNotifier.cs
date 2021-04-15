using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceNotifier
    {
        void Handle(Notification notification);

        bool HasNotification();

        List<Notification> GetNotifications();
    }
}