using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryContact : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync(int page, int pageSize);
        Task<Contact> GetContactByIdAsync(Guid id);
        void RemoveAllByClientId(Guid clientId);
        void RemoveAllByProviderId(Guid providerId);
    }
}