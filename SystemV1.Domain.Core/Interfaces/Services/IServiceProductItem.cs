using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceProductItem : IService<ProductItem>
    {
        void Remove(ProductItem productItem);

        void RemoveUow(ProductItem productItem);

        Task<IEnumerable<ProductItem>> GetByNameAsync(string name);
    }
}