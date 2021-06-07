using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(AddressCollection))]
    public class AddressCollection : ICollectionFixture<AddressTestFixture> { }

    public class AddressTestFixture : IDisposable
    {
        public void Dispose()
        {
        }

        public dynamic GenerateAddressExpected()
        {
            var stateFixture = new StateTestFixture();
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                ZipCode = faker.Address.ZipCode(),
                Street = faker.Address.StreetAddress(),
                Number = faker.Random.Number(1, 9999).ToString(),
                Complement = faker.Address.SecondaryAddress(),
                District = faker.Address.Direction(),
                State = stateFixture.GenerateStateWithCountryValidExpected(),
                City = faker.Address.City()
            };
        }

        public dynamic GenerateStateWithCountryValidExpected()
        {
            var stateFixture = new StateTestFixture();
            return stateFixture.GenerateStateWithCountryValidExpected();
        }

        public dynamic GenerateCountryExpected()
        {
            var countryFixture = new CountryTestFixture();
            return countryFixture.GenerateCountryExpected();
        }

        public List<Address> GenerateAddress(int quantity)
        {
            var address = new Faker<Address>("pt_BR")
                            .CustomInstantiator(f => new Address(Guid.NewGuid(),
                                                                f.Address.ZipCode(),
                                                                f.Address.StreetName(),
                                                                f.Random.Number(1, 99999).ToString(),
                                                                f.Address.SecondaryAddress(),
                                                                f.Address.Direction(),
                                                                f.Address.City()));
            return address.Generate(quantity).ToList();
        }

        public Address GenerateValidAddress()
        {
            return GenerateAddress(1).FirstOrDefault();
        }

        public Address GenerateInvalidAddress()
        {
            return new Address(new Guid(), null, null, null, null, null, null);
        }
    }
}