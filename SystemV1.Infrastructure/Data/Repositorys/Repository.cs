using Dapper;
using Microsoft.EntityFrameworkCore;
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

        public Repository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public void Add(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Add(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var sql = @$"SELECT *
                         FROM {"\""}{typeof(TEntity).Name}{"\""}
                         WHERE {"\""}IsActive{"\""}
                         ORDER BY {"\""}Id{"\""}
                         LIMIT {pageSize}
                         OFFSET {skip}"
                         ;

            return await _sqlContext.Connection.QueryAsync<TEntity>(sql);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var sql = $@"SELECT *
                         FROM {"\""}{typeof(TEntity).Name}{"\""}
                         WHERE {"\""}id{"\""} = {id}
                            and {"\""}IsActive{"\""}";
            return await _sqlContext.Connection.QueryAsync<TEntity>(sql) as TEntity;
        }

        public void Remove(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _sqlContext.Entry(entity).State = EntityState.Modified;
            _sqlContext.Set<TEntity>().Update(entity);
        }
    }
}