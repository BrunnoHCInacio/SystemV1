using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(ClientCollection))]
    public class ClientDomainTest
    {
        private readonly ClientTestFixture _clientTestFixture;

        public ClientDomainTest(ClientTestFixture clientTestFixture)
        {
            _clientTestFixture = clientTestFixture;
        }

        [Fact(DisplayName = "Validar as propriedade do domínio Client")]
        [Trait("Categoria", "Cadastro - Cliente")]
        public void Client_NewClient_ValidateAllPropertiesDomain()
        {
            //Arrange
            var clientExpected = _clientTestFixture.GenerateClientExpected();
            var addressClient = new Address(clientExpected.Address.Id,
                            clientExpected.Address.ZipCode,
                            clientExpected.Address.Street,
                            clientExpected.Address.Number,
                            clientExpected.Address.Complement,
                            clientExpected.Address.District,
                            clientExpected.Address.City);

            var state = new State(clientExpected.Address.State.Id,
                                  clientExpected.Address.State.Name);

            addressClient.SetState(state);

            var contactClient = new List<Contact>
            {
                new Contact(clientExpected.ContactPhone.Id,
                            clientExpected.ContactPhone.TypeContact,
                            clientExpected.ContactPhone.Ddd,
                            clientExpected.ContactPhone.Ddi,
                            null,
                            clientExpected.ContactPhone.PhoneNumber,
                            null) ,

                new Contact(clientExpected.ContactCellPhone.Id,
                            clientExpected.ContactCellPhone.TypeContact,
                            clientExpected.ContactCellPhone.Ddd,
                            clientExpected.ContactCellPhone.Ddi,
                            clientExpected.ContactCellPhone.CellPhoneNumber,
                            null,
                            null),

                new Contact(clientExpected.ContactEmail.Id,
                            clientExpected.ContactEmail.TypeContact,
                            null,
                            null,
                            null,
                            null,
                            clientExpected.ContactEmail.Email)
            };

            //Act
            var client = new Client(clientExpected.Id,
                                    clientExpected.Name,
                                    clientExpected.Document);

            client.AddAddress(addressClient);
            client.AddContacts(contactClient);

            //Assert
            Assert.Equal(clientExpected.Id, client.Id);
            Assert.Equal(clientExpected.Name, client.Name);
            Assert.Equal(clientExpected.Document, client.Document);

            foreach (var address in client.Addresses)
            {
                Assert.Equal(clientExpected.Address.Id, address.Id);
                Assert.Equal(clientExpected.Address.ZipCode, address.ZipCode);
                Assert.Equal(clientExpected.Address.Street, address.Street);
                Assert.Equal(clientExpected.Address.Number, address.Number);
                Assert.Equal(clientExpected.Address.Complement, address.Complement);
                Assert.Equal(clientExpected.Address.District, address.District);
                Assert.Equal(clientExpected.Address.State.Id, address.State.Id);
                Assert.Equal(clientExpected.Address.State.Name, address.State.Name);
                Assert.Equal(clientExpected.Address.City, address.City);
            }

            foreach (var contact in client.Contacts)
            {
                if (contact.TypeContact == EnumTypeContact.TypeContactEmail)
                {
                    Assert.Equal(clientExpected.ContactEmail.Id, contact.Id);
                    Assert.Equal(clientExpected.ContactEmail.Email, contact.Email);
                }
                if (contact.TypeContact == EnumTypeContact.TypeContactCellPhone)
                {
                    Assert.Equal(clientExpected.ContactCellPhone.Id, contact.Id);
                    Assert.Equal(clientExpected.ContactCellPhone.Ddd, contact.Ddd);
                    Assert.Equal(clientExpected.ContactCellPhone.Ddi, contact.Ddi);
                    Assert.Equal(clientExpected.ContactCellPhone.CellPhoneNumber, contact.CellPhoneNumber);
                }
                if (contact.TypeContact == EnumTypeContact.TypeContactPhone)
                {
                    Assert.Equal(clientExpected.ContactPhone.Id, contact.Id);
                    Assert.Equal(clientExpected.ContactPhone.Ddd, contact.Ddd);
                    Assert.Equal(clientExpected.ContactPhone.Ddi, contact.Ddi);
                    Assert.Equal(clientExpected.ContactPhone.PhoneNumber, contact.PhoneNumber);
                }
            }
        }

        [Fact(DisplayName = "Validate valid client")]
        [Trait("Categoria", "Cadastro - Cliente")]
        public void Client_ValidateClient_ShouldBeValid()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();

            //Act
            var result = client.ValidateClient();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate invalid client")]
        [Trait("Categoria", "Cadastro - Cliente")]
        public void Client_ValidateClient_ShouldBeInvalid()
        {
            //Arrange
            var client = _clientTestFixture.GenerateInvalidClient();

            //Act
            var result = client.ValidateClient();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(2, result.Errors.Count);

            Assert.Contains(ClientValidation.NameRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ClientValidation.DocumentRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}