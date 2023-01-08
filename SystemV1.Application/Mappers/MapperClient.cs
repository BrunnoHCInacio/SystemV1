using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperClient : IMapperClient
    {
        public MapperClient()
        { }

        public ClientViewModel EntityToViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                PeopleId = client.Id
            };
        }

        public IEnumerable<ClientViewModel> ListEntityToViewModel(IEnumerable<Client> clientes)
        {
            return clientes.Select(c => EntityToViewModel(c));
        }

        public Client ViewModelToEntity(ClientViewModel clientViewModel)
        {
            return new Client(clientViewModel.Id,
                              clientViewModel.PeopleId);
        }
    }
}