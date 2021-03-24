using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly SqlContext _sqlContext;
        private SqlConnection _sqlConnection;

        public Repository(SqlContext sqlContext, IConfiguration configuration)
        {
            _sqlContext = sqlContext;
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("ConnectionString"));
        }

        public async Task Add(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Add(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var sql = @$"SELECT *
                         FROM {typeof(TEntity).Name}
                         WHERE isactive
                         ORDER BY id
                         OFFSET {pageSize} ROWS
                         FETCH NEXT {skip} ROWS ONLY";

            return _sqlConnection.Query<TEntity>(sql);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            var sql = $@"SELECT *
                         FROM {typeof(TEntity).Name}
                         WHERE id = {id}";
            return (TEntity)_sqlConnection.Query<TEntity>(sql);
        }

        public async Task Remove(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Remove(entity);
        }

        public async Task Update(TEntity entity)
        {
            _sqlContext.Entry(entity).State = EntityState.Modified;
            _sqlContext.Set<TEntity>().Update(entity);
        }
    }
}