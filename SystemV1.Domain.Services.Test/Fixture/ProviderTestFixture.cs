using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                               bool withContact = true,
                                               bool useValidContact = true,
                                               bool withAddress = true,
                                               bool useValidAddress = true)
        {
            var addressFixture = new AddressTestFixture();
            var contactFixtute = new ContactTestFixture();
            var provider = new Faker<Provider>()
                            .CustomInstantiator(f => new Provider(Guid.NewGuid(), f.Person.FullName, f.Company.Cnpj()))
                            .FinishWith((f, p) =>
                            {
                                if (withAddress)
                                {
                                    if (useValidAddress)
                                    {
                                        p.AddAddresses(addressFixture.GenerateAddress(2));
                                    }
                                    else
                                    {
                                        p.AddAddress(addressFixture.GenerateInvalidAddress());
                                        p.AddAddress(addressFixture.GenerateInvalidAddress());
                                        p.AddAddress(addressFixture.GenerateInvalidAddress());
                                    }
                                }

                                if (withContact)
                                {
                                    if (useValidContact)
                                    {
                                        p.AddContacts(contactFixtute.GenerateContact(Enums.EnumTypeContact.TypeContactCellPhone, 2));
                                        p.AddContact(contactFixtute.GenerateValidContactTypeEmail());
                                    }
                                    else
                                    {
                                        p.AddContact(contactFixtute.GenerateInvalidContactTypeCellPhone());
                                        p.AddContact(contactFixtute.GenerateInvalidContactTypeEmail());
                                        p.AddContact(contactFixtute.GenerateInvalidContactTypePhone());
                                    }
                                }
                            });

            return provider.Generate(quantity);
        }

        public Provider GenerateValidProvider(bool withContact = true,
                                              bool useValidContact = true,
                                              bool withAddress = true,
                                              bool useValidAddress =  true)
        {
            return GenerateProvider(1, withContact, useValidContact, withAddress, useValidAddress).FirstOrDefault();
        }

        public Provider GenerateInvalidProvider()
        {
            return new Provider(new Guid(), null, null);
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