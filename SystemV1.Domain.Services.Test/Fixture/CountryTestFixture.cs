using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(CountryCollection))]
    public class CountryCollection : ICollectionFixture<CountryTestFixture> { }

    public class CountryTestFixture : IDisposable
    {
        public Country GenerateValidCountry()
        {
            var country = new Faker<Country>("pt_BR")
                                .CustomInstantiator(f => new Country(Guid.NewGuid(), f.Address.Country()));
            return country;
        }

        public Country GenerateInvalidCountry()
        {
            return new Country(Guid.NewGuid(), "");
        }

        public dynamic GenerateCountryExpected()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Address.Country()
            };
        }

        public dynamic GenerateStateValidExpected()
        {
            var stateFixture = new StateTestFixture();
            return stateFixture.GenerateStateValidExpected();
        }

        public void Dispose()
        {
        }
    }
}