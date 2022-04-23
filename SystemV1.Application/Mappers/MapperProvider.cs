using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(provider == null)
            {
                return null;
            }

            return new ProviderViewModel
            {
                Id = provider.Id,
                Document = provider.Document,
                Contacts = _mapperContact.ListEntityToViewModel(provider.Contacts),
                Addresses = _mapperAddress.ListEntityToViewModel(provider.Addresses),
                Name = provider.Name
            };
        }

        public IEnumerable<ProviderViewModel> ListEntityToViewModel(IEnumerable<Provider> providers)
        {
            return providers.Select(p => EntityToViewModel(p));
        }

        public Provider ViewModelToEntity(ProviderViewModel providerViewModel)
        {
            var provider = new Provider(providerViewModel.Id,
                                        providerViewModel.Name,
                                        providerViewModel.Document);
            if (providerViewModel.Addresses != null
               && providerViewModel.Addresses.Any())
            {
                provider.AddAddresses(providerViewModel.Addresses.Select(a => _mapperAddress.ViewModelToEntity(a)).ToList());
            }

            if (providerViewModel.Contacts!= null
               && providerViewModel.Contacts.Any())
            {
                provider.AddContacts(providerViewModel.Contacts.Select(c => _mapperContact.ViewModelToEntity(c)).ToList());
            }

            return provider;
        }
    }
}