using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperContact
    {
        ContactViewModel EntityToViewModel(Contact contact);

        Contact ViewModelToEntity(ContactViewModel contactViewModel);

        IEnumerable<ContactViewModel> ListEntityToViewModel(IEnumerable<Contact> contacts);
    }
}