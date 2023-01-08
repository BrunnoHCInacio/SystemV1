using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceContact : Service<IRepositoryContact, Contact, IValidationContact>, IServiceContact
    {
        public ServiceContact(IRepositoryContact repositoryContact,
                              IUnitOfWork unitOfWork,
                              INotifier notifier,
                              IValidationContact validation) : base(notifier,
                                                                    repositoryContact,
                                                                    unitOfWork,
                                                                    validation)
        {
        }
    }
}