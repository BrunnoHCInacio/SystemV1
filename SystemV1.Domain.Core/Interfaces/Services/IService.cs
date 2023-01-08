using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IService<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);

        Task AddAsyncUow(TEntity entity);

        void Update(TEntity entity);

        Task UpdateAsyncUow(TEntity entity);

        void Remove(TEntity entity);

        Task RemoveAsyncUow(TEntity entity);

        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional, int page, int pageSize);

        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional);

        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> conditional,
                                     string includes = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditional);
    }
}