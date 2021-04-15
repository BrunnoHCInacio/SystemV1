using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

namespace SystemV1.Domain.Services
{
    public class ProviderService : Service, IProviderService
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IServiceAddress _serviceAddress;
        private readonly IServiceContact _serviceContact;
        private readonly IUnitOfWork _unitOfWork;

        public ProviderService(IRepositoryProvider repositoryProvider,
                               IUnitOfWork unitOfWork,
                               IServiceAddress serviceAddress,
                               IServiceContact serviceContact,
                               INotifier notifier) : base(notifier)
        {
            _repositoryProvider = repositoryProvider;
            _unitOfWork = unitOfWork;
            _serviceAddress = serviceAddress;
            _serviceContact = serviceContact;
        }

        public void Add(Provider provider)
        {
            if (provider.Addresses.Any())
            {
                foreach (var address in provider.Addresses)
                {
                    _serviceAddress.Add(address);
                }
            }

            if (provider.Addresses.Any())
            {
                foreach (var contact in provider.Contacts)
                {
                    _serviceContact.Add(contact);
                }
            }

            _repositoryProvider.Add(provider);
        }

        public async Task AddAsyncUow(Provider provider)
        {
            if (!RunValidation(new ProviderValidation(), provider)
                && provider.Addresses.Any(a => !RunValidation(new AddressValidation(), a))
                && provider.Contacts.Any(c => !RunValidation(new ContactValidation(), c)))
            {
                return;
            }

            Add(provider);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Provider>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryProvider.GetAllAsync(page, pageSize);
        }

        public async Task<Provider> GetByIdAsync(Guid id)
        {
            return await _repositoryProvider.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Provider>> GetByNameAsync(string name)
        {
            return await _repositoryProvider.GetByNameAsync(name);
        }

        public void Remove(Provider provider)
        {
            provider.IsActive = false;
            _repositoryProvider.Update(provider);
        }

        public async Task RemoveAsyncUow(Provider provider)
        {
            Remove(provider);
            await _unitOfWork.CommitAsync();
        }

        public void Update(Provider provider)
        {
            _repositoryProvider.Update(provider);
        }

        public async Task UpdateAsyncUow(Provider provider)
        {
            if (!RunValidation(new ProviderValidation(), provider)
                && provider.Addresses.Any(a => !RunValidation(new AddressValidation(), a))
                && provider.Contacts.Any(c => !RunValidation(new ContactValidation(), c)))
            {
                return;
            }

            Update(provider);
            await _unitOfWork.CommitAsync();
        }
    }
}