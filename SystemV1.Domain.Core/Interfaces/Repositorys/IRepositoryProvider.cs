using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryProvider : IRepository<Provider>
    {
        Task<IEnumerable<Provider>> GetAllProvidersAsync(int page, int pageSize);
        Task<Provider> GetProviderByIdAsync(Guid id);
        Task<IEnumerable<Provider>> GetByNameAsync(string name);
    }
}