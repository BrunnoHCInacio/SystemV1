using System;

namespace SystemV1.Domain.Entitys
{
    public class Address : Entity
    {
        public Address(int zipCode,
                       string street,
                       string number,
                       string complement,
                       string district,
                       State state,
                       Guid id = new Guid(),
                       Client client = null,
                       Provider provider = null)
        {
            Id = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            State = state;
            Client = client;
            Provider = provider;
        }

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