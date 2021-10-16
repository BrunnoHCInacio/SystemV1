using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceState : IService<State>
    {
        Task<IEnumerable<State>> GetByNameAsync(string name);

        Task<State> GetStateCountryByIdAsync(Guid id);

        void Remove(State state);

        Task RemoveAsyncUow(State state);
    }
}