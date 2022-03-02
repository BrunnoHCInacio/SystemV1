using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}