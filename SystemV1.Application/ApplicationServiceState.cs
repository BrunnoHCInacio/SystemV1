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
    public class ApplicationServiceState : IApplicationServiceState
    {
        private readonly IServiceState _serviceState;
        private readonly IMapperState _mapperState;

        public ApplicationServiceState(IServiceState serviceState, IMapperState mapperState)
        {
            _serviceState = serviceState;
            _mapperState = mapperState;
        }

        public async Task AddAsync(StateViewModel stateViewModel)
        {
            var state = _mapperState.ViewModelToEntity(stateViewModel);
            await _serviceState.AddAsyncUow(state);
        }

        public async Task<IEnumerable<StateViewModel>> GetAllAsync(int page, int pageSize)
        {
            var states = await _serviceState.GetAllAsync(page, pageSize);
            return _mapperState.ListEntityToViewModel(states);
        }

        public async Task<StateViewModel> GetByIdAsync(Guid id)
        {
            var state = await _serviceState.GetByIdAsync(id);
            return _mapperState.EntityToViewModel(state);
        }

        public async Task<IEnumerable<StateViewModel>> GetByNameAsync(string name)
        {
            var states = await _serviceState.GetByNameAsync(name);
            return _mapperState.ListEntityToViewModel(states);
        }

        public async Task RemoveAsync(StateViewModel stateViewModel)
        {
            var state = _mapperState.ViewModelToEntity(stateViewModel);
            await _serviceState.RemoveAsyncUow(state);
        }

        public async Task UpdateAsync(StateViewModel stateViewModel)
        {
            var state = _mapperState.ViewModelToEntity(stateViewModel);
            await _serviceState.UpdateAsyncUow(state);
        }
    }
}