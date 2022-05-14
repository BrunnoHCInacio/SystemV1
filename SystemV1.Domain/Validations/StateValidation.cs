using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class StateValidation : AbstractValidator<State>
    {
        public static string StateNameRequired => "O nome do estado é obrigatório.";
        public static string CountryRequired => "O país é obrigatório.";
        public static string StateNotActive => "O estado deve estar ativo.";

        public StateValidation()
        {
            RuleFor(s => s.Name)
                .NotEmpty()
                .WithMessage(StateNameRequired);

            RuleFor(s => s.CountryId)
                .NotEmpty()
                .NotNull()
                .WithMessage(CountryRequired);

            RuleFor(s => s.IsActive)
                .Equal(true)
                .WithMessage(StateNotActive);
        }
    }
}