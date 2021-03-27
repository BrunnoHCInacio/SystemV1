using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperClient
    {
        ClientViewModel EntityToViewModel(Client client);

        Client ViewModelToEntity(ClientViewModel clientViewModel);

        IEnumerable<ClientViewModel> ListEntityToViewModel(IEnumerable<Client> clientes);
    }
}