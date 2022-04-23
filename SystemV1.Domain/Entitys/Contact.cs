using FluentValidation.Results;
using System;
using SystemV1.Domain.Enums;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Contact : Entity
    {
        public Contact(Guid id,
                       EnumTypeContact typeContact,
                       string ddd = "",
                       string ddi = "",
                       string cellPhoneNumber = "",
                       string phoneNumber = "",
                       string email = "")
        {
            Id = id;
            TypeContact = typeContact;
            Ddd = ddd;
            Ddi = ddi;
            CellPhoneNumber = cellPhoneNumber;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Client Client { get; private set; }
        public Guid ClientId { get; set; }
        public Provider Provider { get; private set; }

        public Guid ProviderId { get; set; }

        public EnumTypeContact TypeContact { get; private set; }
        public string Ddd { get; private set; }
        public string Ddi { get; private set; }
        public string CellPhoneNumber { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public ValidationResult ValidateContact()
        {
            return new ContactValidation().Validate(this);
        }

        public void SetClient(Guid clientId)
        {
            ClientId = clientId;
        }

        public void SetProvider(Guid providerId)
        {
            ProviderId = providerId;
        }
    }
}