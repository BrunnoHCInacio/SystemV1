using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ClientValidation : AbstractValidator<Client>
    {
        public static string NameRequired => "O nome do cliente é obrigatório.";
        public static string NameMinLength => "O nome do cliente deve conter pelo menos 2 caracteres";
        public static string NameMaxLength => "O nome do cliente deve conter até 100 caracteres";
        public static string DocumentRequired => "O documento do cliente é obrigatório.";

        public ClientValidation()
        {
            RuleFor(c => c.Name)
               .NotEmpty()
               .WithMessage(NameRequired);

            RuleFor(c => c.Name)
                .MinimumLength(2)
                .WithMessage(NameMinLength);

            RuleFor(c => c.Name)
                .MaximumLength(100)
                .WithMessage(NameMaxLength);

            RuleFor(c => c.Document)
                .NotEmpty()
                .WithMessage(DocumentRequired);
        }
    }
}