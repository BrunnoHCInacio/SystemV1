using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

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
            if (!IsValidEntity(provider.ValidateProvider())
                || provider.Addresses.Any(a => !IsValidEntity(a.ValidateAddress()))
                || provider.Contacts.Any(c => !IsValidEntity(c.ValidateContact())))
            {
                return;
            }

            try
            {
                Add(provider);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o fornecedor.");
            }
        }

        public async Task<IEnumerable<Provider>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryProvider.GetAllProvidersAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter todos fornecedores.");
            }

            return null;
        }

        public async Task<Provider> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryProvider.GetProviderByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o fornecedor por id.");
            }
            return null;
        }

        public async Task<IEnumerable<Provider>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

            try
            {
                return await _repositoryProvider.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter os fornecedores por nome.");
            }
            return null;
        }

        public void Remove(Provider provider)
        {
            provider.IsActive = false;
            _repositoryProvider.Update(provider);
        }

        public async Task RemoveAsyncUow(Provider provider)
        {
            try
            {
                Remove(provider);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o fornecedor.");
            }
        }

        public void Update(Provider provider)
        {
            _repositoryProvider.Update(provider);
        }

        public async Task UpdateAsyncUow(Provider provider)
        {
            if (!IsValidEntity(provider.ValidateProvider())
                || provider.Addresses.Any(a => !IsValidEntity(a.ValidateAddress()))
                || provider.Contacts.Any(c => !IsValidEntity(c.ValidateContact())))
            {
                return;
            }

            Update(provider);
            await _unitOfWork.CommitAsync();
        }
    }
}