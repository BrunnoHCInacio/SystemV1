using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IService<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IEnumerable<TEntity> GetAll(int page, int pageSize);

        TEntity GetById(int id);
    }
}