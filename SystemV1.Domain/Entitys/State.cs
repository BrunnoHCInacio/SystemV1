using System;

namespace SystemV1.Domain.Entitys
{
    public class State : Entity
    {
        public State(string name,
                     Guid id = new Guid(),
                     Country country = null)
        {
            Id = id;
            Name = name;
            Country = country;
        }

        public string Name { get; set; }
        public Guid StateId { get; set; }
        public Country Country { get; set; }
    }
}