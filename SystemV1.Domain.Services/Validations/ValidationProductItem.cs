using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationProductItem : AbstractValidator<ProductItem>, IValidationProductItem
    {
        public static string ProductItemRequired => "O item do produto é obrigatório.";
        public static string ProductItemMinLength => "O item do produto deve conter pelo menos 2 caracteres.";
        public static string ProductItemMaxLength => "O item do produto deve conter até 100 caracteres.";
        public static string ProductItemNotActive => "O item do produto deve estar ativo.";

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForUpdate()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
        }

        private void RuleForAddAndUpdate()
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
        }
    }
}