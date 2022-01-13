using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ProductItemValidation : AbstractValidator<ProductItem>
    {
        public static string ProductItemRequired => "O item do produto é obrigatório.";
        public static string ProductItemMinLength => "O item do produto deve conter pelo menos 2 caracteres.";
        public static string ProductItemMaxLength => "O item do produto deve conter até 100 caracteres.";
        public static string ProductItemNotActive => "O item do produto deve estar ativo.";
        public ProductItemValidation()
        {
            RuleFor(c => c.Modelo)
                .NotEmpty()
                .WithMessage(ProductItemRequired);
            RuleFor(c => c.Modelo)
                .MinimumLength(2)
                .WithMessage(ProductItemMinLength);
            RuleFor(c => c.Modelo)
                .MaximumLength(100)
                .WithMessage(ProductItemMaxLength);
            RuleFor(a => a.IsActive)
                .Equal(true)
                .WithMessage(ProductItemNotActive);
        }
    }
}