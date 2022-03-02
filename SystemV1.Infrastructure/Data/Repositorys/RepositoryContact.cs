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
    public class RepositoryContact : Repository<Contact>, IRepositoryContact
    {
        private readonly SqlContext _sqlContext;

        public RepositoryContact(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(int page, int pageSize)
        {
            return await _sqlContext.Contact.Where(c => c.IsActive)
                                            .Skip(GetSkip(page, pageSize))
                                            .Take(pageSize)
                                            .ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            return await _sqlContext.Contact.SingleAsync(c => c.IsActive && c.Id == id);
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