﻿using FluentValidation;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Validations
{
    public interface IValidationAddress : IValidation<Address>
    {
    }
}