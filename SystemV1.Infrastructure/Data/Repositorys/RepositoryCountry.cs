using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryCountry : Repository<Country>, IRepositoryCountry
    {
        private readonly SqlContext _sqlContext;

        public RepositoryCountry(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }
        
        public async Task<IEnumerable<Country>> GetByNameAsync(string name)
        {
            return await _sqlContext.Country.Where(c => c.Name.ToUpper().Contains(name.ToUpper())).ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var result = (from country in _sqlContext.Country
                          where country.IsActive
                          select new Country(country.Id,
                                             country.Name,
                                             country.DateRegister,
                                             country.DateChange.GetValueOrDefault(),
                                             country.IdUserRegister,
                                             country.IdUserChange,
                                             country.IsActive,
                                             _sqlContext.State.Where(s => s.CountryId == country.Id && s.IsActive).ToList()));

            var test = await result.Skip(skip).Take(pageSize).ToListAsync();
            return test;
        }

        public async Task<Country> GetCountryByIdAsync(Guid id)
        {
            var query = (from country in _sqlContext.Country
                         where country.Id == id
                         && country.IsActive
                         select new Country(country.Id,
                                            country.Name,
                                            country.DateRegister,
                                            country.DateChange.GetValueOrDefault(),
                                            country.IdUserRegister,
                                            country.IdUserChange,
                                            country.IsActive,
                                            _sqlContext.State.Where(s => s.CountryId == country.Id && s.IsActive).ToList()));
            return await query.FirstOrDefaultAsync();
        }
    }
}