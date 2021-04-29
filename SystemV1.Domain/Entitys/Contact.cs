using System;

namespace SystemV1.Domain.Entitys
{
    public class Contact : Entity
    {
        public Contact(string typeContact,
                       string ddd,
                       string ddi,
                       string cellPhoneNumber,
                       string phoneNumber,
                       string email,
                       Guid id = new Guid(),
                       Client client = null,
                       Provider provider = null)
        {
            Id = id;
            TypeContact = typeContact;
            Ddd = ddd;
            Ddi = ddi;
            CellPhoneNumber = cellPhoneNumber;
            PhoneNumber = phoneNumber;
            Email = email;
            Client = client;
            Provider = provider;
        }

        public Client Client { get; set; }
        public Guid ClientId { get; set; }
        public Provider Provider { get; set; }

        public Guid ProviderId { get; set; }

        public string TypeContact { get; set; }
        public string Ddd { get; set; }
        public string Ddi { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}