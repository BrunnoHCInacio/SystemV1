using FluentValidation;
using System;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationCity : AbstractValidator<City>, IValidationCity
    {
        public static string NameRequired => "O nome da cidade deve ser informado.";
        public static string StateRequired => "O estado deve ser informado.";
        public static string StateUnvailable => "O estado informado é inexistente.";
        public static string CityNotRegistred => "A cidade informada é inexistente.";
        public static string CityNotDeleteHaveLinks => "A cidade não pode ser excluída, pois possui referências.";

        private readonly IRepositoryState _repositoryState;
        private readonly IRepositoryAddress _repositoryAddress;

        public ValidationCity(IRepositoryState repositoryState,
                              IRepositoryAddress repositoryAddress)
        {
            _repositoryState = repositoryState;
            _repositoryAddress = repositoryAddress;
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForUpdate()
        {
            RuleFor(c => c.Id)
                .MustAsync(async (cityId, cancellation) =>
                {
                    var result = await _repositoryState.ExistsAsync(s => s.Id == cityId);
                    return result;
                })
                .WithMessage(CityNotRegistred);

            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
            RuleFor(c => c.Id)
                .MustAsync(async (cityId, cancellation) =>
                {
                    var result = !await _repositoryAddress.ExistsAsync(a => a.CityId == cityId);
                    return result;
                })
                .WithMessage(StateUnvailable);
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(c => c.Name)
                          .NotEmpty()
                          .WithMessage(NameRequired);

            RuleFor(c => c.StateId)
                .Must(c => c != Guid.Empty)
                .WithMessage(StateRequired);

            RuleFor(c => c.StateId)
                .MustAsync(async (stateId, cancellation) =>
                {
                    var result = await _repositoryState.ExistsAsync(s => s.Id == stateId);
                    return result;
                })
                .WithMessage(StateUnvailable);
        }
    }
}