using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryProduct : Repository<Product>, IRepositoryProduct
    {
        private readonly SqlContext _sqlContext;

        public RepositoryProduct(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT *
                        FROM {"\""}Product{"\""}
                        WHERE Name LIKE {name}
                            AND {"\""}IsActive{"\""}
                        ";
            return await _sqlContext.Connection.QueryAsync<Product>(sql);
        }
    }
}