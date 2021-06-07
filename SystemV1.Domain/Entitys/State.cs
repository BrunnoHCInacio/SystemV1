using FluentValidation.Results;
using System;
using SystemV1.Domain.Validations;

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

        public string Name { get; private set; }
        public Guid StateId { get; private set; }
        public Country Country { get; private set; }

        public void SetCountry(Country country)
        {
            Country = country;
        }

        public ValidationResult ValidateState()
        {
            return new StateValidation().Validate(this);
        }
    }
}