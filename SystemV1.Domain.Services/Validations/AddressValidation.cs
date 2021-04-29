using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.Street)
                .NotEmpty()
                .WithMessage(ConstantsAddressMessages.StreetRequired);
            RuleFor(a => a.Street)
                .Length(2, 100)
                .WithMessage(ConstantsAddressMessages.StreetLength2_100);
            RuleFor(a => a.District)
                .NotEmpty()
                .WithMessage(ConstantsAddressMessages.DistrictRequired);

            RuleFor(a => a.District)
                .Length(2, 50)
                .WithMessage(ConstantsAddressMessages.DistrictLenght2_50);
        }
    }
}