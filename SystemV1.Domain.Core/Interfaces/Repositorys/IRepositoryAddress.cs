using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryAddress : IRepository<Address>
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync(int page, int pageSize);
        Task<Address> GetAddressByIdAsync(Guid id);
        void RemoveAllByClientId(Guid clientId);
    }
}