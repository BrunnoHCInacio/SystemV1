using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test
{
    public class CountryDomainTest
    {
        [Fact]
        public void Create()
        {
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
                    new State(state1Expected.Name, state1Expected.Id),
                    new State(state2Expected.Name, state2Expected.Id)
                }
            };

            var country = new Country(countryExpected.Name,
                                      countryExpected.Id,
                                      countryExpected.States);

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