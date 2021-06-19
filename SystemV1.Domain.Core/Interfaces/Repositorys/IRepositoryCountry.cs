using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryCountry : IRepository<Country>
    {
        Task<IEnumerable<Country>> GetByNameAsync(string name);

        Task<IEnumerable<Country>> GetAllCountriesAsync(int page, int pageSize);

        Task<Country> GetCountryByIdAsync(Guid id);
    }
}