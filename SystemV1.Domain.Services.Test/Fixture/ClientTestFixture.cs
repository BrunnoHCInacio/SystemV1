using Bogus;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                                           bool withContact = true)
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
                                    c.AddAddresses(addressFixture.GenerateAddress(2));
                                }
                                if (withContact)
                                {
                                    c.AddContacts(contactsFixture.GenerateContact(EnumTypeContact.TypeContactCellPhone, 2));
                                    c.AddContacts(contactsFixture.GenerateContact(EnumTypeContact.TypeContactEmail, 1));
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

        public void Dispose()
        {
        }
    }
}