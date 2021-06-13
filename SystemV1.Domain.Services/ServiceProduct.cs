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
    public class ServiceProduct : Service, IServiceProduct
    {
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProduct(IRepositoryProduct repositoryProduct,
                              IUnitOfWork unitOfWork,
                              INotifier notifier) : base(notifier)
        {
            _repositoryProduct = repositoryProduct;
            _unitOfWork = unitOfWork;
        }

        public void Add(Product product)
        {
            _repositoryProduct.Add(product);
        }

        public async Task AddAsyncUow(Product product)
        {
            if (!RunValidation(product.ValidateProduct()))
            {
                return;
            }

            Add(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryProduct.GetAllAsync(page, pageSize);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _repositoryProduct.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

            return await _repositoryProduct.GetByNameAsync(name);
        }

        public void Remove(Product product)
        {
            product.IsActive = false;
            Update(product);
        }

        public void RemoveUow(Product product)
        {
            Remove(product);
            _unitOfWork.CommitAsync();
        }

        public void Update(Product product)
        {
            _repositoryProduct.Update(product);
        }

        public async Task UpdateAsyncUow(Product product)
        {
            if (!RunValidation(product.ValidateProduct()))
            {
                return;
            }
            Update(product);
            await _unitOfWork.CommitAsync();
        }
    }
}