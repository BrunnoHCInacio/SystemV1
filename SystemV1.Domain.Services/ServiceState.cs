using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceState : Service<IRepositoryState, State, IValidationState>, IServiceState
    {
        public ServiceState(IRepositoryState repositoryState,
                            IUnitOfWork unitOfWork,
                            INotifier notifier,
                            IValidationState validationState) : base(notifier,
                                                                     repositoryState,
                                                                     unitOfWork,
                                                                     validationState)
        {
        }
    }
}