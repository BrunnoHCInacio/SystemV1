using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceState
    {
        Task AddAsync(StateViewModel stateViewModel);

        Task UpdateAsync(StateViewModel stateViewModel);

        Task RemoveAsync(StateViewModel stateViewModel);

        Task<IEnumerable<StateViewModel>> GetAllAsync(int page, int pageSize);

        Task<IEnumerable<StateViewModel>> GetByNameAsync(string name);

        Task<StateViewModel> GetByIdAsync(Guid id);

        Task<StateViewModel> GetStateCountryByIdAsync(Guid id);
    }
}