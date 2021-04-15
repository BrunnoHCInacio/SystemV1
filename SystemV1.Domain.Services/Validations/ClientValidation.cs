using FluentValidation;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class ClientValidation : AbstractValidator<Client>
    {
        public ClientValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome do cliente é obrigatório.");
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome do cliente deve conter entre 2 e 100 caracteres");
            RuleFor(c => c.Document)
                .NotEmpty()
                .WithMessage("O documento do cliente é obrigatório.");
        }
    }
}