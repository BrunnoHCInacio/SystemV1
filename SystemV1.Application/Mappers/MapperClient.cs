using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperClient : IMapperClient
    {
        private readonly IMapperAddress _mapperAddress;
        private readonly IMapperContact _mapperContact;

        public MapperClient(IMapperAddress mapperAddress, IMapperContact mapperContact)
        {
            _mapperAddress = mapperAddress;
            _mapperContact = mapperContact;
        }

        public ClientViewModel EntityToViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                Name = client.Name,
                Document = client.Document,
                Addresses = client.Addresses.Select(a => _mapperAddress.EntityToViewModel(a)),
                Contacts = client.Contacts.Select(c => _mapperContact.EntityToViewModel(c))
            };
        }

        public IEnumerable<ClientViewModel> ListEntityToViewModel(IEnumerable<Client> clientes)
        {
            return clientes.Select(c => EntityToViewModel(c));
        }

        public Client ViewModelToEntity(ClientViewModel clientViewModel)
        {
            var client = new Client(clientViewModel.Id,
                                    clientViewModel.Name,
                                    clientViewModel.Document);

            client.AddAddresses(clientViewModel.Addresses.Select(a => _mapperAddress.ViewModelToEntity(a)).ToList());
            client.AddContacts(clientViewModel.Contacts.Select(c => _mapperContact.ViewModelToEntity(c)).ToList());

            return client;
        }
    }
}