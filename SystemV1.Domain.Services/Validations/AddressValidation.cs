﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
        }
    }
}