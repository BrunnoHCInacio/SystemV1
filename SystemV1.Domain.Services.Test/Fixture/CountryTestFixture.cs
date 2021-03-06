using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(CountryCollection))]
    public class CountryCollection : ICollectionFixture<CountryTestFixture> { }

    public class CountryTestFixture : IDisposable
    {
        public List<Country> GenerateCountry(int quantity)
        {
            var country = new Faker<Country>("pt_BR")
                                .CustomInstantiator(f => new Country(Guid.NewGuid(), f.Address.Country()));
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
            country.AddStates(stateFixture.GenerateStates(3));
            return country;
        }

        public Country GenerateInvalidCountry()
        {
            return new Country(Guid.NewGuid(), "");
        }

        public Country GenerateInvalidCountryWithStates()
        {
            var country = new Country(Guid.NewGuid(), "");
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

        public List<CountryViewModel> GenerateCountryViewModel(int quantity)
        {
            var country = new Faker<CountryViewModel>("pt_BR").RuleFor(c => c.Name, f => f.Address.Country());

            return country.Generate(quantity);
        }

        public CountryViewModel GenerateValidContryViewModel()
        {
            return GenerateCountryViewModel(1).FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}