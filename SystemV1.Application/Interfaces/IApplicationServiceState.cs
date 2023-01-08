using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceState : IApplicationService<StateViewModel>
    {
        Task<StateViewModel> GetStateCountryByIdAsync(Guid id);

        Task<List<StateViewModel>> GetByName(string name);
    }
}