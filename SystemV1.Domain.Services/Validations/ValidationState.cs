using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationState : AbstractValidator<State>, IValidationState
    {
        public static string StateNameRequired => "O nome do estado é obrigatório.";
        public static string CountryUnavailable => "O país informado é inexistente.";
        public static string StateNotRegister => "O estado informado é inexistente.";
        public static string StateNotDeleteHaveLinks => "O estado não pode ser excluido, pois possui referências.";

        private readonly IRepositoryCountry _repositoryCountry;
        private readonly IRepositoryState _repositoryState;
        private readonly IRepositoryCity _repositoryCity;

        public ValidationState(IRepositoryCountry repositoryCountry,
                               IRepositoryState repositoryState,
                               IRepositoryCity repositoryCity)
        {
            _repositoryCountry = repositoryCountry;
            _repositoryState = repositoryState;
            _repositoryCity = repositoryCity;
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForUpdate()
        {
            RuleFor(s => s.Id)
               .MustAsync(async (stateId, cancellation) =>
               {
                   var result = await _repositoryState.ExistsAsync(c => c.Id == stateId);
                   return result;
               })
               .WithMessage(StateNotRegister);

            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
            RuleFor(s => s.Id)
                .MustAsync(async (stateId, cancellation) =>
                {
                    var result = await _repositoryCity.ExistsAsync(c => c.StateId == stateId) == false;
                    return result;
                })
                .WithMessage(StateNotDeleteHaveLinks);
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(s => s.Name)
                           .NotEmpty()
                           .WithMessage(StateNameRequired);

            RuleFor(s => s.CountryId)
                .MustAsync(async (countryId, cancellation) =>
                {
                    var result = await _repositoryCountry.ExistsAsync(c => c.Id == countryId);
                    return result;
                })
                .WithMessage(CountryUnavailable);
        }
    }
}