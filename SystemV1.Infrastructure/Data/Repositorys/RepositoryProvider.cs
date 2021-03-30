using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryProvider : Repository<Provider>, IRepositoryProvider
    {
        private readonly SqlContext _sqlContext;

        public RepositoryProvider(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Provider>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT  id,
                                name,
                                document
                        FROM provider
                        WHERE name LIKE '%{name}%' and isactive
                        ";
            return await _sqlContext.Connection.QueryAsync<Provider>(sql);
        }
    }
}