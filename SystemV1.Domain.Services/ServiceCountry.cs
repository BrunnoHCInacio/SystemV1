using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceCountry : Service<IRepositoryCountry, Country, IValidationCountry>, IServiceCountry
    {
        public ServiceCountry(INotifier notifier,
                              IRepositoryCountry repository,
                              IUnitOfWork unitOfWork,
                              IValidationCountry validation) : base(notifier,
                                                                    repository,
                                                                    unitOfWork,
                                                                    validation)
        {
        }
    }
}