using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize);

        Task<TEntity> GetByIdAsync(Guid id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}