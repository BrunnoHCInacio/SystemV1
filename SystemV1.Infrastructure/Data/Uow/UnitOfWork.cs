using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Uow;

namespace SystemV1.Infrastructure.Data.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlContext _sqlContext;

        public UnitOfWork(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> CommitAsync()
        {
            await _sqlContext.SaveChangesAsync();
            return true;
        }
    }
}