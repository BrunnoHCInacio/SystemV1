using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperState : IMapperState
    {
        public StateViewModel EntityToViewModel(State state)
        {
            return new StateViewModel
            {
                Id = state.Id,
                Name = state.Name,
                CountryId = state.Country != null ? state.Country.Id : new Guid()
            };
        }

        public IEnumerable<StateViewModel> ListEntityToViewModel(IEnumerable<State> states)
        {
            return states.Select(s => EntityToViewModel(s));
        }

        public State ViewModelToEntity(StateViewModel stateViewModel)
        {
            var state = new State
            {
                Id = stateViewModel.Id,
                Name = stateViewModel.Name
            };

            if (stateViewModel.CountryId != new Guid())
            {
                state.Country = new Country
                {
                    Id = stateViewModel.CountryId
                };
            }

            return state;
        }
    }
}