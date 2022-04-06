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
    public class RepositoryState : Repository<State>, IRepositoryState
    {
        private readonly SqlContext _sqlContext;

        public RepositoryState(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<State>> GetByNameAsync(string name)
        {
            return await _sqlContext.State
                                    .Where(s => s.Name.ToUpper().Contains(name.ToUpper()))
                                    .OrderBy(s => s.Name)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var query = (from state in _sqlContext.State
                         join country in _sqlContext.Country
                         on state.CountryId equals country.Id
                         where state.IsActive
                         select new State(state.Id, state.Name, new Country(country.Id, country.Name)));

            return await query.Skip(skip)
                              .Take(pageSize)
                              .OrderBy(s => s.Name)
                              .ToListAsync();
        }

        public async Task<State> GetStateByIdAsync(Guid id)
        {
            var query = (from state in _sqlContext.State
                         join country in _sqlContext.Country
                         on state.CountryId equals country.Id
                         where state.IsActive
                               && state.Id == id
                         select new State(state.Id,
                                          state.Name,
                                          state.DateRegister,
                                          state.DateChange.GetValueOrDefault(),
                                          state.IdUserRegister,
                                          state.IdUserChange,
                                          new Country(country.Id, country.Name),
                                          state.IsActive));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<State> GetStateCountryByIdAsync(Guid id)
        {
            var query = (from state in _sqlContext.State
                         join country in _sqlContext.Country 
                         on state.CountryId equals country.Id
                         where state.IsActive
                               && country.IsActive
                               && state.Id == id
                         select new State(state.Id,
                                          state.Name,
                                          state.DateRegister,
                                          state.DateChange.GetValueOrDefault(),
                                          state.IdUserRegister,
                                          state.IdUserChange,
                                          country,
                                          state.IsActive));

            return await query.FirstOrDefaultAsync();
        }
    }
}