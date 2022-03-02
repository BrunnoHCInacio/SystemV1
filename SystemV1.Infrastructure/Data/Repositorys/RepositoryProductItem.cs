using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryProductItem : Repository<ProductItem>, IRepositoryProductItem
    {
        private readonly SqlContext _sqlContext;

        public RepositoryProductItem(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<ProductItem>> GetAllProductItemsAsync(int page, int pageSize)
        {
            return await _sqlContext.ProductItem.Where(pi => pi.IsActive)
                                                .Skip(GetSkip(page, pageSize))
                                                .Take(pageSize)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<ProductItem>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT
                            modelo,
                            value,
                            issold
                            isavailable
                        FROM productitem
                        WHERE modelo LIKE '%{name}%' and isactive";

            return await _sqlContext.Connection.QueryAsync<ProductItem>(sql);
        }

        public async Task<ProductItem> GetProductItemByIdAsync(System.Guid id)
        {
            return await _sqlContext.ProductItem.SingleAsync(pi => pi.IsActive && pi.Id == id);
        }
    }
}