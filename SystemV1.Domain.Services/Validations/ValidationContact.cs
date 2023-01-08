using FluentValidation;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;

namespace SystemV1.Domain.Services.Validations
{
    public class ValidationContact : AbstractValidator<Contact>, IValidationContact
    {
        public static string TypeContactRequired => "O tipo de contato deve ser informado.";
        public static string PhoneNumberRequired => "O telefone fixo é obrigatório.";
        public static string PhoneNumberMinLength => "O telefone fixo deve conter 8 caracteres.";
        public static string PhoneNumberMaxLength => "O telefone fixo deve conter 8 caracteres.";
        public static string DddRequired => "O DDD é obrigatório";
        public static string DddMinLength => "O DDD deve conter pelo menos 1 caracteres.";
        public static string DddMaxLength => "O DDD deve conter até 3 caracteres.";

        public static string CellPhoneRequired => "O celular é obrigatório.";
        public static string CellPhoneMinLength => "O celular deve conter pelo menos 8 caracteres.";
        public static string CellPhoneMaxLength => "O celular deve conter no máximo 9 caracteres.";

        public static string EmailRequired => "O email é obrigatório.";
        public static string EmailValid => "Informe um email válido.";
        public static string ContactNotActive => "O contato deve estar ativo.";

        public ValidationContact()
        {
        }

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
            RuleFor(c => c.TypeContact)
                                   .NotNull()
                                   .WithMessage(TypeContactRequired);

            When(c => c.TypeContact == EnumTypeContact.TypeContactPhone,
                () =>
                {
                    RuleFor(c => c.PhoneNumber)
                    .NotEmpty()
                    .WithMessage(PhoneNumberRequired);

                    RuleFor(c => c.PhoneNumber)
                    .MinimumLength(8)
                    .WithMessage(PhoneNumberMinLength);

                    RuleFor(c => c.PhoneNumber)
                    .MaximumLength(8)
                    .WithMessage(PhoneNumberMaxLength);

                    RuleFor(c => c.Ddd)
                    .NotEmpty()
                    .WithMessage(DddRequired);

                    RuleFor(c => c.Ddd)
                    .MinimumLength(1)
                    .WithMessage(DddMinLength);

                    RuleFor(c => c.Ddd)
                    .MaximumLength(3)
                    .WithMessage(DddMaxLength);
                });

            When(c => c.TypeContact == EnumTypeContact.TypeContactCellPhone,
                () =>
                {
                    RuleFor(c => c.Ddd)
                        .NotEmpty()
                        .WithMessage(DddRequired);

                    RuleFor(c => c.Ddd)
                        .MinimumLength(1)
                        .WithMessage(DddMinLength);

                    RuleFor(c => c.Ddd)
                        .MaximumLength(3)
                        .WithMessage(DddMaxLength);

                    RuleFor(c => c.CellPhoneNumber)
                        .NotEmpty()
                        .WithMessage(CellPhoneRequired);

                    RuleFor(c => c.CellPhoneNumber)
                        .MinimumLength(8)
                        .WithMessage(CellPhoneMinLength);

                    RuleFor(c => c.CellPhoneNumber)
                        .MaximumLength(9)
                        .WithMessage(CellPhoneMaxLength);
                });

            When(c => c.TypeContact == EnumTypeContact.TypeContactEmail,
                () =>
                {
                    RuleFor(c => c.Email)
                        .NotEmpty()
                        .WithMessage(EmailRequired);

                    RuleFor(c => c.Email)
                        .EmailAddress()
                        .WithMessage(EmailValid);
                });
        }
    }
}