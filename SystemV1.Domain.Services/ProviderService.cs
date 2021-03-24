using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ProviderService : Service<Provider>, IProviderService
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ProviderService(IRepositoryProvider repositoryProvider,
                               IUnitOfWork unitOfWork) : base(repositoryProvider, unitOfWork)
        {
            _repositoryProvider = repositoryProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task Remove(Provider provider)
        {
            provider.IsActive = false;
            _repositoryProvider.Update(provider);
        }

        public async Task RemoveUow(Provider provider)
        {
            Remove(provider);
            _unitOfWork.Commit();
        }
    }
}