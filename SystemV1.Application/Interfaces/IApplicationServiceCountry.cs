using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceCountry
    {
        Task<IEnumerable<CountryViewModel>> GetAllAsync(int page, int pageSize);

        Task<CountryViewModel> GetByIdAsync(Guid id);

        Task<IEnumerable<CountryViewModel>> GetByNameAsync(string name);

        Task AddAsync(CountryViewModel countryViewModel);

        Task UpdateAsync(CountryViewModel countryViewModel);

        Task RemoveAsync(Guid id);

        Task<bool> ExistsCountry(Guid id);
    }
}