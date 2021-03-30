using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceClient : Service<Client>, IServiceClient
    {
        private readonly IRepositoryClient _repositoryClient;
        private readonly IServiceAddress _serviceAddress;
        private readonly IServiceContact _serviceContact;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceClient(IRepositoryClient repositoruClient,
                             IUnitOfWork unitOfWork,
                             IServiceAddress serviceAddress,
                             IServiceContact serviceContact) : base(repositoruClient, unitOfWork)
        {
            _repositoryClient = repositoruClient;
            _unitOfWork = unitOfWork;
            _serviceAddress = serviceAddress;
            _serviceContact = serviceContact;
        }

        public async Task AddClientAsyncUow(Client client)
        {
            if (client.Addresses.Any())
            {
                foreach (var address in client.Addresses)
                {
                    _serviceAddress.Add(address);
                }
            }

            if (client.Addresses.Any())
            {
                foreach (var contact in client.Contacts)
                {
                    _serviceContact.Add(contact);
                }
            }

            await AddAsyncUow(client);
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            return await _repositoryClient.GetByNameAsync(name);
        }

        public void Remove(Client client)
        {
            client.IsActive = false;
            _repositoryClient.Update(client);
        }

        public async Task RemoveAsyncUow(Client client)
        {
            Remove(client);
            await _unitOfWork.CommitAsync();
        }
    }
}