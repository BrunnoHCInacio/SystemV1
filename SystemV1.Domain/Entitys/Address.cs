using System;

namespace SystemV1.Domain.Entitys
{
    public class Address : Entity
    {
        public Client Client { get; set; }

        public Guid ClientId { get; set; }
        public Provider Provider { get; set; }
        public Guid ProviderId { get; set; }

        public int ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
    }
}