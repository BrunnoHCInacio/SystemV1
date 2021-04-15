using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceClient : IService<Client>
    {
        void Remove(Client client);

        Task RemoveAsyncUow(Client client);

        Task<IEnumerable<Client>> GetByNameAsync(string name);
    }
}