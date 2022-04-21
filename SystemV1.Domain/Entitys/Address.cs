﻿using FluentValidation.Results;
using System;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Address : Entity
    {
        public Address(Guid id,
                      string zipCode,
                      string street,
                      string number,
                      string complement,
                      string district)
        {
            Id = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
        }
        public Address(Guid id,
                       string zipCode,
                       string street,
                       string number,
                       string complement,
                       string district,
                       City city)
        {
            Id = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            City = city;
        }

        public Client Client { get; private set; }
        public Guid ClientId { get; private set; }
        public Provider Provider { get; private set; }
        public Guid ProviderId { get; private set; }

        public string ZipCode { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public City City { get; private set; }
        public Guid CityId { get; private set; }
        

        public ValidationResult ValidateAddress()
        {
            return new AddressValidation().Validate(this);
        }

        public void SetProvider(Guid providerId)
        {
            ProviderId = providerId;
        }

        public void SetClient(Guid clientId)
        {
            ClientId = ClientId;
        }

        public void SetCity(Guid cityId)
        {
            CityId = cityId;
        }
    }
}