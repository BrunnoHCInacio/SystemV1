using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperProvider : IMapperProvider
    {
        private readonly IMapperAddress _mapperAddress;
        private readonly IMapperContact _mapperContact;

        public MapperProvider(IMapperAddress mapperAddress,
                              IMapperContact mapperContact)
        {
            _mapperAddress = mapperAddress;
            _mapperContact = mapperContact;
        }

        public ProviderViewModel EntityToViewModel(Provider provider)
        {
            return new ProviderViewModel
            {
                Id = provider.Id,
                Document = provider.Document,
                Contacts = _mapperContact.ListEntityToViewModel(provider.Contacts),
                Addresses = _mapperAddress.ListEntityToViewModel(provider.Addresses)
            };
        }

        public IEnumerable<ProviderViewModel> ListEntityToViewModel(IEnumerable<Provider> providers)
        {
            return providers.Select(p => EntityToViewModel(p));
        }

        public Provider ViewModelToEntity(ProviderViewModel providerViewModel)
        {
            return new Provider()
            {
                Id = providerViewModel.Id,
                Document = providerViewModel.Document,
                Name = providerViewModel.Name
            };
        }
    }
}