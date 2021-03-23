using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceProduct : IService<Product>
    {
        void Remove(Product product);
        void RemoveUow(Product product);
    }
}