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
    public class RepositoryCity : Repository<City>, IRepositoryCity
    {
        private readonly SqlContext _sqlContext;
        public RepositoryCity(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(int page, int pageSize)
        {
            return await _sqlContext.Cities.Where(c => c.IsActive).Skip(GetSkip(page, pageSize)).Take(pageSize).ToListAsync();
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            var query = from city in _sqlContext.Cities
                        join state in _sqlContext.State
                            on city.StateId equals state.Id
                        where city.Id == id 
                        && city.IsActive 
                        && state.IsActive
                        select new City(city.Id, city.Name, new State(state.Id, state.Name));
            
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<City>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
