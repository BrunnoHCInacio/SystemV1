using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceCountry : IService<Country>
    {
        Task<IEnumerable<Country>> GetByNameAsync(string name);
        Task<bool> ExistsCountry(Guid id);
    }
}