using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryPeople : Repository<People>, IRepositoryPeople
    {
        public RepositoryPeople(SqlContext sqlContext) : base(sqlContext)
        {
        }
    }
}