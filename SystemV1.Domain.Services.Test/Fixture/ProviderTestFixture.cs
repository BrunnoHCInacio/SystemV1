using Bogus;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ProviderCollection))]
    public class ProviderCollection : ICollectionFixture<ProviderTestFixture>
    {
    }

    public class ProviderTestFixture : IDisposable
    {
        public List<Provider> GenerateProvider(int quantity,
                                               Guid peopleId)
        {
            var addressFixture = new AddressTestFixture();
            var contactFixtute = new ContactTestFixture();
            var provider = new Faker<Provider>()
                            .CustomInstantiator(f => new Provider(Guid.NewGuid(), peopleId));

            return provider.Generate(quantity);
        }

        public Provider GenerateValidProvider(Guid peopleId)
        {
            return GenerateProvider(1, peopleId).FirstOrDefault();
        }

        public Provider GenerateInvalidProvider()
        {
            return new Provider(Guid.Empty);
        }

        public dynamic GenerateProviderExpected()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Document = faker.Company.Cnpj()
            };
        }

        public List<ProviderViewModel> GenerateValidProviderViewModel(int qty, Guid peopleId)
        {
            var client = new Faker<ProviderViewModel>("pt_BR")
                                .FinishWith(
                                (f, p) =>
                                {
                                    p.PeopleId = peopleId;
                                });
            return client.Generate(qty);
        }

        public void Dispose()
        {
        }
    }
}