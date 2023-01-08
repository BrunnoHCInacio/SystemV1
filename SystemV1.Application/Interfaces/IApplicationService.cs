using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationService<TViewModel>
    {
        Task AddAsync(TViewModel TViewModel);

        Task UpdateAsync(TViewModel TViewModel);

        Task RemoveAsync(Guid Id);

        Task<List<TViewModel>> GetAllAsync(int page, int pageSize);

        Task<TViewModel> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);
    }
}