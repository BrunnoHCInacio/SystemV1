﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Repositorys
{
    public interface IRepositoryProductItem : IRepository<ProductItem>
    {
        Task<IEnumerable<ProductItem>> GetAllProductItemsAsync(int page, int pageSize);
        Task<ProductItem> GetProductItemByIdAsync(Guid id);
        Task<IEnumerable<ProductItem>> GetByNameAsync(string name);
    }
}