using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application
{
    public class ApplicationServiceState : ApplicationService<IServiceState,
                                                              StateViewModel,
                                                              IMapperState,
                                                              State>, IApplicationServiceState
    {
        private readonly IServiceState _serviceState;
        private readonly IMapperState _mapperState;

        public ApplicationServiceState(IServiceState serviceState,
                                       IMapperState mapperState) : base(serviceState, mapperState)
        {
            _serviceState = serviceState;
            _mapperState = mapperState;
        }

        public async Task<StateViewModel> GetStateCountryByIdAsync(Guid id)
        {
            var state = await _serviceState.GetEntityAsync(s => s.Id == id, "Country");
            return _mapperState.EntityToViewModel(state);
        }

        public async Task<List<StateViewModel>> GetByName(string name)
        {
            var states = await _serviceState.SearchAsync(c => c.Name.ToUpper() == name.ToUpper());
            return _mapperState.ListEntityToViewModel(states);
        }
    }
}