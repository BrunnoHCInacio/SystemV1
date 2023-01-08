using System;
using SystemV1.Domain.Enums;

namespace SystemV1.Domain.Entitys
{
    public class Contact : Entity
    {
        public Contact()
        { }

        public Contact(Guid id,
                       EnumTypeContact typeContact,
                       People people,
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
            People = people;
            PeopleId = people?.Id ?? Guid.Empty;
        }

        public People People { get; private set; }
        public Guid PeopleId { get; private set; }

        public EnumTypeContact TypeContact { get; private set; }
        public string Ddd { get; private set; }
        public string Ddi { get; private set; }
        public string CellPhoneNumber { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
    }
}