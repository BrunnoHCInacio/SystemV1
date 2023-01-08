using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationProvider : AbstractValidator<Provider>, IValidationProvider
    {
        public static string PeopleNotExist => "A pessoa informada é inexistente.";
        public static string ProviderNotExist => "O fornecedor informado é inexistente.";

        private readonly IRepositoryPeople _repositoryPeople;
        private readonly IRepositoryProvider _repositoryProvider;

        public ValidationProvider(IRepositoryPeople repositoryPeople,
                                  IRepositoryProvider repositoryProvider)
        {
            _repositoryPeople = repositoryPeople;

            _repositoryProvider = repositoryProvider;
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForUpdate()
        {
            RuleFor(p => p.Id)
                            .MustAsync(async (providerId, cancelattion) =>
                            {
                                return await _repositoryProvider.ExistsAsync(p => p.Id == providerId);
                            })
                            .WithMessage(ProviderNotExist);
            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(p => p.PeopleId)
                            .MustAsync(async (peopleId, cancelattion) =>
                            {
                                return await _repositoryPeople.ExistsAsync(p => p.Id == peopleId);
                            })
                            .WithMessage(PeopleNotExist);
        }
    }
}