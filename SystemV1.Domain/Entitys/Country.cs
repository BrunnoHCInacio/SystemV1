using System;
using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Country : Entity
    {
        public Country(string name,
                       Guid id = new Guid(),
                       IEnumerable<State> states = null)
        {
            States = new List<State>();
            Id = id;
            Name = name;
            States = states;
        }

        public string Name { get; set; }

        public IEnumerable<State> States { get; set; }
    }
}