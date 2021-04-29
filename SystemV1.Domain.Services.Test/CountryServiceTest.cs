using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Enums;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Services.Test
{
    public class CountryServiceTest
    {
        [Fact]
        public void TestValidations()
        {
            ValidateCountry(Case.Success);
            ValidateCountry(Case.Fail);
            ValidateItensValidation();
        }

        [Fact]
        public void Create()
        {
        }

        private void ValidateCountry(Case caseValidate)
        {
            var state1Expected = new
            {
                Name = caseValidate == Case.Success
                         ? "Goias"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                Id = Guid.NewGuid()
            };
            var state2Expected = new
            {
                Name = caseValidate == Case.Success
                         ? "Amazonas"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                Id = Guid.NewGuid()
            };

            var countryExpected = new
            {
                Id = Guid.NewGuid(),
                Name = caseValidate == Case.Success
                         ? "Brasil"
                         : caseValidate == Case.Fail
                             ? ""
                             : null,
                States = new List<State>
                {
                    new State(state1Expected.Name, state1Expected.Id),
                    new State(state2Expected.Name, state2Expected.Id)
                }
            };

            var country = new Country(countryExpected.Name, countryExpected.Id, countryExpected.States);

            GenericValidationTest.RunValidation(new CountryValidation(),
                                                country,
                                                caseValidate);
            foreach (var state in country.States)
            {
                GenericValidationTest.RunValidation(new StateValidation(),
                                                    state,
                                                    caseValidate);
            }
        }

        private void ValidateItensValidation()
        {
            var messagesState = new List<string> { ConstantsStateMessages.StateRequired };
            var messagesCountry = new List<string> { ConstantsCountryMessage.CountryRequired };
            ValidateItens("", "", "", messagesCountry, messagesState);

            ValidateItens("Goiás", "", "Brasil", null, messagesState);

            ValidateItens("", "Amazonas", "", messagesCountry);
        }

        private void ValidateItens(string stateName1,
                                   string stateName2,
                                   string countryName,
                                   List<string> messagesCountryExpected = null,
                                   List<string> messagesStateExpected = null)
        {
            var states = new List<State>
            {
                new State(stateName1, Guid.NewGuid()),
                new State(stateName2, Guid.NewGuid())
            };

            var country = new Country(countryName, Guid.NewGuid(), states);

            GenericValidationTest.RunValidation(new CountryValidation(), country, null, messagesCountryExpected);

            foreach (var state in states)
            {
                GenericValidationTest.RunValidation(new StateValidation(), state, null, messagesStateExpected);
            }
        }
    }
}