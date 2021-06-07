using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class CountryValidation : AbstractValidator<Country>
    {
        public static string CountryNameRequired = "O nome do país é obrigatório";

        public CountryValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(CountryNameRequired);
        }
    }
}