using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceAddress : IService<Address>
    {
        void RemoveAllByClientId(Guid clientId);
        void RemoveAllByProviderId(Guid ProviderId);
    }
}