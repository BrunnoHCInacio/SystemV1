using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceProvider : IService<Provider>
    {
        void Remove(Provider provider);

        void RemoveUow(Provider provider);
    }
}