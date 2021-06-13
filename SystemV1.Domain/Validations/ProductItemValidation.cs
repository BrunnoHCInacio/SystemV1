using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ProductItemValidation : AbstractValidator<ProductItem>
    {
        public static string ModelRequired => "O modelo é obrigatório.";
        public static string ModelMinLength => "O modelo deve conter pelo menos 2 caracteres.";
        public static string ModelMaxLength => "O modelo deve conter até 100 caracteres.";

        public ProductItemValidation()
        {
            RuleFor(c => c.Modelo)
                .NotEmpty()
                .WithMessage(ModelRequired);
            RuleFor(c => c.Modelo)
                .MinimumLength(2)
                .WithMessage(ModelMinLength);
            RuleFor(c => c.Modelo)
                .MaximumLength(100)
                .WithMessage(ModelMaxLength);
        }
    }
}