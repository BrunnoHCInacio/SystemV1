using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(CityCollection))]
    public class CityCollection : ICollectionFixture<CityTestFixture>
    {
    }
    public class CityTestFixture : IDisposable
    {
        public List<City> Generate(int qty)
        {
            var faker = new Faker<City>("pt_BR");
            var stateFixture = new StateTestFixture();
            return faker.CustomInstantiator(f => new City(Guid.NewGuid(), f.Address.City(), stateFixture.GenerateValidState())).Generate(qty);
        }

        public dynamic GenerateExpectedCity()
        {
            var faker = new Faker("pt_BR");
            var stateFixture = new StateTestFixture();
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Address.City(),
                State = stateFixture.GenerateStateValidExpected()
            };
        }

        public City GenerateValidCity()
        {
            return Generate(1).FirstOrDefault();
        }

        public City GenerateInvalidCity()
        {
            return new City(Guid.Empty, "", null);
        }

        public void Dispose()
        {
        }
    }
}
