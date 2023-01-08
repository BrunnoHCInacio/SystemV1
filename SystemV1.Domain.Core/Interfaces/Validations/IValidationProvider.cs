using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Validations
{
    public interface IValidationProvider : IValidation<Provider>
    {
    }
}