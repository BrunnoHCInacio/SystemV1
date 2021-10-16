using Bogus;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientCollection : ICollectionFixture<ClientTestFixture>
    {
    }

    public class ClientTestFixture : IDisposable
    {
        public dynamic GenerateClientExpected()
        {
            var faker = new Faker("pt_BR");
            var addressFixture = new AddressTestFixture();
            var contactFixture = new ContactTestFixture();
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Name.FullName(),
                Document = faker.Person.Cpf(),
                Address = addressFixture.GenerateAddressExpected(),
                ContactEmail = contactFixture.GenerateValidContactExpectedTypeEmail(),
                ContactPhone = contactFixture.GenerateValidContactExpectedTypePhone(),
                ContactCellPhone = contactFixture.GenerateValidContactExpectedTypeCellPhone()
            };
        }

        public List<Client> GenerateClient(int quantity,
                                           bool withAddress = true,
                                           bool hasValidAddress = true,
                                           bool withContact = true,
                                           bool hasValidContact = true)
        {
            var addressFixture = new AddressTestFixture();
            var contactsFixture = new ContactTestFixture();

            var client = new Faker<Client>("pt_BR")
                            .CustomInstantiator(f => new Client(Guid.NewGuid(),
                                                                f.Name.FullName(),
                                                                f.Person.Cpf()))
                            .FinishWith((f, c) =>
                            {
                                if (withAddress)
                                {
                                    if (hasValidAddress)
                                    {
                                        c.AddAddresses(addressFixture.GenerateAddress(2));
                                    }
                                    else
                                    {
                                        c.AddAddress(addressFixture.GenerateInvalidAddress());
                                    }
                                }
                                if (withContact)
                                {
                                    if (hasValidContact)
                                    {
                                        c.AddContacts(contactsFixture.GenerateContact(EnumTypeContact.TypeContactCellPhone, 2));
                                        c.AddContacts(contactsFixture.GenerateContact(EnumTypeContact.TypeContactEmail, 1));
                                    }
                                    else
                                    {
                                        c.AddContact(contactsFixture.GenerateInvalidContactTypeCellPhone());
                                        c.AddContact(contactsFixture.GenerateInvalidContactTypeEmail());
                                        c.AddContact(contactsFixture.GenerateInvalidContactTypePhone());
                                    }
                                }
                            });

            return client.Generate(quantity);
        }

        public Client GenerateValidClient()
        {
            return GenerateClient(1).FirstOrDefault();
        }

        public Client GenerateInvalidClient()
        {
            return new Client(new Guid(), null, null);
        }

        public List<ClientViewModel> GenerateClientViewModel(int qtyClients,
                                                             int qtyAddress,
                                                             int qtyContacts,
                                                             bool withAddress = true,
                                                             bool hasValidAddress = true,
                                                             bool withContact = true,
                                                             bool hasValidContact = true)
        {
            var addressFixture = new AddressTestFixture();
            var contactsFixture = new ContactTestFixture();

            var client = new Faker<ClientViewModel>("pt_BR")
                                .FinishWith(
                                (f, c) =>
                                {
                                    c.Name = f.Name.FullName();
                                    c.Document = f.Person.Cpf();
                                    if (withAddress)
                                    {
                                        if (hasValidAddress)
                                        {
                                            c.Addresses = addressFixture.GenerateValidAddressViewModel(qtyAddress);
                                        }
                                        else
                                        {
                                            c.Addresses = addressFixture.GenerateInvalidAddressViewModel();
                                        }
                                    }
                                    if (withContact)
                                    {
                                        if (hasValidContact)
                                        {
                                            List<ContactViewModel> contacts = contactsFixture.GenerateValidContactViewModel(EnumTypeContact.TypeContactCellPhone, qtyContacts);
                                            contacts.AddRange(contactsFixture.GenerateValidContactViewModel(EnumTypeContact.TypeContactEmail, qtyContacts));

                                            c.Contacts = contacts;
                                        }
                                        else
                                        {
                                            List<ContactViewModel> contacts = new List<ContactViewModel>();

                                            contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactCellPhone, qtyContacts));
                                            contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactPhone, qtyContacts));
                                            contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactEmail, qtyContacts));
                                        }
                                    }
                                });
            return client.Generate(qtyClients);
        }

        public ClientViewModel GenerateInvalidClientViewModel()
        {
            return new ClientViewModel();
        }

        public void Dispose()
        {
        }
    }
}