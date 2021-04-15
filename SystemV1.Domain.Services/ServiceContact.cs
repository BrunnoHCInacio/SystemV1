using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

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
            if (!RunValidation(new ContactValidation(), contact))
            {
                return;
            }

            Add(contact);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryContact.GetAllAsync(page, pageSize);
        }

        public async Task<Contact> GetByIdAsync(Guid id)
        {
            return await _repositoryContact.GetByIdAsync(id);
        }

        public void Remove(Contact contact)
        {
            contact.IsActive = false;
        }

        public async Task RemoveAsyncUow(Contact contact)
        {
            Update(contact);
            await _unitOfWork.CommitAsync();
        }

        public void Update(Contact contact)
        {
            _repositoryContact.Update(contact);
        }

        public async Task UpdateAsyncUow(Contact contact)
        {
            if (!RunValidation(new ContactValidation(), contact))
            {
                return;
            }

            Update(contact);
            await _unitOfWork.CommitAsync();
        }
    }
}