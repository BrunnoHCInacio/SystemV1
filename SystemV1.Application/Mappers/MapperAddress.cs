using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                ZipCode = address.ZipCode,
                
            };
        }

        public IEnumerable<AddressViewModel> ListEntityToViewModel(IEnumerable<Address> addresses)
        {
            return addresses.Select(a => EntityToViewModel(a));
        }

        public Address ViewModelToEntity(AddressViewModel addressViewModel)
        {
            var address = new Address(addressViewModel.Id,
                                      addressViewModel.ZipCode,
                                      addressViewModel.Street,
                                      addressViewModel.Number,
                                      addressViewModel.Complement,
                                      addressViewModel.District,
                                      null);
            
            if (addressViewModel.IdCLient.HasValue)
            {
                address.SetClient(new Client(addressViewModel.IdCLient.GetValueOrDefault(), null, null));
            }

            if (addressViewModel.IdProvider.HasValue)
            {
                address.SetProvider(new Provider(addressViewModel.IdProvider.GetValueOrDefault(), null, null));
            }

            return address;
        }
    }
}