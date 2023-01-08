using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.ViewModels;
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
        public List<State> GenerateStates(int quantity,
                                          Country country = null)
        {
            var countryFixture = new CountryTestFixture();

            var state = new Faker<State>("pt_BR")
                                .CustomInstantiator(f => new State(Guid.NewGuid(),
                                                    f.Address.State()))
                                .FinishWith((f, s) =>
                                {
                                    s.SetCountry(country);
                                });
            return state.Generate(quantity);
        }

        public State GenerateValidState(Country country = null)
        {
            return GenerateStates(quantity: 1, country: country).FirstOrDefault();
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

        public List<StateViewModel> GenerateStatesViewModel(int qty, Guid? id = null, Guid? countryId = null)
        {
            var state = new Faker<StateViewModel>("pt_BR")
                            .FinishWith((f, s) =>
                            {
                                s.Name = f.Address.State();
                                s.Id = id.GetValueOrDefault();
                                s.CountryId = countryId.GetValueOrDefault();
                            });

            return state.Generate(qty);
        }

        public StateViewModel GenerateValidStateViewModel(Guid? id = null, Guid? countryId = null)
        {
            return GenerateStatesViewModel(1, id, countryId).FirstOrDefault();
        }

        public StateViewModel GenerateInvalidStateViewModel()
        {
            return new StateViewModel();
        }

        public void Dispose()
        {
        }
    }
}