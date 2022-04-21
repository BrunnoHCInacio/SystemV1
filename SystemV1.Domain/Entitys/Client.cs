using FluentValidation.Results;
using System;
using System.Collections.Generic;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Client : People
    {
        public Client(Guid id,
                      string name,
                      string document)
        {
            Id = id;
            Name = name;
            Document = SetDocumentMask(document);
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

        public ValidationResult ValidateClient()
        {
            return new ClientValidation().Validate(this);
        }

        private string SetDocumentMask(string documentNumber)
        {
            if(documentNumber == null)
            {
                return string.Empty;
            }

            if (!documentNumber.Contains(".")
               && !documentNumber.Contains("-")
               && documentNumber.Length == 11)
            {
                return Convert.ToUInt64(documentNumber).ToString(@"000\.000\.000\-00");
            }

            if (!documentNumber.Contains(".")
               && !documentNumber.Contains("-")
               && !documentNumber.Contains("/")
               && documentNumber.Length == 14)
            {
                return Convert.ToUInt64(documentNumber).ToString(@"00\.000\.000\/0000\-00"); 
            }

            return documentNumber;
        }
    }
}