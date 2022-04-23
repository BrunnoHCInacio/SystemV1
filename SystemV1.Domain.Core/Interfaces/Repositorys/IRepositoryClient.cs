using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryClient : IRepository<Client>
    {
        Task<IEnumerable<Client>>GetAllClientsAsync(int page, int pageSize);
        Task<Client> GetClientByIdAsync(Guid id);
        Task<IEnumerable<Client>> GetByNameAsync(string name);
        Task<bool> ExistClient(Guid id);
    }
}