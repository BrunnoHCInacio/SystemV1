using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test
{
    [Collection(nameof(CountryCollection))]
    public class CountryDomainTest
    {
        private readonly CountryTestFixture _countryTestFixture;

        public CountryDomainTest(CountryTestFixture countryTestFixture)
        {
            _countryTestFixture = countryTestFixture;
        }

        #region Test validate for each property from entity
        [Fact(DisplayName = "Validate set as correct properties")]
        [Trait("Categoria", "País - Cadastro")]
        public void Country_NewCountryWithState_ShouldSetCorrectProperties()
        {
            //Arrange

            var state1Expected = _countryTestFixture.GenerateStateValidExpected();
            var state2Expected = _countryTestFixture.GenerateStateValidExpected();

            var countryExpected = _countryTestFixture.GenerateCountryExpected();

            var states = new List<State>
            {
                new State(state1Expected.Id, state1Expected.Name),
                new State(state2Expected.Id, state2Expected.Name)
            };

            //Act
            var country = new Country(countryExpected.Id, countryExpected.Name);
            country.AddStates(states);

            //Assert
            Assert.Equal(country.Id, countryExpected.Id);
            Assert.Equal(country.Name, countryExpected.Name);

            foreach (var state in country.States)
            {
                if (state1Expected.Id == state.Id)
                {
                    Assert.Equal(state.Name, state1Expected.Name);
                    Assert.Equal(state.Id, state1Expected.Id);
                }
                if (state2Expected.Id == state.Id)
                {
                    Assert.Equal(state.Name, state2Expected.Name);
                    Assert.Equal(state.Id, state2Expected.Id);
                }
            }
        }
        #endregion

        // TODO: Criar teste para testar as propriedades genericas: data cadastro, data alteração, usuário cadastro e usuário alteração

        #region Test validation for data
        [Fact(DisplayName = "Validate country valid")]
        [Trait("Categoria", "País - Cadastro")]
        public void Country_ValidateNewCountry_ShouldBeValid()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();

            //Act
            var result = country.ValidadeCountry();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate country invalid")]
        [Trait("Categoria", "País - Cadastro")]
        public void Country_ValidateNewCountry_ShouldBeInvalid()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();

            //Act
            var result = country.ValidadeCountry();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            
            Assert.Contains(CountryValidation.CountryNameRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CountryValidation.NameMinLength, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate country invalid")]
        [Trait("Categoria", "País - Cadastro")]
        public void Country_ValidateNewCountryDisabled_ShouldFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryDisabled();

            //Act
            var result = country.ValidadeCountry();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Single(result.Errors);
            
            Assert.Contains(CountryValidation.CountryNotActive, result.Errors.Select(e => e.ErrorMessage));
            
        }

        #endregion
    }
}