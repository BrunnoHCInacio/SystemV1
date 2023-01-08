using Bogus;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(PeopleCollection))]
    public class PeopleCollection : ICollectionFixture<PeopleTestFixture>
    {
    }

    public class PeopleTestFixture : IDisposable
    {
        public dynamic GeneratePeopleExpected()
        {
            var faker = new Faker("pt_BR");
            var contactsFixture = new ContactTestFixture();
            var addressFixture = new AddressTestFixture();

            var contactPhone = contactsFixture.GenerateContact(EnumTypeContact.TypeContactPhone, 1).FirstOrDefault();
            var contactCellPhone = contactsFixture.GenerateContact(EnumTypeContact.TypeContactCellPhone, 1).FirstOrDefault();
            var contactEmail = contactsFixture.GenerateContact(EnumTypeContact.TypeContactEmail, 1).FirstOrDefault();
            var address = addressFixture.GenerateAddress(2).FirstOrDefault();
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Name.FullName(),
                Document = faker.Person.Cpf(),
                Address = address,
                ContactPhone = contactPhone,
                ContactCellPhone = contactCellPhone,
                ContactEmail = contactEmail
            };
        }

        public List<People> GenerateClient(int quantity,
                                           bool withAddress = true,
                                           bool hasValidAddress = true,
                                           bool withContact = true,
                                           bool hasValidContact = true)
        {
            var addressFixture = new AddressTestFixture();
            var contactsFixture = new ContactTestFixture();

            var client = new Faker<People>("pt_BR")
                            .CustomInstantiator(f => new People(Guid.NewGuid(),
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

        public void Dispose()
        {
        }
    }
}