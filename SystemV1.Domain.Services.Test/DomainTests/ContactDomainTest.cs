using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
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
        [Trait("UnitTests - Entity", "Contact")]
        public void Contact_NewContact_ShouldSetCorrectPropertiesForPhoneWithSuccess()
        {
            //Arrange
            var contactPhoneExpected = _contactTestFixture.GenerateValidContactExpectedTypePhone();

            //Act
            var phone = new Contact(contactPhoneExpected.Id,
                                    contactPhoneExpected.TypeContact,
                                    null,
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

        [Fact(DisplayName = "Validate set as correct properties for cellphone")]
        [Trait("UnitTests - Entity", "Contact")]
        public void Contact_NewContact_ShouldSetCorrectPropertiresForCellPhoneWithSuccess()
        {
            //Arrange
            var contactCellPhoneExpected = _contactTestFixture.GenerateValidContactExpectedTypeCellPhone();

            //Act
            var cellPhone = new Contact(contactCellPhoneExpected.Id,
                                       contactCellPhoneExpected.TypeContact,
                                       null,
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

        [Fact(DisplayName = "Validade set as correct proprerties for email")]
        [Trait("UnitTests - Entity", "Contact")]
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
                                   null,
                                   contactEmailExpected.Email);

            //Assert
            Assert.Equal(contactEmailExpected.Id, email.Id);
            Assert.Equal(contactEmailExpected.TypeContact, email.TypeContact);
            Assert.Equal(contactEmailExpected.Email, email.Email);
        }

        #endregion Test validate for each property from entity
    }
}