using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

namespace SystemV1.Domain.Services
{
    public class ServiceClient : Service, IServiceClient
    {
        private readonly IRepositoryClient _repositoryClient;
        private readonly IServiceAddress _serviceAddress;
        private readonly IServiceContact _serviceContact;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;

        public ServiceClient(IRepositoryClient repositoruClient,
                             IUnitOfWork unitOfWork,
                             IServiceAddress serviceAddress,
                             IServiceContact serviceContact,
                             INotifier notifier) : base(notifier)
        {
            _repositoryClient = repositoruClient;
            _unitOfWork = unitOfWork;
            _serviceAddress = serviceAddress;
            _serviceContact = serviceContact;
            _notifier = notifier;
        }

        public void Add(Client client)
        {
            if (client.Addresses.Any())
            {
                foreach (var address in client.Addresses)
                {
                    _serviceAddress.Add(address);
                }
            }

            if (client.Contacts.Any())
            {
                foreach (var contact in client.Contacts)
                {
                    _serviceContact.Add(contact);
                }
            }

            _repositoryClient.Add(client);
        }

        public async Task AddAsyncUow(Client client)
        {
            if (!RunValidation(client.ValidateClient())
                && client.Addresses.Any(a => !RunValidation(a.ValidateAddress()))
                && client.Contacts.Any(c => !RunValidation(c.ValidateContact())))
            {
                return;
            }

            Add(client);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Client>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryClient.GetAllAsync(page, pageSize);
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _repositoryClient.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

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

        public void Update(Client client)
        {
            _repositoryClient.Update(client);
        }

        public async Task UpdateAsyncUow(Client client)
        {
            if (!RunValidation(client.ValidateClient())
                && client.Addresses.Any(a => !RunValidation(a.ValidateAddress()))
                && client.Contacts.Any(c => !RunValidation(c.ValidateContact())))
            {
                return;
            }

            Update(client);
            await _unitOfWork.CommitAsync();
        }
    }
}