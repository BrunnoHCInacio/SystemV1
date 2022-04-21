using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class City : Entity
    {
        public City(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public City(Guid id, string name, State state)
        {
            Id = id;
            Name = name;
            State = state;
            StateId = state.Id;
        }

        public string Name { get; set; }
        public Guid StateId { get; private set; }
        public State State { get; set; }

        public void SetState(Guid stateId)
        {
            StateId = stateId;
        }

        public ValidationResult ValidateCity()
        {
            return new CityValidation().Validate(this);
        }
    }
}
