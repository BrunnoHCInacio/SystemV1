using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public static string NameRequired => "O nome é obrigatório.";
        public static string ProductNotActive => "O produto deve estar ativo.";
        public ProductValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameRequired);
            RuleFor(a => a.IsActive)
                .Equal(true)
                .WithMessage(ProductNotActive);
        }
    }
}