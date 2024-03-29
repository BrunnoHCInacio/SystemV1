﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ContactCollection))]
    public class ContactCollection : ICollectionFixture<ContactTestFixture>
    { }

    public class ContactTestFixture : IDisposable
    {
        #region Generate Expected data

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

        #endregion Generate Expected data

        #region Generate Valid data

        public List<Contact> GenerateContact(EnumTypeContact typeContact, int quantity)
        {
            var contacts = new Faker<Contact>("pt_BR");
            var peopleId = Guid.NewGuid();
            if (typeContact == EnumTypeContact.TypeContactCellPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            new People(peopleId),
                                                            f.Random.Number(1, 999).ToString(),
                                                            $"+{f.Random.Number(1, 999)}",
                                                            f.Phone.PhoneNumber("#########")));
            }
            else if (typeContact == EnumTypeContact.TypeContactPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            new People(peopleId),
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
                                                           new People(peopleId),
                                                           email: f.Internet.Email()));
            }

            return contacts.Generate(quantity);
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

        #endregion Generate Valid data

        #region Generate invalid data

        public Contact GenerateInvalidContactTypeEmail()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactEmail, null);
        }

        public Contact GenerateInvalidContactTypePhone()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactPhone, null);
        }

        public Contact GenerateInvalidContactTypeCellPhone()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactCellPhone, null);
        }

        public Contact GenerateInvalidPhoneContactWithInvalidProperties()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactCellPhone, null, "", "", "222");
        }

        public Contact GenerateInvalidCellPhoneContactWithInvalidProperies()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactCellPhone, null, "", "", "123");
        }

        public Contact GenerateInvalidEmailContactWithInvalidProperies()
        {
            return new Contact(Guid.Empty, EnumTypeContact.TypeContactEmail, new People(Guid.Empty), email: "brunno");
        }

        #endregion Generate invalid data

        public List<ContactViewModel> GenerateValidContactViewModel(EnumTypeContact typeContact,
                                                                    int qty)
        {
            var contacts = GenerateContact(typeContact, qty);
            var contactsViewModel = new List<ContactViewModel>();

            contactsViewModel.AddRange(contacts.Select(c => new ContactViewModel
            {
                Id = c.Id,
                Ddd = c.Ddd,
                Ddi = c.Ddi,
                CellPhoneNumber = c.CellPhoneNumber,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                TypeContact = c.TypeContact
            }));
            return contactsViewModel;
        }

        public List<ContactViewModel> GenerateInvalidContactViewModel(EnumTypeContact typeContact,
                                                                    int qty)
        {
            var invalidContacts = new List<ContactViewModel>();
            Contact contact;

            for (int i = 0; i < qty; i++)
            {
                if (typeContact == EnumTypeContact.TypeContactCellPhone)
                {
                    contact = GenerateInvalidContactTypeCellPhone();
                }
                else if (typeContact == EnumTypeContact.TypeContactEmail)
                {
                    contact = GenerateInvalidContactTypeEmail();
                }
                else
                {
                    contact = GenerateInvalidContactTypePhone();
                }

                invalidContacts.Add(new ContactViewModel
                {
                    Id = contact.Id,
                    Ddd = contact.Ddd,
                    Ddi = contact.Ddi,
                    CellPhoneNumber = contact.CellPhoneNumber,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber
                });
            }
            return invalidContacts;
        }

        public List<ContactViewModel> GenerateContact(int qtyContacts,
                                                     bool isValidContact,
                                                     ContactTestFixture contactsFixture)
        {
            if (qtyContacts > 0)
            {
                if (isValidContact)
                {
                    List<ContactViewModel> contacts = contactsFixture.GenerateValidContactViewModel(EnumTypeContact.TypeContactCellPhone, qtyContacts);
                    contacts.AddRange(contactsFixture.GenerateValidContactViewModel(EnumTypeContact.TypeContactEmail, qtyContacts));
                    return contacts;
                }
                else
                {
                    List<ContactViewModel> contacts = new List<ContactViewModel>();

                    contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactCellPhone, qtyContacts));
                    contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactPhone, qtyContacts));
                    contacts.AddRange(contactsFixture.GenerateInvalidContactViewModel(EnumTypeContact.TypeContactEmail, qtyContacts));
                    return contacts;
                }
            }
            return null;
        }

        #region Disabled valid data

        public Contact GenerateValidContactTypeCellPhoneDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactCellPhone, 1).FirstOrDefault();
        }

        public Contact GenerateValidContactTypeEmailDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactEmail, 1).FirstOrDefault();
        }

        public Contact GenerateValidContactTypePhoneDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactPhone, 1).FirstOrDefault();
        }

        #endregion Disabled valid data

        public void Dispose()
        {
        }
    }
}