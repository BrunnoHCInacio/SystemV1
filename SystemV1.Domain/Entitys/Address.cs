using System;

namespace SystemV1.Domain.Entitys
{
    public class Address : Entity
    {
        public Address()
        {
        }

        public Address(Guid id,
                       string zipCode,
                       string street,
                       string number,
                       string complement,
                       string district,
                       Guid cityId,
                       Guid peopleId)
        {
            Id = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            CityId = cityId;
            PeopleId = peopleId;
        }

        public People People { get; private set; }
        public Guid PeopleId { get; private set; }

        public string ZipCode { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public City City { get; private set; }
        public Guid CityId { get; private set; }
    }
}