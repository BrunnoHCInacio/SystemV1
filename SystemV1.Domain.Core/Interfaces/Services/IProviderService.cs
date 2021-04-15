using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IProviderService : IService<Provider>
    {
        void Remove(Provider provider);

        Task RemoveAsyncUow(Provider provider);

        Task<IEnumerable<Provider>> GetByNameAsync(string name);
    }
}