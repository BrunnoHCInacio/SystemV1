using System;
using System.Collections.Generic;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> GetAll(int page, int pageSize);

        TEntity GetById(Guid id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}