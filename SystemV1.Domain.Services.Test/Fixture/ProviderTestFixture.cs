using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<Provider> GenerateProvider(int quantity)
        {
            var addressFixture = new AddressTestFixture();
            var contactFixtute = new ContactTestFixture();
            var provider = new Faker<Provider>()
                            .CustomInstantiator(f => new Provider(Guid.NewGuid(), f.Person.FullName))
                            .FinishWith((f, p) =>
                            {
                                p.AddAddresses(addressFixture.GenerateAddress(2));
                                p.AddContacts(contactFixtute.GenerateContact(Enums.EnumTypeContact.TypeContactCellPhone, 2));
                                p.AddContact(contactFixtute.GenerateValidContactTypeEmail());
                            });

            return provider.Generate(quantity);
        }

        public Provider GenerateValidProvider()
        {
            return GenerateProvider(1).FirstOrDefault();
        }

        public Provider GenerateInvalidProvider()
        {
            return new Provider(new Guid(), "");
        }

        public dynamic GenerateProviderExpected()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Person.FullName
            };
        }

        public void Dispose()
        {
        }
    }
}