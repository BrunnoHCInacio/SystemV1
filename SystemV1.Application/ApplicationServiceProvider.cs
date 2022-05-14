using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        private readonly IProviderService _providerService;
        private readonly IMapperProvider _mapperProvider;

        public ApplicationServiceProvider(IProviderService providerService,
                                          IMapperProvider mapperProvider)
        {
            _providerService = providerService;
            _mapperProvider = mapperProvider;
        }

        public async Task AddAsync(ProviderViewModel providerViewModel)
        {
            var provider = _mapperProvider.ViewModelToEntity(providerViewModel);
            await _providerService.AddAsyncUow(provider);
        }

        public async Task<bool> ExistsProvider(Guid id)
        {
            return await _providerService.ExistsProvider(id);
        }

        public async Task<IEnumerable<ProviderViewModel>> GetAllAsync(int page, int pageSize)
        {
            var providers = await _providerService.GetAllAsync(page, pageSize);
            return _mapperProvider.ListEntityToViewModel(providers);
        }

        public async Task<ProviderViewModel> GetByIdAsync(Guid id)
        {
            var provider = await _providerService.GetByIdAsync(id);
            return _mapperProvider.EntityToViewModel(provider);
        }

        public async Task<IEnumerable<ProviderViewModel>> GetByNameAsync(string name)
        {
            var providers = await _providerService.GetByNameAsync(name);
            return _mapperProvider.ListEntityToViewModel(providers);
        }

        public async Task RemoveAsync(Guid id)
        {
            var provider = await _providerService.GetByIdAsync(id);
            await _providerService.RemoveAsyncUow(provider);
        }

        public async Task UpdateAsync(ProviderViewModel providerViewModel)
        {
            var provider = _mapperProvider.ViewModelToEntity(providerViewModel);
            await _providerService.UpdateAsyncUow(provider);
        }
    }
}