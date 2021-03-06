using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperContact : IMapperContact
    {
        public ContactViewModel EntityToViewModel(Contact contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                CellPhoneNumber = contact.CellPhoneNumber,
                Ddd = contact.Ddd,
                Ddi = contact.Ddi,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                TypeContact = contact.TypeContact
            };
        }

        public IEnumerable<ContactViewModel> ListEntityToViewModel(IEnumerable<Contact> contacts)
        {
            return contacts.Select(c => EntityToViewModel(c));
        }

        public Contact ViewModelToEntity(ContactViewModel contactViewModel)
        {
            var contact = new Contact(contactViewModel.Id,
                                      contactViewModel.TypeContact,
                                      contactViewModel.Ddd,
                                      contactViewModel.Ddi,
                                      contactViewModel.CellPhoneNumber,
                                      contactViewModel.PhoneNumber,
                                      contactViewModel.Email);

            if (contactViewModel.IdProvider.HasValue)
            {
                // contact.Provider = new Provider { Id = contactViewModel.IdProvider.GetValueOrDefault() };
            }
            if (contactViewModel.IdClient.HasValue)
            {
                // contact.Client = new Client(contactViewModel.IdClient.GetValueOrDefault(), null, null);
            }

            return contact;
        }
    }
}