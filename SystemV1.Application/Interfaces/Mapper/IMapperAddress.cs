using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperAddress
    {
        AddressViewModel EntityToViewModel(Address address);

        Address ViewModelToEntity(AddressViewModel addressViewModel);

        IEnumerable<AddressViewModel> ListEntityToViewModel(IEnumerable<Address> addresses);
    }
}