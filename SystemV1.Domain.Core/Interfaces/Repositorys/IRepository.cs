using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAll(int page, int pageSize);

        Task<TEntity> GetById(Guid id);

        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Remove(TEntity entity);
    }
}