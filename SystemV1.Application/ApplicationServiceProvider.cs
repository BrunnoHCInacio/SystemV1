using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        private readonly IProviderService _providerService;

        public ApplicationServiceProvider(IProviderService providerService)
        {
            _providerService = providerService;
        }

        public async Task Add(ProviderViewModel providerViewModel)
        {
        }

        public async Task<IEnumerable<ProviderViewModel>> GetAll(int page, int pageSize)
        {
            //return await _providerService.GetAll(page, pageSize);
        }

        public async Task<ProviderViewModel> GetById(Guid id)
        {
        }

        public async Task<IEnumerable<ProductItemViewModel>> GetByName(string name)
        {
        }

        public async Task Remove(Guid id)
        {
        }

        public async Task Update(ProviderViewModel providerViewModel)
        {
        }
    }
}