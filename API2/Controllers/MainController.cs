﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.API2.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        public MainController(INotifier notifier)
        {
            _notifier = notifier;
        }

        [NonAction]
        protected ActionResult OkResult(object result = null)
        {
            if (HasNotification())
            {
                return BadRequest(_notifier.GetNotifications().SelectMany(n => n.Message));
            }
            if (result != null)
            {
                return Ok(new
                {
                    result
                });
            }
            return Ok();
        }

        [NonAction]
        private bool HasNotification()
        {
            if (_notifier.HasNotification())
            {
                return true;
            }
            return false;
        }

        [NonAction]
        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}