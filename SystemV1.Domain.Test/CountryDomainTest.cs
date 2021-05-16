using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test
{
    public class CountryDomainTest
    {
        [Fact]
        public void Country_NewCountry_ReturnFillCountry()
        {
            //Arrange
            var state1Expected = new
            {
                Name = "Goias",
                Id = Guid.NewGuid()
            };
            var state2Expected = new
            {
                Name = "Amazonas",
                Id = Guid.NewGuid()
            };

            var countryExpected = new
            {
                Id = Guid.NewGuid(),
                Name = "Brasil",
                States = new List<State>
                {
                    new State(state1Expected.Id,state1Expected.Name),
                    new State(state2Expected.Id, state2Expected.Name)
                }
            };

            //Act
            var country = new Country(countryExpected.Name,
                                      countryExpected.Id);
            country.States = countryExpected.States;

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
    }
}