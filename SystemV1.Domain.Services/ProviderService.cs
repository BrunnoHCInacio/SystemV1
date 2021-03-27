﻿using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepositoryAddress _repositoryAddress;
        private readonly IRepositoryContact _repositoryContact;
        private readonly IUnitOfWork _unitOfWork;

        public ProviderService(IRepositoryProvider repositoryProvider,
                               IUnitOfWork unitOfWork) : base(repositoryProvider, unitOfWork)
        {
            _repositoryProvider = repositoryProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task AddProviderAsync(Provider provider)
        {
            if (provider.Addresses.Any())
            {
                foreach (var address in provider.Addresses)
                {
                    _repositoryAddress.Add(address);
                }
            }
            if (provider.Contacts.Any())
            {
                foreach (var contact in provider.Contacts)
                {
                    _repositoryContact.Add(contact);
                }
            }

            await AddAsyncUow(provider);
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
    }
}