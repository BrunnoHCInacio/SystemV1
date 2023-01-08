using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceCity : IApplicationService<CityViewModel>
    {
        Task<List<CityViewModel>> GetByNameAsync(string name);

        Task<CityViewModel> GetCityStateByIdAsync(Guid id);
    }
}