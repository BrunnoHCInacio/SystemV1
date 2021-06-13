using System;
using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Provider : People
    {
        public Provider(Guid id, string name)
        {
            Id = id;
            Name = name;
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
    }
}