using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryPeopleAddress : Repository<PeopleAddress>, IRepositoryPeopleAddress
    {
        private readonly SqlContext _sqlContext;

        public RepositoryPeopleAddress(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }
    }
}