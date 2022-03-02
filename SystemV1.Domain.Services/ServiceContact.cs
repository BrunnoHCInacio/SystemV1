using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceContact : Service, IServiceContact
    {
        private readonly IRepositoryContact _repositoryContact;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceContact(IRepositoryContact repositoryContact,
                              IUnitOfWork unitOfWork,
                              INotifier notifier) : base(notifier)
        {
            _repositoryContact = repositoryContact;
            _unitOfWork = unitOfWork;
        }

        public void Add(Contact contact)
        {
            _repositoryContact.Add(contact);
        }

        public async Task AddAsyncUow(Contact contact)
        {
            if (!RunValidation(contact.ValidateContact()))
            {
                return;
            }

            try
            {
                Add(contact);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o contato.");
            }
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryContact.GetAllContactsAsync(page, pageSize);
            }
            catch(Exception)
            {
                Notify("Falha ao obter todos os contatos.");
            }
            return null;
        }

        public async Task<Contact> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryContact.GetContactByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o contato por id.");
            }
            return null;
        }

        public void Remove(Contact contact)
        {
            contact.DisableRegister();
            Update(contact);
        }

        public void RemoveAllByClientId(Guid clientId)
        {
            _repositoryContact.RemoveAllByClientId(clientId);
        }

        public async Task RemoveAsyncUow(Contact contact)
        {
            try
            {
                Remove(contact);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o contato.");
            }
        }

        public void Update(Contact contact)
        {
            _repositoryContact.Update(contact);
        }

        public async Task UpdateAsyncUow(Contact contact)
        {
            if (!RunValidation(contact.ValidateContact()))
            {
                return;
            }

            try
            {
                Update(contact);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao modificar o contato.");
            }
        }
    }
}