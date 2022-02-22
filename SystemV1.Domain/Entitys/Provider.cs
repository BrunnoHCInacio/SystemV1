using FluentValidation.Results;
using System;
using System.Collections.Generic;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Provider : People
    {
        public Provider(Guid id, string name, string document)
        {
            Id = id;
            Name = name;
            Document = document;
            Addresses = new List<Address>();
            Contacts = new List<Contact>();
        }

        public List<Address> Addresses { get; private set; }
        public List<Contact> Contacts { get; private set; }

        public void AddAddresses(List<Address> addresses)
        {
            Addresses.AddRange(addresses);
        }

        public void AddAddress(Address address)
        {
            Addresses.Add(address);
        }

        public void AddContacts(List<Contact> contacts)
        {
            Contacts.AddRange(contacts);
        }

        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
        }

        public ValidationResult ValidateProvider()
        {
            return new ProviderValidation().Validate(this);
        }
    }
}