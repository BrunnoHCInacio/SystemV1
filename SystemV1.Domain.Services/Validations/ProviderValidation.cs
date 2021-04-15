using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ProviderValidation : AbstractValidator<Provider>
    {
        public ProviderValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.");

            RuleFor(c => c.Name)
                .MinimumLength(2)
                .WithMessage("O nome deve conter pelo menos 2 caracteres.");
            RuleFor(c => c.Name)
                .MaximumLength(100)
                .WithMessage("O nome deve conter até 100 caracteres.");
        }
    }
}