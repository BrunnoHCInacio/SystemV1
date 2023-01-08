using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationClient : AbstractValidator<Client>, IValidationClient
    {
        public string PeopleRequired => "os dados pessoais devem ser informados.";
        public string PeopleNotExist => "A pessoa informada é inexistente.";

        private readonly IRepositoryPeople _repositoryPeople;

        public ValidationClient(IRepositoryPeople repositoryPeople)
        {
            _repositoryPeople = repositoryPeople;
        }

        public void RulesForAdd()
        {
            RulesForAddAndUpdate();
        }

        public void RulesForDelete()
        {
        }

        public void RulesForUpdate()
        {
            RulesForAddAndUpdate();
        }

        private void RulesForAddAndUpdate()
        {
            RuleFor(c => c.PeopleId)
                .NotNull()
                .WithMessage(PeopleRequired);

            RuleFor(c => c.PeopleId)
                .MustAsync(async (peopleId, cancellation) =>
                {
                    return await _repositoryPeople.ExistsAsync(p => p.Id == peopleId);
                })
                .WithMessage(PeopleNotExist);
        }
    }
}