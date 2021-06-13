using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Services.Test.Fixture
{
    [CollectionDefinition(nameof(StateCollection))]
    public class StateCollection : ICollectionFixture<StateTestFixture>
    {
    }

    public class StateTestFixture : IDisposable
    {
        public List<State> GenerateStates(int quantity)
        {
            //return new State(Guid.NewGuid(), "Goias");
            var countryFixture = new CountryTestFixture();

            var state = new Faker<State>("pt_BR")
                                .CustomInstantiator(f => new State(Guid.NewGuid(),
                                                    f.Address.State()))
                                .FinishWith((f, s) =>
                                {
                                    s.SetCountry(countryFixture.GenerateValidCountry());
                                });
            return state.Generate(quantity);
        }

        public State GenerateValidState()
        {
            return GenerateStates(1).FirstOrDefault();
        }

        public State GenerateInvalidState()
        {
            return new State(new Guid(), "");
        }

        public dynamic GenerateStateWithCountryValidExpected()
        {
            var countryFixture = new CountryTestFixture();
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Address.State(),
                Country = countryFixture.GenerateCountryExpected()
            };
        }

        public dynamic GenerateStateValidExpected()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Address.State()
            };
        }

        public void Dispose()
        {
        }
    }
}