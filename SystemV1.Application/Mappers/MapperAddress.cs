using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperAddress : IMapperAddress
    {
        public AddressViewModel EntityToViewModel(Address address)
        {
            return new AddressViewModel
            {
                Id = address.Id,
                Street = address.Street,
                Complement = address.Complement,
                Number = address.Number,
                District = address.District,
                ZipCode = address.ZipCode,
                CityId = address.City.Id,
                CityName = address.City.Name
            };
        }

        public IEnumerable<AddressViewModel> ListEntityToViewModel(IEnumerable<Address> addresses)
        {
            return addresses.Select(a => EntityToViewModel(a));
        }

        public Address ViewModelToEntity(AddressViewModel addressViewModel,
                                         Guid peopleId)
        {
            return new Address(addressViewModel.Id,
                               addressViewModel.ZipCode,
                               addressViewModel.Street,
                               addressViewModel.Number,
                               addressViewModel.Complement,
                               addressViewModel.District,
                               addressViewModel.CityId,
                               peopleId);
        }
    }
}