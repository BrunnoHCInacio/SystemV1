using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public static string NameRequired => "O nome é obrigatório.";
        public static string ProductNotActive => "O produto deve estar ativo.";
        public static string ProductItemNotEmpyt => "O produto deve conter items.";
        public ProductValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameRequired);
            RuleFor(a => a.IsActive)
                .Equal(true)
                .WithMessage(ProductNotActive);

            RuleFor(p => p.ProductItems)
                .Must(pi =>pi.Any())
                .WithMessage(ProductItemNotEmpyt);
        }
    }
}