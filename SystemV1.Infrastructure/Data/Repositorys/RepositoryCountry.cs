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
    }
}