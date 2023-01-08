using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationCountry : AbstractValidator<Country>, IValidationCountry
    {
        public static string CountryNameRequired = "O nome do país é obrigatório.";
        public static string NameMinLength = "O nome do país deve conter entre 2 e 100 caracteres.";
        public static string CountryNotRegistred = "O país informado é inexistente.";
        public static string CountryHaveLinksNotRemove = "O país não pode ser excluído, pois possui referencias.";

        private readonly IRepositoryCountry _repositoryCountry;
        private readonly IRepositoryState _repositoryState;

        public ValidationCountry(IRepositoryCountry repositoryCountry,
                                 IRepositoryState repositoryState)
        {
            _repositoryCountry = repositoryCountry;
            _repositoryState = repositoryState;
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
            RuleFor(c => c.Id)
                .MustAsync(async (countryId, cancellation) =>
                {
                    return !await _repositoryState.ExistsAsync(s => s.CountryId == countryId);
                })
                .WithMessage(CountryHaveLinksNotRemove);
        }

        public void RulesForUpdate()
        {
            RuleFor(c => c.Id)
                .MustAsync(async (countryId, cancellation) =>
                {
                    var result = await _repositoryCountry.ExistsAsync(c => c.Id == countryId);
                    return result;
                })
                .WithMessage(CountryNotRegistred);

            RuleForAddAndUpdate();
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(c => c.Name)
                           .NotEmpty()
                           .WithMessage(CountryNameRequired);

            RuleFor(c => c.Name).MinimumLength(2).WithMessage(NameMinLength);
        }
    }
}