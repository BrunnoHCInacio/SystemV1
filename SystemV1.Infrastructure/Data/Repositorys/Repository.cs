using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var entities = _sqlContext.Set<TEntity>().Where(conditional).ToList();
            _sqlContext.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _sqlContext.Entry(entity).State = EntityState.Modified;
            _sqlContext.Set<TEntity>().Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _sqlContext.Set<TEntity>().Remove(entity);
        }

        public async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional)
        {
            return await _sqlContext.Set<TEntity>().Where(conditional).ToListAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> conditional,
                                                  string includes = null)
        {
            var query = _sqlContext.Set<TEntity>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes.Split(','))
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(conditional);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> conditional)
        {
            return await _sqlContext.Set<TEntity>().Where(conditional).CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditional)
        {
            return await _sqlContext.Set<TEntity>().Where(conditional).AnyAsync();
        }

        public async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional,
                                               int page,
                                               int pageSize)
        {
            var query = _sqlContext.Set<TEntity>();
            if (conditional != null)
            {
                query.Where(conditional);
            }

            return await query.Skip(GetSkip(page, pageSize))
                              .Take(pageSize)
                              .ToListAsync();
        }

        private int GetSkip(int page, int pageSize)
        {
            return (page - 1) * pageSize;
        }
    }
}