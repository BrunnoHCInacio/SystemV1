using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryClient : Repository<Client>, IRepositoryClient
    {
        private readonly SqlContext _sqlContext;

        public RepositoryClient(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> ExistClient(Guid id)
        {
            return await _sqlContext.Client.Where(c => c.IsActive && c.Id == id).AnyAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(int page, int pageSize)
        {
            return await _sqlContext.Client.Where(c => c.IsActive)
                                           .Skip(GetSkip(page, page))
                                           .Take(pageSize)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT
                            {"\""}Id{"\""},
                            {"\""}Name{"\""},
                            {"\""}Document{"\""}
                        FROM {"\""}client{"\""}
                        WHERE name LIKE '%{name}%' and {"\""}IsActive{"\""}
                        ";
            return await _sqlContext.Connection.QueryAsync<Client>(sql);
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            
            var query = (from client in _sqlContext.Client
                         where client.IsActive && client.Id == id
                         select new Client(client.Id, 
                                           client.Name, 
                                           client.Document,
                                           _sqlContext.Address.Where(c => c.IsActive && c.ClientId == client.Id).Include(c=> c.City).ToList(),
                                           _sqlContext.Contact.Where(c=>c.IsActive && c.ClientId == client.Id).ToList()));

            return await query.FirstOrDefaultAsync();
        }
    }
}