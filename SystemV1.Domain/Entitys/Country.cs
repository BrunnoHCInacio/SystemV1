using System;
using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Country : Entity
    {
        public Country()
        {
            States = new List<State>();
        }

        public Country(Guid id)
        {
            Id = id;
        }

        public Country(Guid id,
                       string name,
                       List<State> states)
        {
            Id = id;
            Name = name;
            States = states;
        }

        public string Name { get; set; }

        public List<State> States { get; private set; }

        public void AddStates(List<State> states)
        {
            States = states;
        }

        public void AddState(State state)
        {
            States.Add(state);
        }
    }
}