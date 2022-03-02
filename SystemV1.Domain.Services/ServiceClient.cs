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
                || client.Addresses.Any(a => !RunValidation(a.ValidateAddress()))
                || client.Contacts.Any(c => !RunValidation(c.ValidateContact())))
            {
                return;
            }
            try
            {
                Add(client);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o cliente.");
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryClient.GetAllClientsAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter todos os clientes.");
            }
            return null;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryClient.GetClientByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o cliente por nome.");
            }

            return null;
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

            try
            {
                return await _repositoryClient.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o cliente por nome.");
            }
            return null;
        }

        public void Remove(Client client)
        {
            client.IsActive = false;
            if (client.Addresses.Any())
            {
                _serviceAddress.RemoveAllByClientId(client.Id);
            }
            if (client.Contacts.Any())
            {
                _serviceContact.RemoveAllByClientId(client.Id);
            }
            _repositoryClient.Update(client);
        }

        public async Task RemoveAsyncUow(Client client)
        {
            try
            {
                Remove(client);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover cliente.");
            }
        }

        public void Update(Client client)
        {
            if (client.Addresses.Any())
            {
                _serviceAddress.RemoveAllByClientId(client.Id);
                foreach (var address in client.Addresses)
                {
                    _serviceAddress.Add(address);
                }
            }

            if (client.Contacts.Any())
            {
                _serviceContact.RemoveAllByClientId(client.Id);
                foreach (var contact in client.Contacts)
                {
                    _serviceContact.Add(contact);
                }
            }
            _repositoryClient.Update(client);
        }

        public async Task UpdateAsyncUow(Client client)
        {
            if (!RunValidation(client.ValidateClient())
                || client.Addresses.Any(a => !RunValidation(a.ValidateAddress()))
                || client.Contacts.Any(c => !RunValidation(c.ValidateContact())))
            {
                return;
            }

            try
            {
                Update(client);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao alterar o cliente.");
            }
        }
    }
}