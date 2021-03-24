using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    internal interface IApplicationServiceProvider
    {
        Task Add(ProviderViewModel providerViewModel);

        Task Update(ProviderViewModel providerViewModel);

        Task Remove(Guid id);

        Task<IEnumerable<ProviderViewModel>> GetAll(int page, int pageSize);

        Task<ProviderViewModel> GetById(Guid id);

        Task<IEnumerable<ProductItemViewModel>> GetByName(string name);
    }
}