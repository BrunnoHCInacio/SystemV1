using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public static string StreetRequired => "O logradoudo é obrigatório.";

        public static string StreetLength2_100 => "O logradouro deve conter entre 2 e 100 caracteres.";

        public static string DistrictRequired => "O bairro é obrigatório.";

        public static string DistrictLenght2_50 => "O bairro deve conter entre 2 e 50 caracteres.";

        public static string AddressNotActive => "O endereço deve estar ativo.";
        public AddressValidation()
        {
            RuleFor(a => a.Street)
               .NotEmpty()
               .WithMessage(StreetRequired);
            RuleFor(a => a.Street)
                .Length(2, 100)
                .WithMessage(StreetLength2_100);
            RuleFor(a => a.District)
                .NotEmpty()
                .WithMessage(DistrictRequired);

            RuleFor(a => a.District)
                .Length(2, 50)
                .WithMessage(DistrictLenght2_50);

            RuleFor(a => a.IsActive)
                .Equal(true)
                .WithMessage(AddressNotActive);
        }
    }
}