using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationPeople : AbstractValidator<People>, IValidationPeople
    {
        public static string NameRequired => "O nome do cliente é obrigatório.";
        public static string NameMinLength => "O nome do cliente deve conter pelo menos 2 caracteres";
        public static string NameMaxLength => "O nome do cliente deve conter até 100 caracteres";
        public static string DocumentRequired => "O documento do cliente é obrigatório.";
        public static string ClientNotActive => "O cliente deve estar ativo.";
        public static string DocumentInvalid => "O documento inválido.";
        public static string PeopleNotRegistred => "A pessoa informada é inexistente.";

        private readonly IRepositoryPeople _repositoryPeople;
        private readonly IValidationAddress _validationAddress;
        private readonly IValidationContact _validationContact;

        public ValidationPeople(IRepositoryPeople repositoryPeople)
        {
            _repositoryPeople = repositoryPeople;
            _validationContact = new ValidationContact();
            _validationAddress = new ValidationAddress();

            RuleForEach(p => p.Addresses)
               .SetValidator(_validationAddress)
               .When(p => p.Addresses != null && p.Addresses.Any());

            RuleForEach(p => p.Contacts)
                .SetValidator(_validationContact)
                .When(p => p.Contacts != null && p.Contacts.Any());
        }

        public void RulesForAdd()
        {
            RuleForAddAndUpdate();
        }

        public void RulesForUpdate()
        {
            RuleFor(p => p.Id)
                .MustAsync(async (peopleId, cancellation) =>
                {
                    return await _repositoryPeople.ExistsAsync(p => p.Id == peopleId);
                })
                .WithMessage(PeopleNotRegistred);

            RuleForAddAndUpdate();
        }

        public void RulesForDelete()
        {
        }

        private void RuleForAddAndUpdate()
        {
            RuleFor(c => c.Name)
                       .NotEmpty()
                       .WithMessage(NameRequired);

            RuleFor(c => c.Name)
                .MinimumLength(2)
                .WithMessage(NameMinLength);

            RuleFor(c => c.Name)
                .MaximumLength(100)
                .WithMessage(NameMaxLength);

            RuleFor(c => c.Document)
                .NotEmpty()
                .WithMessage(DocumentRequired);

            RuleFor(p => p)
               .Must(p => ValideDocument(p))
               .WithMessage(DocumentInvalid);
        }

        private bool ValideDocument(People people)
        {
            if (people.TypePeople == EnumTypePeople.PHYSICAL)
            {
                return Regex.Replace(people.Document, @"[^0-9]", string.Empty).Length == 11;
            }

            if (people.TypePeople == EnumTypePeople.JURIDICAL)
            {
                return Regex.Replace(people.Document, @"[^0-9]", string.Empty).Length == 14;
            }

            return false;
        }
    }
}