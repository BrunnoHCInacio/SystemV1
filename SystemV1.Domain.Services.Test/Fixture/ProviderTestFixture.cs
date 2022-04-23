using Bogus;
using Bogus.DataSets;
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
                Name = faker.Person.FullName,
                Document = faker.Person.Cpf()
            };
        }

        public List<ProviderViewModel> GenerateValidProviderViewModel(int qty,
                                                                      List<CityViewModel> citiesViewModel,
                                                                      int qtyAddress = 0,
                                                                      int qtyContacts = 0,
                                                                      bool isValidAddress = true,
                                                                      bool isValidContact = true)
        {
            var addressFixture = new AddressTestFixture();
            var contactsFixture = new ContactTestFixture();

            var client = new Faker<ProviderViewModel>("pt_BR")
                                .FinishWith(
                                (f, p) =>
                                {
                                    p.Name = f.Company.CompanyName();
                                    p.Document = f.Company.Cnpj();
                                    p.Addresses = addressFixture.GenerateAddress(qtyAddress, isValidAddress, addressFixture, citiesViewModel);
                                    p.Contacts =  contactsFixture.GenerateContact(qtyContacts, isValidContact, contactsFixture);
                                });
            return client.Generate(qty);
        }



        public void Dispose()
        {
        }
    }
}