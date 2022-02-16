using System;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryAddress : IRepository<Address>
    {
        void RemoveAllByClientId(Guid clientId);
    }
}