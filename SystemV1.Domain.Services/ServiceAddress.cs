using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceAddress : Service<IRepositoryAddress, Address, IValidationAddress>, IServiceAddress
    {
        public ServiceAddress(IRepositoryAddress repositoryAddress,
                              IUnitOfWork unitOfWork,
                              INotifier notifier,
                              IValidationAddress validation) : base(notifier,
                                                                    repositoryAddress,
                                                                    unitOfWork,
                                                                    validation)
        {
        }
    }
}