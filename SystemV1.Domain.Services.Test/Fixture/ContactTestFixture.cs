using Bogus;
using System;
using System.Collections;
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
    [CollectionDefinition(nameof(ContactCollection))]
    public class ContactCollection : ICollectionFixture<ContactTestFixture> { }

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

        #endregion

        #region Generate Valid data
        public List<Contact> GenerateContact(EnumTypeContact typeContact, int quantity, bool registerActive = true)
        {
            var contacts = new Faker<Contact>("pt_BR");

            if (typeContact == EnumTypeContact.TypeContactCellPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            f.Random.Number(1, 999).ToString(),
                                                            $"+{f.Random.Number(1, 999)}",
                                                            f.Phone.PhoneNumber("#########")))
                        .FinishWith((f, c) =>
                        {
                            if (!registerActive) c.DisableRegister();
                        });
            }
            else if (typeContact == EnumTypeContact.TypeContactPhone)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                            typeContact,
                                                            f.Random.Number(1, 99).ToString(),
                                                            $"+{f.Random.Number(1, 99)}",
                                                            null,
                                                            f.Phone.PhoneNumber("########"),
                                                            null))
                        .FinishWith((f, c) =>
                        {
                            if (!registerActive) c.DisableRegister();
                        }); ;
            }
            else if (typeContact == EnumTypeContact.TypeContactEmail)
            {
                contacts.CustomInstantiator(f => new Contact(Guid.NewGuid(),
                                                           typeContact,
                                                           email: f.Internet.Email()))
                        .FinishWith((f, c) =>
                        {
                            if (!registerActive) c.DisableRegister();
                        }); ;
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
        #endregion

        #region Generate invalid data
        public Contact GenerateInvalidContactTypeEmail()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactEmail);
        }
        public Contact GenerateInvalidContactTypePhone()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactPhone);
        }
        public Contact GenerateInvalidContactTypeCellPhone()
        {
            return new Contact(new Guid(), EnumTypeContact.TypeContactCellPhone);
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
        #endregion

        
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
                PhoneNumber = c.PhoneNumber
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

        #region Disabled valid data

        public Contact GenerateValidContactTypeCellPhoneDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactCellPhone, 1, false).FirstOrDefault();
        }

        public Contact GenerateValidContactTypeEmailDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactEmail, 1, false).FirstOrDefault();
        }

        public Contact GenerateValidContactTypePhoneDisabled()
        {
            return GenerateContact(EnumTypeContact.TypeContactPhone, 1, false).FirstOrDefault();
        }
        #endregion

        public void Dispose()
        {
        }
    }
}