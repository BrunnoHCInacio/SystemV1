using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class CityValidation : AbstractValidator<City>
    {
        public string NameRequired => "O nome da cidade deve ser informado.";
        public CityValidation()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage(NameRequired);
        }
    }
}
