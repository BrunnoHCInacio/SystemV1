using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceCountry : IApplicationService<CountryViewModel>
    {
        Task<List<CountryViewModel>> GetByNameAsync(string name);
    }
}