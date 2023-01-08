using FluentValidation.Results;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Test.ServiceTests
{
    public abstract class ServiceTestBase
    {
        protected readonly AutoMocker _autoMocker;

        protected ServiceTestBase(AutoMocker autoMocker)
        {
            _autoMocker = autoMocker;
        }

        protected void SetSetupMock<TEntity, TValidation>(TEntity entity, List<ValidationFailure> errors = null)
            where TValidation : class, IValidation<TEntity>
            where TEntity : Entity
        {
            if (errors != null && errors.Any())
            {
                _autoMocker.GetMock<TValidation>().Setup(v => v.Validate(entity)).Returns(new ValidationResult(errors));
                return;
            }

            _autoMocker.GetMock<TValidation>().Setup(v => v.Validate(entity)).Returns(new ValidationResult());
        }

        protected static List<ValidationFailure> GenerateMockErrors(string propertyValidated, string message)
        {
            return new List<ValidationFailure>
            {
                new ValidationFailure(propertyValidated, message)
            };
        }
    }
}