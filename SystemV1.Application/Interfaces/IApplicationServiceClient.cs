﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application.Interfaces
{
    public interface IApplicationServiceClient
    {
        Task AddAsync(ClientViewModel clientViewModel);

        Task UpdateAsync(ClientViewModel clientViewModel);

        Task RemoveAsync(Guid id);

        Task<ClientViewModel> GetByIdAsync(Guid id);

        Task<IEnumerable<ClientViewModel>> GetAllAsync(int page, int pageSize);

        Task<IEnumerable<ClientViewModel>> GetByNameAsync(string name);
    }
}