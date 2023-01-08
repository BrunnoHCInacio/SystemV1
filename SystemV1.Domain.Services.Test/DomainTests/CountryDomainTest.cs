using System.Collections.Generic;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
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
        [Trait("UnitTests - Entity", "Country")]
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
            var country = new Country(countryExpected.Id, countryExpected.Name, null);
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

        #endregion Test validate for each property from entity
    }
}