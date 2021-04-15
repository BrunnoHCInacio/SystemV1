using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

namespace SystemV1.Domain.Services
{
    public class ServiceProductItem : Service, IServiceProductItem
    {
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProductItem(IRepositoryProductItem repositoryProductItem,
                                  IUnitOfWork unitOfWork,
                                  INotifier notifier) : base(notifier)
        {
            _repositoryProductItem = repositoryProductItem;
            _unitOfWork = unitOfWork;
        }

        public void Add(ProductItem productItem)
        {
            _repositoryProductItem.Add(productItem);
        }

        public async Task AddAsyncUow(ProductItem productItem)
        {
            if (!RunValidation(new ProductItemValidation(), productItem))
            {
                return;
            }

            Add(productItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProductItem>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryProductItem.GetAllAsync(page, pageSize);
        }

        public async Task<ProductItem> GetByIdAsync(Guid id)
        {
            return await _repositoryProductItem.GetByIdAsync(id);
        }

        public void Remove(ProductItem productItem)
        {
            productItem.IsActive = false;
            Update(productItem);
        }

        public void RemoveUow(ProductItem productItem)
        {
            Remove(productItem);
            _unitOfWork.CommitAsync();
        }

        public void Update(ProductItem productItem)
        {
            _repositoryProductItem.Update(productItem);
        }

        public async Task UpdateAsyncUow(ProductItem productItem)
        {
            if (!RunValidation(new ProductItemValidation(), productItem))
            {
                return;
            }

            Update(productItem);
            await _unitOfWork.CommitAsync();
        }
    }
}