using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.Domain.Services
{
    public class Service
    {
        private readonly INotifier _notifier;

        public Service(INotifier notifier)
        {
            _notifier = notifier;
        }

        public bool IsValidEntity(ValidationResult entity)
        {
            if (entity.IsValid) return true;

            Notify(entity);

            return false;
        }

        public bool RunValidation<TValidation, TEntity>(TValidation validation, TEntity entity) where TValidation : AbstractValidator<TEntity> where TEntity : Entity
        {
            var validator = validation.Validate(entity);
            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }

        public void Notify(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        public void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}