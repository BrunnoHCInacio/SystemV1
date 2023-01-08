using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProduct : Service<IRepositoryProduct, Product, IValidationProduct>, IServiceProduct
    {
        private readonly IServiceProductItem _serviceProductItem;

        public ServiceProduct(IRepositoryProduct repositoryProduct,
                              IUnitOfWork unitOfWork,
                              INotifier notifier,
                              IValidationProduct validation,
                              IServiceProductItem serviceProductItem) : base(notifier,
                                                                    repositoryProduct,
                                                                    unitOfWork,
                                                                    validation)
        {
            _serviceProductItem = serviceProductItem;
        }

        public override async Task AddAsyncUow(Product product)
        {
            await base.AddAsyncUow(product);

            foreach (var produtItem in product.ProductItems)
            {
                await _serviceProductItem.AddAsyncUow(produtItem);
            }
        }
    }
}