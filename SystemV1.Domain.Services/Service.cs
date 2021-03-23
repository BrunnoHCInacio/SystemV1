using System;
using System.Collections.Generic;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : Entity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IRepository<TEntity> repository,
                       IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(TEntity entity)
        {
            _repository.Add(entity);
        }

        public void AddUow(TEntity entity)
        {
            Add(entity);
            _unitOfWork.Commit();
        }

        public IEnumerable<TEntity> GetAll(int page, int pageSize)
        {
            return _repository.GetAll(page, pageSize);
        }

        public TEntity GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public void UpdateUow(TEntity entity)
        {
            Update(entity);
            _unitOfWork.Commit();
        }
    }
}