﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryCity : IRepository<City>
    {
        Task<IEnumerable<City>> GetAllCitiesAsync(int page, int pageSize);
        Task<City> GetByIdAsync(Guid id);
        Task<IEnumerable<City>> GetByNameAsync(string name);
    }
}
