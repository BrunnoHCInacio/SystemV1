using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Core.Constants;
using SystemV1.Domain.Services.Test.Enums;
using SystemV1.Domain.Services.Validations;
using Xunit;

namespace SystemV1.Domain.Services.Test
{
    public class ContactServiceTest
    {
        [Fact]
        public void Create()
        {
            var contactValidation = new ContactValidation();

            TestValidate(Case.Success);
            TestAttributes(Constants.TypeContactEmail,
                           "",
                           "",
                           "",
                           "",
                           "brunno",
                           new List<string> { ConstantsContactMessages.EmailValid });

            TestAttributes(Constants.TypeContactPhone,
                           "",
                           "",
                           "",
                           "",
                           "",
                           new List<string>
                           {
                               ConstantsContactMessages.PhoneNumberRequired,
                               ConstantsContactMessages.PhoneNumberMinLength,
                               ConstantsContactMessages.PhoneNumberMaxLength,
                               ConstantsContactMessages.DddRequired,
                               ConstantsContactMessages.DddMinLength,
                           });

            TestAttributes(Constants.TypeContactCellPhone,
                           "",
                           "",
                           "",
                           "",
                           "",
                           new List<string>
                           {
                              ConstantsContactMessages.DddRequired,
                              ConstantsContactMessages.DddMinLength,
                              ConstantsContactMessages.DddMaxLength,
                              ConstantsContactMessages.CellPhoneRequired,
                              ConstantsContactMessages.CellPhoneMinLength,
                              ConstantsContactMessages.CellPhoneMaxLength
                           });
        }

        private void TestValidate(Case caseValidate)
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

            GenericValidationTest.RunValidation(new ContactValidation(), phone, caseValidate);
            GenericValidationTest.RunValidation(new ContactValidation(), email, caseValidate);
            GenericValidationTest.RunValidation(new ContactValidation(), cellPhone, caseValidate);
        }

        private void TestAttributes(string typeContact,
                                   string ddd,
                                   string ddi,
                                   string cellPhone,
                                   string phoneNumber,
                                   string email,
                                   List<string> messagesExpected)
        {
            var contactExpected = new Contact(typeContact,
                                                   ddd,
                                                   ddi,
                                                   cellPhone,
                                                   phoneNumber,
                                                   email,
                                                   Guid.NewGuid());

            GenericValidationTest.RunValidation(new ContactValidation(), contactExpected, null, messagesExpected);
        }
    }
}