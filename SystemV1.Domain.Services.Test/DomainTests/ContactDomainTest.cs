using System;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test
{
    [Collection(nameof(ContactCollection))]
    public class ContactDomainTest
    {
        public readonly ContactTestFixture _contactTestFixture;

        public ContactDomainTest(ContactTestFixture contactTestFixture)
        {
            _contactTestFixture = contactTestFixture;
        }

        #region Test validate for each property from entity

        [Fact(DisplayName = "Validate set as correct properties for phone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContact_ShouldSetCorrectPropertiesForPhoneWithSuccess()
        {
            //Arrange
            var contactPhoneExpected = _contactTestFixture.GenerateValidContactExpectedTypePhone();

            //Act
            var phone = new Contact(contactPhoneExpected.Id,
                                    contactPhoneExpected.TypeContact,
                                    contactPhoneExpected.Ddd,
                                    contactPhoneExpected.Ddi,
                                    null,
                                    contactPhoneExpected.PhoneNumber,
                                    null);

            //Assert
            Assert.Equal(contactPhoneExpected.Id, phone.Id);
            Assert.Equal(contactPhoneExpected.TypeContact, phone.TypeContact);
            Assert.Equal(contactPhoneExpected.Ddd, phone.Ddd);
            Assert.Equal(contactPhoneExpected.Ddi, phone.Ddi);
            Assert.Equal(contactPhoneExpected.PhoneNumber, phone.PhoneNumber);

        }

        [Fact(DisplayName ="Validate set as correct properties for cellphone")]
        [Trait("Categoria", "Cadastro Contato")]
        public void Contact_NewContact_ShouldSetCorrectPropertiresForCellPhoneWithSuccess()
        {
            //Arrange
            var contactCellPhoneExpected = _contactTestFixture.GenerateValidContactExpectedTypeCellPhone();

            //Act
            var cellPhone = new Contact(contactCellPhoneExpected.Id,
                                       contactCellPhoneExpected.TypeContact,
                                       contactCellPhoneExpected.Ddd,
                                       contactCellPhoneExpected.Ddi,
                                       contactCellPhoneExpected.CellPhoneNumber,
                                       null,
                                       null);

            //Assert
            Assert.Equal(contactCellPhoneExpected.Id, cellPhone.Id);
            Assert.Equal(contactCellPhoneExpected.TypeContact, cellPhone.TypeContact);
            Assert.Equal(contactCellPhoneExpected.Ddd, cellPhone.Ddd);
            Assert.Equal(contactCellPhoneExpected.Ddi, cellPhone.Ddi);
            Assert.Equal(contactCellPhoneExpected.CellPhoneNumber, cellPhone.CellPhoneNumber);
        }

        [Fact(DisplayName = "Validade set as correct proprerties for email") ]
        [Trait("Categoria", "Cadastro Contato")]
        public void Contact_NewContact_ShouldSetCorrectPropertiesForEmailWithSuccess()
        {
            //Arrange 
            var contactEmailExpected = _contactTestFixture.GenerateValidContactExpectedTypeEmail();

            //Act
            var email = new Contact(contactEmailExpected.Id,
                                   contactEmailExpected.TypeContact,
                                   null,
                                   null,
                                   null,
                                   null,
                                   contactEmailExpected.Email);

            //Assert
            Assert.Equal(contactEmailExpected.Id, email.Id);
            Assert.Equal(contactEmailExpected.TypeContact, email.TypeContact);
            Assert.Equal(contactEmailExpected.Email, email.Email);
        }

        #endregion

        #region Test validation for data

        [Fact(DisplayName = "Validate valid contact type phone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypePhone_ShouldBeValid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.True(result.IsValid);
            Assert.False(result.Errors.Any());
        }

        [Fact(DisplayName = "Validate valid contact type cellphone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeCellPhone_ShouldBeValid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.True(result.IsValid);
            Assert.False(result.Errors.Any());
        }

        [Fact(DisplayName = "Validate valid contact type email")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeEmail_ShouldBeValid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.True(result.IsValid);
            Assert.False(result.Errors.Any());
        }

        [Fact(DisplayName = "Validate invalid contact type email")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeEmail_ShouldBeInvalid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidContactTypeEmail();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            
            Assert.Contains(ContactValidation.EmailRequired, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate invalid contact type phone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypePhone_ShouldBeInvalid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidContactTypePhone();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(4, result.Errors.Count);

            Assert.Contains(ContactValidation.DddRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.DddMinLength, result.Errors.Select(e => e.ErrorMessage));

            Assert.Contains(ContactValidation.PhoneNumberRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.PhoneNumberMinLength, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate invalid contact type phone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypePhone_ShouldHavePropertiesInvalid()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidContactTypePhone();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(4, result.Errors.Count);

            Assert.Contains(ContactValidation.DddRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.DddMinLength, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.PhoneNumberMaxLength, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.PhoneNumberMinLength, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate invalid contact type cellphone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeCellPhone_ShouldBeInvalid()
        {
            var contact = _contactTestFixture.GenerateInvalidCellPhoneContactWithInvalidProperies();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(3, result.Errors.Count);

            Assert.Contains(ContactValidation.DddRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(ContactValidation.DddMinLength, result.Errors.Select(e => e.ErrorMessage));

            Assert.Contains(ContactValidation.CellPhoneMinLength, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate disabled valid contact type phone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypePhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhoneDisabled();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Single(result.Errors);

            Assert.Contains(ContactValidation.ContactNotActive, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate disabled valid contact type cellphone")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeCellPhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhoneDisabled();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Single(result.Errors);

            Assert.Contains(ContactValidation.ContactNotActive, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate disabled valid contact type email")]
        [Trait("Categoria", "Cadastro - Contato")]
        public void Contact_NewContactTypeEmail_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmailDisabled();

            //Act
            var result = contact.ValidateContact();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Single(result.Errors);

            Assert.Contains(ContactValidation.ContactNotActive, result.Errors.Select(e => e.ErrorMessage));
        }

        #endregion
    }
}