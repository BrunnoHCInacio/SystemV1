using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Services.Notifications
{
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}