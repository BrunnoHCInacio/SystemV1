using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ProductItemValidation : AbstractValidator<ProductItem>
    {
        public ProductItemValidation()
        {
            RuleFor(c => c.Modelo)
                .NotEmpty()
                .WithMessage("O modelo é obrigatório.");
            RuleFor(c => c.Modelo)
                .MinimumLength(2)
                .WithMessage("O modelo deve conter pelo menos 2 caracteres.");
            RuleFor(c => c.Modelo)
                .MaximumLength(100)
                .WithMessage("O modelo deve conter até 100 caracteres.");
        }
    }
}