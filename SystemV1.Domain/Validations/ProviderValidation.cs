using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ProviderValidation : AbstractValidator<Provider>
    {
        private static string NameRequired => "O nome é obrigatório.";
        private static string NameMinLength => "O nome deve conter pelo menos 2 caracteres.";
        private static string NameMaxLength => "O nome deve conter até 100 caracteres.";
        public static string ProviderNotActive => "O fornecedor deve estar ativo.";

        public ProviderValidation()
        {
            RuleFor(p => p.Name)
               .NotEmpty()
               .WithMessage(NameRequired);

            RuleFor(p => p.Name)
                .MinimumLength(2)
                .WithMessage(NameMinLength);
            
            RuleFor(p => p.Name)
                .MaximumLength(100)
                .WithMessage(NameMaxLength);

            RuleFor(p => p.IsActive)
                .Equal(true)
                .WithMessage(ProviderNotActive);
        }
    }
}