using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
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

        [Fact(DisplayName = "Validate set as correct proprieties")]
        [Trait("UnitTests - Entity", "City")]
        public void City_SetValueProperties_MustSetValuesWithSuccess()
        {
            //Arrange
            var cityExpected = _cityTestFixture.GenerateExpectedCity();

            //Act
            var city = new City(cityExpected.Id, cityExpected.Name, new State(cityExpected.State.Id, cityExpected.State.Name));

            //Assert
            Assert.Equal(city.Id, cityExpected.Id);
            Assert.Equal(city.Name, cityExpected.Name);
            Assert.Equal(city.State.Id, cityExpected.State.Id);
            Assert.Equal(city.State.Name, cityExpected.State.Name);
        }
    }
}