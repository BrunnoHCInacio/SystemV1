using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Enums;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Services.Test
{
    public class AddressServiceTest
    {
        [Fact]
        public void TestValidations()
        {
            ValidateTest(Case.Success);
            ValidateTest(Case.Fail);
            ValidateItensValidation();
        }

        [Fact]
        public void Create()
        {
        }

        private void ValidateTest(Case caseValidate)
        {
            var countryExpected = new
            {
                Id = Guid.NewGuid(),
                Name = caseValidate == Case.Success
                         ? "Brasil"
                         : caseValidate == Case.Fail
                             ? ""
                             : null
            };

            var stateExpected = new
            {
                Id = Guid.NewGuid(),
                Name = caseValidate == Case.Success
                         ? "Goiás"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                Country = countryExpected
            };

            var addressExpected = new
            {
                Id = Guid.NewGuid(),
                ZipCode = (int)75650000,
                Street = caseValidate == Case.Success
                         ? "Rua 2"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                Number = caseValidate == Case.Success
                         ? "s/n"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                Complement = caseValidate == Case.Success
                         ? "Qd 01 Lt 01"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                District = caseValidate == Case.Success
                         ? "Centro"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                State = stateExpected,
            };

            var country = new Country(stateExpected.Country.Name,
                                      stateExpected.Country.Id);

            var state = new State(stateExpected.Name,
                                  stateExpected.Id,
                                  country);

            var address = new Address(addressExpected.ZipCode,
                                      addressExpected.Street,
                                      addressExpected.Number,
                                      addressExpected.Complement,
                                      addressExpected.District,
                                      state,
                                      addressExpected.Id);

            GenericValidationTest.RunValidation(new CountryValidation(), country, caseValidate);
            GenericValidationTest.RunValidation(new StateValidation(), state, caseValidate);
            GenericValidationTest.RunValidation(new AddressValidation(), address, caseValidate);
        }

        private void ValidateItensValidation()
        {
            ValidateAttributes("",
                                "centro",
                                new List<string> { ConstantsAddressMessages.StreetRequired,
                                                   ConstantsAddressMessages.StreetLength2_100 });
            ValidateAttributes("rua 1",
                               "",
                                new List<string> { ConstantsAddressMessages.DistrictRequired,
                                                   ConstantsAddressMessages.DistrictLenght2_50 });
        }

        private void ValidateAttributes(string street,
                                       string district,
                                       List<string> messagesExpected)
        {
            var testStreet = new Address(0,
                                      street,
                                      "",
                                      "",
                                      district,
                                      null,
                                      Guid.NewGuid());

            GenericValidationTest.RunValidation(new AddressValidation(), testStreet, null, messagesExpected);
        }
    }
}