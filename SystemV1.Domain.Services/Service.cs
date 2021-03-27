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

        public void Add(TEntity entity)
        {
            _repository.Add(entity);
        }

        public async Task AddAsyncUow(TEntity entity)
        {
            Add(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize)
        {
            return await _repository.GetAllAsync(page, pageSize);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public async Task UpdateAsyncUow(TEntity entity)
        {
            Update(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}