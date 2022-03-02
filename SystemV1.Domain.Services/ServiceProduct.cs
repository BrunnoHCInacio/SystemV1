using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProduct : Service, IServiceProduct
    {
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProduct(IRepositoryProduct repositoryProduct,
                              IUnitOfWork unitOfWork,
                              INotifier notifier, 
                              IRepositoryProductItem repositoryProductItem) : base(notifier)
        {
            _repositoryProduct = repositoryProduct;
            _unitOfWork = unitOfWork;
            _repositoryProductItem = repositoryProductItem;
        }

        public void Add(Product product)
        {
            if (product.ProductItems.Any())
            {
                foreach (var productItem in product.ProductItems)
                {
                    AddProductItem(productItem);
                }
            }
            _repositoryProduct.Add(product);
        }

        public async Task AddAsyncUow(Product product)
        {
            if (!RunValidation(product.ValidateProduct())
                || product.ProductItems.Any(pi => !RunValidation(pi.ValidateProductItem())))
            {
                return;
            }

            try
            {
                Add(product);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar produto.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryProduct.GetAllProductsAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter os produtos.");
            }
            return null;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryProduct.GetProductByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o produto por id.");
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

            try
            {
                return await _repositoryProduct.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter os produtos por nome.");
            }
            return null;
        }

        public void Remove(Product product)
        {
            product.DisableRegister();
            Update(product);
        }

        public async Task RemoveAsyncUow(Product product)
        {
            try
            {
                Remove(product);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o produto");
            }
        }

        public void Update(Product product)
        {
            _repositoryProduct.Update(product);
        }

        public async Task UpdateAsyncUow(Product product)
        {
            if (!RunValidation(product.ValidateProduct())
                || product.ProductItems.Any(pi => !RunValidation( pi.ValidateProductItem())))
            {
                return;
            }
            try
            {
                Update(product);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao alterar o produto");
            }
        }

        public void AddProductItem(ProductItem productItem)
        {
            _repositoryProductItem.Add(productItem);
        }


        public async Task<IEnumerable<ProductItem>> GetAllProductItemAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryProductItem.GetAllProductItemsAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter todos os items de produto.");
            }
            return null;
        }

        public async Task<ProductItem> GetProductItemByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryProductItem.GetProductItemByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o item de produto por id.");
            }
            return null;
        }

        public async Task<IEnumerable<ProductItem>> GetProductItemByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }
            try
            {
                return await _repositoryProductItem.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o item do produto por nome");
            }

            return null;
        }

        public void RemoveProductItem(ProductItem productItem)
        {
            productItem.DisableRegister();
            UpdateProductItem(productItem);
        }

        public async Task RemoveProductItemAsyncUow(ProductItem productItem)
        {
            try
            {
                RemoveProductItem(productItem);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o item do produto");
            }
        }

        public void UpdateProductItem(ProductItem productItem)
        {
            _repositoryProductItem.Update(productItem);
        }
    }
}