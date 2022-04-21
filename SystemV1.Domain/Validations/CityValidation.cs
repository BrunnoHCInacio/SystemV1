using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class CityValidation : AbstractValidator<City>
    {
        public static string NameRequired => "O nome da cidade deve ser informado.";
        public static string StateRequired => "O estado deve ser informado.";
        public CityValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameRequired);

            RuleFor(c => c.StateId)
                .Must(c => c != Guid.Empty)
                .WithMessage(StateRequired);

        }
    }
}
