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
        public static string CountryNotActive = "O cadastro do país deve estar habilitado";
        public static string NameMinLength = "O nome do país deve conter entre 2 e 100 caracteres";

        public CountryValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(CountryNameRequired);

            RuleFor(c => c.Name).MinimumLength(2).WithMessage(NameMinLength);

            RuleFor(c => c.IsActive)
                .NotEqual(false)
                .WithMessage(CountryNotActive);
        }
    }
}