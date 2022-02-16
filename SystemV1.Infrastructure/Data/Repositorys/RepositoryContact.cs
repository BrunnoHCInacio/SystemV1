using Dapper;
using System;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryContact : Repository<Contact>, IRepositoryContact
    {
        private readonly SqlContext _sqlContext;

        public RepositoryContact(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public void RemoveAllByClientId(Guid clientId)
        {
            var sql = $@"update {"\""}Contact{"\""} 
                         set {"\""}IsActive{"\""} = false 
                         where {"\""}ClientId{"\""} = '{clientId}' and {"\""}ClientId{"\""}";

            _sqlContext.Connection.Execute(sql);
        }
    }
}