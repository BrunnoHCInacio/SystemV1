using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceCity
    {
        Task AddAsync(CityViewModel cityViewModel);
        Task UpdateAsync(CityViewModel cityViewModel);
        Task DeleteAsync(CityViewModel cityViewModel);
        Task<IEnumerable<CityViewModel>> GetAllAsync(int page, int pageSize);
        Task<CityViewModel> GetByIdAsync(Guid id);
    }
}
