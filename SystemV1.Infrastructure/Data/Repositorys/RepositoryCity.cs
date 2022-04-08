using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryCity : Repository<City>, IRepositoryCity
    {
        public RepositoryCity(SqlContext sqlContext) : base(sqlContext)
        {
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
