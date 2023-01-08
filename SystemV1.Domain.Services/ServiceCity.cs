using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceCity : Service<IRepositoryCity, City, IValidationCity>, IServiceCity
    {
        public ServiceCity(INotifier notifier,
                           IRepositoryCity repositoryCity,
                           IUnitOfWork unitOfWork,
                           IValidationCity validation) : base(notifier,
                                                              repositoryCity,
                                                              unitOfWork,
                                                              validation)
        {
        }
    }
}