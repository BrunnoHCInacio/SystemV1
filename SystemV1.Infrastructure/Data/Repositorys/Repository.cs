using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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

        public void RemoveRange(Expression<Func<TEntity, bool>> conditional)
        {
            _sqlContext.Set<TEntity>().RemoveRange(_sqlContext.Set<TEntity>().Where(conditional));
        }

        public void Update(TEntity entity)
        {
            _sqlContext.Entry(entity).State = EntityState.Modified;
            _sqlContext.Set<TEntity>().Update(entity);
        }

        public int GetSkip(int page,int pageSize)
        {
            return (page - 1) * pageSize;
        }

        public void Remove(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Remove(entity);
        }
    }
}