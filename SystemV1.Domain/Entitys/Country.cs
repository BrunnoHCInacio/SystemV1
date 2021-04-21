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

        public string Name { get; set; }

        public IEnumerable<State> States { get; set; }
    }
}