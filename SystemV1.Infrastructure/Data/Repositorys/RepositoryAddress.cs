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
    public class RepositoryAddress : Repository<Address>, IRepositoryAddress
    {
        private readonly SqlContext _sqlContext;

        public RepositoryAddress(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<Address> GetAddressByIdAsync(Guid id)
        {
            return await _sqlContext.Address.SingleAsync(a => a.IsActive && a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync(int page, int pageSize)
        {
            return await _sqlContext.Address.Where(a => a.IsActive)
                                            .Skip(GetSkip(page, pageSize))
                                            .Take(page)
                                            .ToListAsync();
        }

        public void RemoveAllByClientId(Guid clientId)
        {
            var sql = $@"UPDATE 
                            {"\""}Address{"\""} 
                         SET {"\""}IsActive{"\""} = false 
                         WHERE {"\""}ClientId{"\""} = '{clientId}' and {"\""}IsActive{"\""}";

            _sqlContext.Connection.Execute(sql);
        }
    }
}