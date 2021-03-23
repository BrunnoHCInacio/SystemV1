using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProduct : Service<Product>, IServiceProduct
    {
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProduct(IRepositoryProduct repositoryProduct, 
                              IUnitOfWork unitOfWork):base(repositoryProduct, unitOfWork)
        {
            _repositoryProduct = repositoryProduct;
            _unitOfWork = unitOfWork;
        }

        public void Remove(Product product)
        {
            product.IsActive = false;
            _repositoryProduct.Update(product);
        }

        public void RemoveUow(Product product)
        {
            Remove(product);
            _unitOfWork.Commit();
        }
    }
}
