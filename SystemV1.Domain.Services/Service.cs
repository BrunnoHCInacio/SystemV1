using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Notifications;

namespace SystemV1.Domain.Services
{
    public class Service<TRepository, TEntity, TValidation> : IService<TEntity>
        where TEntity : Entity
        where TRepository : IRepository<TEntity>
        where TValidation : IValidation<TEntity>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly TRepository _repository;
        private readonly TValidation _validation;

        public Service(INotifier notifier,
                       TRepository repository,
                       IUnitOfWork unitOfWork,
                       TValidation validation)
        {
            _repository = repository;
            _notifier = notifier;
            _unitOfWork = unitOfWork;
            _validation = validation;
        }

        public bool IsValidOperation(ValidationResult result)
        {
            if (result.IsValid) return true;

            Notify(result);

            return false;
        }

        public bool RunValidation(TValidation validation, TEntity entity)
        {
            var validator = validation.Validate(entity);
            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }

        public void Notify(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        public void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        public virtual async Task AddAsyncUow(TEntity entity)
        {
            if (!IsValidOperation(ValidateAdd(entity)))
            {
                return;
            }

            Add(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsyncUow(TEntity entity)
        {
            if (!IsValidOperation(ValidateUpdate(entity)))
            {
                return;
            }

            Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveAsyncUow(TEntity entity)
        {
            if (!IsValidOperation(ValidateDelete(entity)))
            {
                return;
            }
            Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional, int page, int pageSize)
        {
            return await _repository.SearchAsync(conditional, page, pageSize);
        }

        public async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> conditional)
        {
            return await _repository.SearchAsync(conditional);
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> conditional,
                                                  string includes = null)
        {
            return await _repository.GetEntityAsync(conditional, includes);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditional)
        {
            return await _repository.ExistsAsync(conditional);
        }

        public void Add(TEntity entity)
        {
            _repository.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _repository.Remove(entity);
        }

        public ValidationResult ValidateAdd(TEntity entity)
        {
            _validation.RulesForAdd();
            return _validation.Validate(entity);
        }

        private ValidationResult ValidateUpdate(TEntity entity)
        {
            _validation.RulesForUpdate();
            return _validation.Validate(entity);
        }

        private ValidationResult ValidateDelete(TEntity entity)
        {
            _validation.RulesForDelete();
            return _validation.Validate(entity);
        }
    }
}