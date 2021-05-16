using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test
{
    public class AddressDomainTest
    {
        [Fact]
        public void Create()
        {
            var countryExpected = new
            {
                Id = Guid.NewGuid(),
                Name = "Brasil"
            };

            var stateExpected = new
            {
                Id = Guid.NewGuid(),
                Name = "Goiás",
                Country = countryExpected
            };

            var addressExpected = new
            {
                Id = Guid.NewGuid(),
                ZipCode = (int)75650000,
                Street = "Rua 2",
                Number = "s/n",
                Complement = "Qd 01 Lt 01",
                District = "Centro",
                State = stateExpected,
            };

            var country = new Country(stateExpected.Country.Name,
                                      stateExpected.Country.Id);

            var state = new State(stateExpected.Name,
                                  stateExpected.Id);
            state.Country = country;
            var address = new Address(addressExpected.ZipCode,
                                      addressExpected.Street,
                                      addressExpected.Number,
                                      addressExpected.Complement,
                                      addressExpected.District,
                                      addressExpected.Id);

            address.State = state;

            Assert.Equal(address.Id, addressExpected.Id);
            Assert.Equal(address.Street, addressExpected.Street);
            Assert.Equal(address.ZipCode, addressExpected.ZipCode);
            Assert.Equal(address.Number, addressExpected.Number);
            Assert.Equal(address.Complement, addressExpected.Complement);
            Assert.Equal(address.District, addressExpected.District);

            Assert.Equal(address.State.Id, stateExpected.Id);
            Assert.Equal(address.State.Name, stateExpected.Name);

            Assert.Equal(address.State.Country.Id, countryExpected.Id);
            Assert.Equal(address.State.Country.Name, countryExpected.Name);
        }
    }
}