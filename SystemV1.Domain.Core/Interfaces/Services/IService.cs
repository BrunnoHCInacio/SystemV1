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
        Task Add(TEntity entity);

        Task AddUow(TEntity entity);

        Task Update(TEntity entity);

        Task UpdateUow(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll(int page, int pageSize);

        Task<TEntity> GetById(Guid id);
    }
}