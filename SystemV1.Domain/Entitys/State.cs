using System;

namespace SystemV1.Domain.Entitys
{
    public class State : Entity
    {
        public State(Guid id,
                     string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public Guid StateId { get; set; }
        public Country Country { get; set; }
    }
}