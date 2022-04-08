using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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

        Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize);

        Task<TEntity> GetByIdAsync(Guid id);
    }
}