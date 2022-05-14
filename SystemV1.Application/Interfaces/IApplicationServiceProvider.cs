using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceProvider
    {
        Task AddAsync(ProviderViewModel providerViewModel);

        Task UpdateAsync(ProviderViewModel providerViewModel);

        Task RemoveAsync(Guid id);

        Task<IEnumerable<ProviderViewModel>> GetAllAsync(int page, int pageSize);

        Task<ProviderViewModel> GetByIdAsync(Guid id);

        Task<IEnumerable<ProviderViewModel>> GetByNameAsync(string name);
        Task<bool> ExistsProvider(Guid id);
    }
}