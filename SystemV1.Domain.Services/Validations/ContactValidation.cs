using FluentValidation;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ContactValidation : AbstractValidator<Contact>
    {
        public ContactValidation()
        {
            RuleFor(c => c.TypeContact)
                .NotEmpty()
                .WithMessage(ConstantsContactMessages.TypeContactRequired);

            When(c => c.TypeContact == Constants.TypeContactPhone,
                () =>
                {
                    RuleFor(c => c.PhoneNumber)
                        .NotEmpty()
                        .WithMessage(ConstantsContactMessages.PhoneNumberRequired);

                    RuleFor(c => c.PhoneNumber)
                        .MinimumLength(8)
                        .WithMessage(ConstantsContactMessages.PhoneNumberMinLength);

                    RuleFor(c => c.PhoneNumber)
                        .MaximumLength(8)
                        .WithMessage(ConstantsContactMessages.PhoneNumberMaxLength);

                    RuleFor(c => c.Ddd)
                        .NotEmpty()
                        .WithMessage(ConstantsContactMessages.DddRequired);

                    RuleFor(c => c.Ddd)
                        .MinimumLength(2)
                        .WithMessage(ConstantsContactMessages.DddMinLength);

                    RuleFor(c => c.Ddd)
                        .MaximumLength(3)
                        .WithMessage(ConstantsContactMessages.DddMaxLength);
                });

            When(c => c.TypeContact == Constants.TypeContactCellPhone,
                () =>
                {
                    RuleFor(c => c.Ddd)
                        .NotEmpty()
                        .WithMessage(ConstantsContactMessages.DddRequired);

                    RuleFor(c => c.Ddd)
                        .MinimumLength(2)
                        .WithMessage(ConstantsContactMessages.DddMinLength);

                    RuleFor(c => c.Ddd)
                        .MaximumLength(3)
                        .WithMessage(ConstantsContactMessages.DddMaxLength);

                    RuleFor(c => c.CellPhoneNumber)
                        .NotEmpty()
                        .WithMessage(ConstantsContactMessages.CellPhoneRequired);

                    RuleFor(c => c.CellPhoneNumber)
                        .MinimumLength(8)
                        .WithMessage(ConstantsContactMessages.CellPhoneMinLength);

                    RuleFor(c => c.CellPhoneNumber)
                        .MaximumLength(9)
                        .WithMessage(ConstantsContactMessages.CellPhoneMaxLength);
                });

            When(c => c.TypeContact == Constants.TypeContactEmail,
                () =>
                {
                    RuleFor(c => c.Email)
                        .NotEmpty()
                        .WithMessage(ConstantsContactMessages.EmailRequired);

                    RuleFor(c => c.Email)
                        .EmailAddress()
                        .WithMessage(ConstantsContactMessages.EmailValid);
                });
        }
    }
}