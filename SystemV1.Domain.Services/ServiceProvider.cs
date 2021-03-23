
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceProvider : Service<Provider>, IServiceProvider
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProvider(IRepositoryProvider repositoryProvider, 
                               IUnitOfWork unitOfWork):base(repositoryProvider, unitOfWork)
        {
            _repositoryProvider = repositoryProvider;
            _unitOfWork = unitOfWork;
        }

        public void Remove(Provider provider)
        {
            provider.IsActive = false;
            _repositoryProvider.Update(provider);
        }

        public void RemoveUow(Provider provider)
        {
            Remove(provider);
            _unitOfWork.Commit();
        }
    }
}
