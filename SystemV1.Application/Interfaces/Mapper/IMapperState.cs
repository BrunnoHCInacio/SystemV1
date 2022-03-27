using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperState
    {
        StateViewModel EntityToViewModel(State state);

        State ViewModelToEntity(StateViewModel stateViewModel);

        IEnumerable<StateViewModel> ListEntityToViewModel(IEnumerable<State> state);

        List<State> ListViewModelToEntity(IEnumerable<StateViewModel> statesViewModel);
    }
}