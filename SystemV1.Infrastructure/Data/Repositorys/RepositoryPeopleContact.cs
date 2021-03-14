using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryPeopleContact : Repository<PeopleContact>, IRepositoryPeopleContact
    {
        private readonly SqlContext _sqlContext;

        public RepositoryPeopleContact(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }
    }
}