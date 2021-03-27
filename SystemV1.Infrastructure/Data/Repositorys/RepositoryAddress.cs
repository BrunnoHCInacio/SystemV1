using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryAddress : Repository<Address>, IRepositoryAddress
    {
        private readonly SqlContext _sqlContext;

        public RepositoryAddress(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }
    }
}