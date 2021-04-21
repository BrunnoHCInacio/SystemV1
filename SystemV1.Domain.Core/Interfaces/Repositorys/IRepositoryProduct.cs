using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}