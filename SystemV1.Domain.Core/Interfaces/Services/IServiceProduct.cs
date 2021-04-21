using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceProduct : IService<Product>
    {
        void Remove(Product product);

        void RemoveUow(Product product);

        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}