using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;

namespace SystemV1.Domain.Test.Fixture
{
    public class CityTestFixture : IDisposable
    {
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
            var faker = new Faker("pt_BR");
            var stateFixture = new StateTestFixture();
            return new City(Guid.NewGuid(), faker.Address.City(), stateFixture.GenerateValidState());
        }

        public City GenerateInvalidCity()
        {
            return new City(Guid.Empty, "", null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
