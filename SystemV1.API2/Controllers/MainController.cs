using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        protected MainController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected ActionResult OkResult(ModelStateDictionary modelState)
        {
            NotifyErrorModelInvalid(modelState);
            return OkResult();
        }

        [NonAction]
        protected ActionResult OkResult(object result = null)
        {
            if (HasNotification())
            {
                return BadRequest(new
                {
                    success = false,
                    errors = _notifier.GetNotifications().Select(n => n.Message)
                });
            }
            if (result != null)
            {
                return Ok(new
                {
                    Success = true,
                    Data = result
                });
            }
            return Ok();
        }

        [NonAction]
        protected void NotifyErrorModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                Notify(error.Exception == null ? error.ErrorMessage : error.Exception.Message);
            }
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