using FluentValidation;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    internal class ContactValidation : AbstractValidator<Contact>
    {
        public ContactValidation()
        {
            RuleFor(c => c.TypeContact)
                .NotEmpty()
                .WithMessage("O tipo de contato deve ser informado.");

            When(c => c.TypeContact == Constants.TypeContactPhone,
                () =>
                {
                    RuleFor(c => c.PhoneNumber)
                        .NotEmpty()
                        .WithMessage("O telefone fixo é obrigatório.");

                    RuleFor(c => c.PhoneNumber)
                        .MinimumLength(8)
                        .WithMessage("O telefone fixo deve conter 8 caracteres.");

                    RuleFor(c => c.PhoneNumber)
                        .MaximumLength(8)
                        .WithMessage("O telefone fixo deve conter 8 caracteres.");

                    RuleFor(c => c.Ddd)
                        .NotEmpty()
                        .WithMessage("O DDD é obrigatório");

                    RuleFor(c => c.Ddd)
                        .MinimumLength(2)
                        .WithMessage("O DDD deve conter 2 caracteres.");

                    RuleFor(c => c.Ddd)
                        .MaximumLength(2)
                        .WithMessage("O DDD deve conter 2 caracteres.");
                });

            When(c => c.TypeContact == Constants.TypeContactCellPhone,
                () =>
                {
                    RuleFor(c => c.Ddd)
                        .NotEmpty()
                        .WithMessage("O DDD é obrigatório.");

                    RuleFor(c => c.Ddd)
                        .MinimumLength(2)
                        .WithMessage("O DDD deve conter 2 caracteres.");

                    RuleFor(c => c.Ddd)
                        .MaximumLength(2)
                        .WithMessage("O DDD deve conter 2 caracteres.");

                    RuleFor(c => c.CellPhoneNumber)
                        .NotEmpty()
                        .WithMessage("O celular é obrigatório.");

                    RuleFor(c => c.CellPhoneNumber)
                        .MinimumLength(8)
                        .WithMessage("O celular deve conter 8 caracteres.");

                    RuleFor(c => c.CellPhoneNumber)
                        .MaximumLength(8)
                        .WithMessage("O celular deve conter 8 caracteres.");
                });

            When(c => c.TypeContact == Constants.TypeContactEmail,
                () =>
                {
                    RuleFor(c => c.Email)
                        .NotEmpty()
                        .WithMessage("O email é obrigatório.");

                    RuleFor(c => c.Email)
                        .EmailAddress()
                        .WithMessage("Informe um email válido.");
                });
        }
    }
}