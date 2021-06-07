using Bogus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ContactCollection))]
    public class ContactCollection : ICollectionFixture<ContactTestFixture> { }

    public class ContactTestFixture : IDisposable
    {
        public dynamic GenerateValidContactExpectedTypeEmail()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                TypeContact = EnumTypeContact.TypeContactEmail,
                Email = faker.Internet.Email()
            };
        }

        public dynamic GenerateValidContactExpectedTypePhone()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                TypeContact = EnumTypeContact.TypeContactPhone,
                Ddd = faker.Random.Number(1, 99).ToString(),
                Ddi = $"+{faker.Random.Number(1, 99)}",
                PhoneNumber = faker.Phone.PhoneNumber("####-####")
            };
        }

        public dynamic GenerateValidContactExpectedTypeCellPhone()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                TypeContact = EnumTypeContact.TypeContactCellPhone,
                Ddd = faker.Random.Number(1, 99).ToString(),
                Ddi = $"+{faker.Random.Number(1, 99)}",
                CellPhoneNumber = faker.Phone.PhoneNumber("#########")
            };
        }

        public Contact GenerateValidContactTypePhone()
        {
            return GenerateContact(EnumTypeContact.TypeContactPhone, 1).FirstOrDefault();
        }

        public Contact GenerateValidContactTypeCellPhone()
        {
            return GenerateContact(EnumTypeContact.TypeContactCellPhone, 1).FirstOrDefault();
        }

        public Contact GenerateValidContactTypeEmail()
        {
            return GenerateContact(EnumTypeContact.TypeContactEmail, 1).FirstOrDefault();
        }

        public Contact GenerateInvalidContactTypeEmail()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactEmail, null, null, null, null, null);
        }

        public Contact GenerateInvalidPhoneContactWithInvalidProperties()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactCellPhone, "", "", "222");
        }

        public Contact GenerateInvalidCellPhoneContactWithInvalidProperies()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactCellPhone, "", "", "123");
        }

        public Contact GenerateInvalidEmailContactWithInvalidProperies()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactEmail, email: "brunno");
        }

        public Contact GenerateInvalidContactTypePhone()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactPhone);
        }

        public Contact GenerateInvalidContactTypeCellPhone()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactCellPhone);
        }

        public List<Contact> GenerateContact(EnumTypeContact typeContact, int quantity)
        {
            var contacts = new Faker<Contact>("pt_BR");

            if (typeContact == EnumTypeContact.TypeContactCellPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            f.Random.Number(1, 999).ToString(),
                                                            $"+{f.Random.Number(1, 999)}",
                                                            f.Phone.PhoneNumber("#########")));
            }
            else if (typeContact == EnumTypeContact.TypeContactPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            f.Random.Number(1, 99).ToString(),
                                                            $"+{f.Random.Number(1, 99)}",
                                                            null,
                                                            f.Phone.PhoneNumber("########"),
                                                            null));
            }
            else if (typeContact == EnumTypeContact.TypeContactEmail)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                           typeContact,
                                                           email: f.Internet.Email()));
            }

            return contacts.Generate(quantity);
        }

        public void Dispose()
        {
        }
    }
}