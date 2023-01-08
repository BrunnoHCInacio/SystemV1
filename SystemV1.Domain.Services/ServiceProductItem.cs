using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProductItem : Service<IRepositoryProductItem, ProductItem, IValidationProductItem>, IServiceProductItem
    {
        public ServiceProductItem(INotifier notifier,
                                  IRepositoryProductItem repository,
                                  IUnitOfWork unitOfWork,
                                  IValidationProductItem validation) : base(notifier,
                                                                            repository,
                                                                            unitOfWork,
                                                                            validation)
        {
        }
    }
}