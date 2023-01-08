using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ProviderService : Service<IRepositoryProvider, Provider, IValidationProvider>, IProviderService
    {
        public ProviderService(IRepositoryProvider repositoryProvider,
                               IUnitOfWork unitOfWork,
                               INotifier notifier,
                               IValidationProvider validation) : base(notifier, repositoryProvider, unitOfWork, validation)
        {
        }
    }
}