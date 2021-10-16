using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryState : IRepository<State>
    {
        Task<IEnumerable<State>> GetByNameAsync(string name);

        Task<IEnumerable<State>> GetAllStatesAsync(int page, int pageSize);

        Task<State> GetStateByIdAsync(Guid id);

        Task<State> GetStateCountryByIdAsync(Guid id);
    }
}