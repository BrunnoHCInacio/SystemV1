using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceClient : Service<IRepositoryClient, Client, IValidationClient>, IServiceClient
    {
        public ServiceClient(IRepositoryClient repositoruClient,
                             IUnitOfWork unitOfWork,
                             INotifier notifier,
                             IValidationClient validation) : base(notifier,
                                                                  repositoruClient,
                                                                  unitOfWork,
                                                                  validation)
        {
        }
    }
}