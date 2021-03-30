using System;

namespace SystemV1.Domain.Entitys
{
    public class Contact : Entity
    {
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