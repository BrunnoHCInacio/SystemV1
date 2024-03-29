﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void RemoveRange(Expression<Func<TEntity, bool>> conditional);

        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional);

        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional,
                                        int page,
                                        int pageSize);

        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> conditional,
                                     string includes = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> conditional);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditional);
    }
}