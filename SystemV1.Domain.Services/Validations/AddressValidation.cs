using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.Street).NotEmpty().WithMessage("O logradoudo é obrigatório.");
            RuleFor(a => a.Street).Length(2, 100).WithMessage("O logradouro deve conter entre 2 e 100 caracteres.");
            RuleFor(a => a.District).NotEmpty().WithMessage("O bairro é obrigatório.");
            RuleFor(a => a.District).Length(2, 50).WithMessage("O bairro deve conter entre 2 e 50 caracteres.");
        }
    }
}