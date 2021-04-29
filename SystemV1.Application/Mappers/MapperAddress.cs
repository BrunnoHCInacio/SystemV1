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
                IdCountry = address.Country?.Id ?? null,
                CountryName = address.Country?.Name ?? "",
                IdState = address.State?.Id ?? null,
                StateName = address.State?.Name ?? ""
            };
        }

        public IEnumerable<AddressViewModel> ListEntityToViewModel(IEnumerable<Address> addresses)
        {
            return addresses.Select(a => EntityToViewModel(a));
        }

        public Address ViewModelToEntity(AddressViewModel addressViewModel)
        {
            var state = new State("", addressViewModel.IdState.GetValueOrDefault());

            var address = new Address(addressViewModel.ZipCode,
                                      addressViewModel.Street,
                                      addressViewModel.Number,
                                      addressViewModel.Complement,
                                      addressViewModel.District,
                                      state);

            if (addressViewModel.IdCLient.HasValue)
            {
                address.Client = new Client { Id = addressViewModel.IdCLient.GetValueOrDefault() };
            }

            if (addressViewModel.IdProvider.HasValue)
            {
                address.Provider = new Provider { Id = addressViewModel.IdProvider.GetValueOrDefault() };
            }

            return address;
        }
    }
}