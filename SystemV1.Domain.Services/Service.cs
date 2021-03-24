using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Add(TEntity entity)
        {
            await _repository.Add(entity);
        }

        public async Task AddUow(TEntity entity)
        {
            await Add(entity);
            _unitOfWork.Commit();
        }

        public async Task<IEnumerable<TEntity>> GetAll(int page, int pageSize)
        {
            return await _repository.GetAll(page, pageSize);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async Task Update(TEntity entity)
        {
            await _repository.Update(entity);
        }

        public async Task UpdateUow(TEntity entity)
        {
            await Update(entity);
            _unitOfWork.Commit();
        }
    }
}