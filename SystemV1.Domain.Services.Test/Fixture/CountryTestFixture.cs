using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(CountryCollection))]
    public class CountryCollection : ICollectionFixture<CountryTestFixture>
    { }

    public class CountryTestFixture : IDisposable
    {
        #region Generate valid data

        public List<Country> GenerateCountry(int quantity)
        {
            var country = new Faker<Country>("pt_BR")
                                .CustomInstantiator(f => new Country(Guid.NewGuid(), f.Address.Country(), null));

            return country.Generate(quantity);
        }

        public Country GenerateValidCountry()
        {
            return GenerateCountry(1).FirstOrDefault();
        }

        public Country GenerateValidCountryWithStates()
        {
            var stateFixture = new StateTestFixture();
            var country = GenerateValidCountry();
            country.AddStates(stateFixture.GenerateStates(quantity: 3, country: country));
            return country;
        }

        #endregion Generate valid data

        #region Generate invalid data

        public Country GenerateInvalidCountry()
        {
            return new Country(Guid.NewGuid(), "", null);
        }

        public Country GenerateInvalidCountryWithStates()
        {
            var country = new Country(Guid.NewGuid(), "", null);
            var stateFixture = new StateTestFixture();
            country.AddState(stateFixture.GenerateInvalidState());
            country.AddState(stateFixture.GenerateInvalidState());
            country.AddState(stateFixture.GenerateInvalidState());

            return country;
        }

        public Country GenerateValidCountryWithInvalidStates()
        {
            var country = GenerateValidCountry();
            var stateFixture = new StateTestFixture();
            country.AddState(stateFixture.GenerateInvalidState());
            country.AddState(stateFixture.GenerateInvalidState());
            country.AddState(stateFixture.GenerateInvalidState());

            return country;
        }

        #endregion Generate invalid data

        #region Generate expected data

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

        #endregion Generate expected data

        public List<CountryViewModel> GenerateCountryViewModel(int qty, Guid? id = null)
        {
            var country = new Faker<CountryViewModel>("pt_BR").FinishWith((f, c) =>
            {
                c.Id = id;
                c.Name = f.Address.Country();
             
            });

            return country.Generate(qty);
        }

        public CountryViewModel GenerateValidContryViewModel(Guid? id = null)
        {
            return GenerateCountryViewModel(1, id).FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}