using Xunit;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Entitys;
using System;

namespace SystemV1.Domain.Test
{
    public class ContactDomainTest
    {
        [Fact]
        public void Create()
        {
            var contactPhoneExpected = new
            {
                Id = Guid.NewGuid(),
                TypeContact = Constants.TypeContactPhone,
                Ddd = "64",
                Ddi = "+55",
                CellPhoneNumber = "",
                PhoneNumber = "34133191",
                Email = ""
            };
            var contactEmailExpected = new
            {
                Id = Guid.NewGuid(),
                TypeContact = Constants.TypeContactEmail,
                Ddd = "",
                Ddi = "",
                CellPhoneNumber = "",
                PhoneNumber = "",
                Email = "brunnoh11@gmail.com"
            };
            var contactCellPhoneExpected = new
            {
                Id = Guid.NewGuid(),
                TypeContact = Constants.TypeContactCellPhone,
                Ddd = "64",
                Ddi = "+55",
                CellPhoneNumber = "99999999",
                PhoneNumber = "",
                Email = ""
            };

            var phone = new Contact(contactPhoneExpected.TypeContact,
                                    contactPhoneExpected.Ddd,
                                    contactPhoneExpected.Ddi,
                                    contactPhoneExpected.CellPhoneNumber,
                                    contactPhoneExpected.PhoneNumber,
                                    contactPhoneExpected.Email,
                                    contactPhoneExpected.Id);

            var email = new Contact(contactEmailExpected.TypeContact,
                                    contactEmailExpected.Ddd,
                                    contactEmailExpected.Ddi,
                                    contactEmailExpected.CellPhoneNumber,
                                    contactEmailExpected.PhoneNumber,
                                    contactEmailExpected.Email,
                                    contactEmailExpected.Id);

            var cellPhone = new Contact(contactCellPhoneExpected.TypeContact,
                                    contactCellPhoneExpected.Ddd,
                                    contactCellPhoneExpected.Ddi,
                                    contactCellPhoneExpected.CellPhoneNumber,
                                    contactCellPhoneExpected.PhoneNumber,
                                    contactCellPhoneExpected.Email,
                                    contactCellPhoneExpected.Id);

            Assert.Equal(contactPhoneExpected.Id, phone.Id);
            Assert.Equal(contactPhoneExpected.TypeContact, phone.TypeContact);
            Assert.Equal(contactPhoneExpected.Ddd, phone.Ddd);
            Assert.Equal(contactPhoneExpected.Ddi, phone.Ddi);
            Assert.Equal(contactPhoneExpected.CellPhoneNumber, phone.CellPhoneNumber);
            Assert.Equal(contactPhoneExpected.PhoneNumber, phone.PhoneNumber);
            Assert.Equal(contactPhoneExpected.Email, phone.Email);

            Assert.Equal(contactEmailExpected.Id, email.Id);
            Assert.Equal(contactEmailExpected.TypeContact, email.TypeContact);
            Assert.Equal(contactEmailExpected.Ddd, email.Ddd);
            Assert.Equal(contactEmailExpected.Ddi, email.Ddi);
            Assert.Equal(contactEmailExpected.CellPhoneNumber, email.CellPhoneNumber);
            Assert.Equal(contactEmailExpected.PhoneNumber, email.PhoneNumber);
            Assert.Equal(contactEmailExpected.Email, email.Email);

            Assert.Equal(contactCellPhoneExpected.Id, cellPhone.Id);
            Assert.Equal(contactCellPhoneExpected.TypeContact, cellPhone.TypeContact);
            Assert.Equal(contactCellPhoneExpected.Ddd, cellPhone.Ddd);
            Assert.Equal(contactCellPhoneExpected.Ddi, cellPhone.Ddi);
            Assert.Equal(contactCellPhoneExpected.CellPhoneNumber, cellPhone.CellPhoneNumber);
            Assert.Equal(contactCellPhoneExpected.PhoneNumber, cellPhone.PhoneNumber);
            Assert.Equal(contactCellPhoneExpected.Email, cellPhone.Email);
        }
    }
}