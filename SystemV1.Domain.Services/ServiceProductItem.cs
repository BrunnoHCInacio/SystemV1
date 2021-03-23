using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProductItem : Service<ProductItem>, IServiceProductItem
    {
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IUnitOfWork _unitOfWork;
        public ServiceProductItem(IRepositoryProductItem repositoryProductItem, IUnitOfWork unitOfWork) : base(repositoryProductItem, unitOfWork)
        {
            _repositoryProductItem = repositoryProductItem;
            _unitOfWork = unitOfWork;
        }

        public void Remove(ProductItem productItem)
        {
            productItem.IsActive = false;
            _repositoryProductItem.Update(productItem);
        }

        public void RemoveUow(ProductItem productItem)
        {
            Remove(productItem);
            _unitOfWork.Commit();
        }
    }
}
