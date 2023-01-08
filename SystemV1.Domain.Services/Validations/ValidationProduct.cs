using FluentValidation;
using System.Linq;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationProduct : AbstractValidator<Product>, IValidationProduct
    {
        public static string NameRequired => "O nome é obrigatório.";
        public static string ProductNotActive => "O produto deve estar ativo.";
        public static string ProductItemNotEmpyt => "O produto deve conter items.";
        public static string ProviderNotExist => "O fornecedor informado é inexistente.";
        public static string ProductNotRegistred => "O produto informado é inexistente.";

        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IRepositoryProduct _repositoryProduct;

        private readonly ValidationProductItem _validationProductItems;

        public ValidationProduct(IRepositoryProvider repositoryProvider,
                                 IRepositoryProduct repositoryProduct)
        {
            _repositoryProvider = repositoryProvider;
            _repositoryProduct = repositoryProduct;

            _validationProductItems = new ValidationProductItem();
            
            RuleForEach(p => p.ProductItems)
                .SetValidator(_validationProductItems);
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
            _validationProductItems.RulesForAdd();
        }

        public void RulesForUpdate()
        {
            RuleFor(p => p.Id)
                .MustAsync(async (producId, cancellation) =>
                {
                    return await _repositoryProduct.ExistsAsync(p => p.Id == producId);
                })
                .WithMessage(ProductNotRegistred);

            RuleForAddAndUpdate();
            _validationProductItems.RulesForUpdate();
        }

        public void RulesForDelete()
        {
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(c => c.Name)
               .NotEmpty()
               .WithMessage(NameRequired);

            RuleFor(p => p.ProductItems)
                .Must(pi => pi.Any())
                .WithMessage(ProductItemNotEmpyt);

            RuleFor(p => p.ProviderId)
                .MustAsync(async (providerId, cancellation) =>
                {
                    return await _repositoryProvider.ExistsAsync(p => p.Id == providerId);
                })
                .WithMessage(ProviderNotExist);

            
        }
    }
}