using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    public class CityDomainTest
    {
        private readonly CityTestFixture _cityTestFixture;

        public CityDomainTest()
        {
            _cityTestFixture = new CityTestFixture();
        }

        [Fact(DisplayName ="Validate set as correct proprieties")]
        [Trait("Categoria","Cidade - Cadastro")]
        public void City_SetValueProperties_MustSetValuesWithSuccess()
        {
            //Arrange
            var cityExpected = _cityTestFixture.GenerateExpectedCity();

            //Act
            var city = new City(cityExpected.Id, cityExpected.Name, new State(cityExpected.State.Id,cityExpected.State.Name));

            //Assert
            Assert.Equal(city.Id, cityExpected.Id);
            Assert.Equal(city.Name, cityExpected.Name);
            Assert.Equal(city.State.Id, cityExpected.State.Id);
            Assert.Equal(city.State.Name, cityExpected.State.Name);
        }

        [Fact(DisplayName ="Validate valid city")]
        [Trait("Categoria", "Cidade - Cadastro")]
        public void City_ValidateCity_MustBeValid()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();

            //Act
            var result = city.ValidateCity();

            //Asert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate invalid city")]
        [Trait("Categoria", "Cidade - Cadastro")]
        public void City_ValidateCity_MustBeInvalidAndContainsValidationsMessage()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();

            //Act
            var result = city.ValidateCity();

            //Asert
            Assert.False(result.IsValid);
            Assert.Contains(CityValidation.NameRequired, result.Errors.Select(e=>e.ErrorMessage));
        }
    }
}
