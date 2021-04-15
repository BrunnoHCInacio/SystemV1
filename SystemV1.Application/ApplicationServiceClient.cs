using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Application
{
    public class ApplicationServiceClient : IApplicationServiceClient
    {
        private readonly IMapperClient _mapperClient;
        private readonly IServiceClient _serviceClient;

        public ApplicationServiceClient(IMapperClient mapperClient,
                                        IServiceClient serviceClient)
        {
            _mapperClient = mapperClient;
            _serviceClient = serviceClient;
        }

        public async Task AddAsync(ClientViewModel clientViewModel)
        {
            var client = _mapperClient.ViewModelToEntity(clientViewModel);
            await _serviceClient.AddAsyncUow(client);
        }

        public async Task<IEnumerable<ClientViewModel>> GetAllAsync(int page, int pageSize)
        {
            var clients = await _serviceClient.GetAllAsync(page, pageSize);
            return _mapperClient.ListEntityToViewModel(clients);
        }

        public async Task<ClientViewModel> GetByIdAsync(Guid id)
        {
            var client = await _serviceClient.GetByIdAsync(id);
            return _mapperClient.EntityToViewModel(client);
        }

        public async Task<IEnumerable<ClientViewModel>> GetByNameAsync(string name)
        {
            var clients = await _serviceClient.GetByNameAsync(name);
            return _mapperClient.ListEntityToViewModel(clients);
        }

        public async Task RemoveAsync(Guid id)
        {
            var client = await _serviceClient.GetByIdAsync(id);
            await _serviceClient.RemoveAsyncUow(client);
        }

        public async Task UpdateAsync(ClientViewModel clientViewModel)
        {
            var client = _mapperClient.ViewModelToEntity(clientViewModel);
            await _serviceClient.UpdateAsyncUow(client);
        }
    }
}