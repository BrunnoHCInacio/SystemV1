using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(PeopleCollection))]
    public class PeopleDomainTest
    {
        private readonly PeopleTestFixture _fixture;

        public PeopleDomainTest(PeopleTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = "Test for mask document client")]
        [Trait("UnitTests - Entity", "People")]
        [InlineData("62437538089", true)]
        [InlineData("493.215.800-93", true)]
        [InlineData("49.792.228/0001-50", false)]
        [InlineData("15316749000110", false)]
        public void Client_SetDocument_ShouldFormateDocument(string document, bool isCPF)
        {
            //Arrange and act
            var client = new People(Guid.NewGuid(), "", document);
            var documentWithoutMask = Regex.Replace(document, @"[^0-9]", string.Empty);
            //Assert
            if (isCPF)
            {
                Assert.NotEmpty(client.Document);
                Assert.True(client.Document.Contains(".") && client.Document.Contains("-"));
                Assert.Equal(Convert.ToUInt64(documentWithoutMask).ToString(@"000\.000\.000\-00"), client.Document);
            }
            else
            {
                Assert.NotEmpty(client.Document);
                Assert.True(client.Document.Contains(".")
                            && client.Document.Contains("-")
                            && client.Document.Contains("/"));

                Assert.Equal(Convert.ToUInt64(documentWithoutMask).ToString(@"00\.000\.000\/0000\-00"), client.Document);
            }

            foreach (var letter in document)
            {
                Assert.True(client.Document.Contains(letter));
            }
        }

        [Fact(DisplayName = "Validar as propriedade do domínio pessoa")]
        [Trait("UnitTests - Entity", "People")]
        public void Client_NewClient_ValidateAllPropertiesDomain()
        {
            //Arrange
            var peopleExpected = _fixture.GeneratePeopleExpected();
            var addressClient = new Address(peopleExpected.Address.Id,
                                            peopleExpected.Address.ZipCode,
                                            peopleExpected.Address.Street,
                                            peopleExpected.Address.Number,
                                            peopleExpected.Address.Complement,
                                            peopleExpected.Address.District,
                                            peopleExpected.Address.CityId,
                                            peopleExpected.Id);

            ////Act
            var people = new People(peopleExpected.Id,
                                    peopleExpected.Name,
                                    peopleExpected.Document);

            var contactClient = new List<Contact>
            {
                new Contact(peopleExpected.ContactPhone.Id,
                            peopleExpected.ContactPhone.TypeContact,
                            people,
                            peopleExpected.ContactPhone.Ddd,
                            peopleExpected.ContactPhone.Ddi,
                            null,
                            peopleExpected.ContactPhone.PhoneNumber,
                            null) ,

                new Contact(peopleExpected.ContactCellPhone.Id,
                            peopleExpected.ContactCellPhone.TypeContact,
                            people,
                            peopleExpected.ContactCellPhone.Ddd,
                            peopleExpected.ContactCellPhone.Ddi,
                            peopleExpected.ContactCellPhone.CellPhoneNumber,
                            null,
                            null),

                new Contact(peopleExpected.ContactEmail.Id,
                            peopleExpected.ContactEmail.TypeContact,
                            people,
                            null,
                            null,
                            null,
                            null,
                            peopleExpected.ContactEmail.Email)
            };

            people.AddAddress(addressClient);
            people.AddContacts(contactClient);

            ////Assert
            Assert.Equal(peopleExpected.Id, people.Id);
            Assert.Equal(peopleExpected.Name, people.Name);
            Assert.Equal(peopleExpected.Document, people.Document);

            foreach (var address in people.Addresses)
            {
                Assert.Equal(peopleExpected.Address.Id, address.Id);
                Assert.Equal(peopleExpected.Address.ZipCode, address.ZipCode);
                Assert.Equal(peopleExpected.Address.Street, address.Street);
                Assert.Equal(peopleExpected.Address.Number, address.Number);
                Assert.Equal(peopleExpected.Address.Complement, address.Complement);
                Assert.Equal(peopleExpected.Address.District, address.District);
                Assert.Equal(peopleExpected.Address.CityId, address.CityId);
            }

            foreach (var contact in people.Contacts)
            {
                if (contact.TypeContact == EnumTypeContact.TypeContactEmail)
                {
                    Assert.Equal(peopleExpected.ContactEmail.Id, contact.Id);
                    Assert.Equal(peopleExpected.ContactEmail.Email, contact.Email);
                }
                if (contact.TypeContact == EnumTypeContact.TypeContactCellPhone)
                {
                    Assert.Equal(peopleExpected.ContactCellPhone.Id, contact.Id);
                    Assert.Equal(peopleExpected.ContactCellPhone.Ddd, contact.Ddd);
                    Assert.Equal(peopleExpected.ContactCellPhone.Ddi, contact.Ddi);
                    Assert.Equal(peopleExpected.ContactCellPhone.CellPhoneNumber, contact.CellPhoneNumber);
                }
                if (contact.TypeContact == EnumTypeContact.TypeContactPhone)
                {
                    Assert.Equal(peopleExpected.ContactPhone.Id, contact.Id);
                    Assert.Equal(peopleExpected.ContactPhone.Ddd, contact.Ddd);
                    Assert.Equal(peopleExpected.ContactPhone.Ddi, contact.Ddi);
                    Assert.Equal(peopleExpected.ContactPhone.PhoneNumber, contact.PhoneNumber);
                }
            }
        }
    }
}