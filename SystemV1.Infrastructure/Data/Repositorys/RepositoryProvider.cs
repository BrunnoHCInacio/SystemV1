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
    public class RepositoryProvider : Repository<Provider>, IRepositoryProvider
    {
        private readonly SqlContext _sqlContext;

        public RepositoryProvider(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Provider>> GetAllProvidersAsync(int page, int pageSize)
        {
            return await _sqlContext.Providers.Where(p => p.IsActive)
                                              .Skip(GetSkip(page, pageSize))
                                              .Take(pageSize)
                                              .ToListAsync();
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

        public async Task<Provider> GetProviderByIdAsync(System.Guid id)
        {
            var query = from provider in _sqlContext.Providers
                        where provider.IsActive && provider.Id == id
                        select new Provider(provider.Id,
                                            provider.Name,
                                            provider.Document,
                                            _sqlContext.Address.Where(a => a.IsActive && a.ProviderId == provider.Id).ToList(),
                                            _sqlContext.Contact.Where(a => a.IsActive && a.ProviderId == provider.Id).ToList());
            return await query.FirstOrDefaultAsync() ;
        }
    }
}