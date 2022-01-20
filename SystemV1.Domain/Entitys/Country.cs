using FluentValidation.Results;
using System;
using System.Collections.Generic;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Country : Entity
    {
        public Country(Guid id,
                       string name)
        {
            States = new List<State>();
            Id = id;
            Name = name;
        }

        public Country(Guid id,
                       string name,
                       DateTime dateRegister,
                       DateTime dateChange,
                       Guid idUserRegister,
                       Guid idUserChange,
                       bool isActive)
        {
            States = new List<State>();
            Id = id;
            Name = name;
            DateRegister = dateRegister;
            DateChange = dateChange;
            IdUserRegister = idUserRegister;
            IdUserChange = idUserChange;
            IsActive = isActive;
        }

        public string Name { get; private set; }

        public List<State> States { get; private set; }

        public ValidationResult ValidadeCountry()
        {
            return new CountryValidation().Validate(this);
        }

        public void AddStates(List<State> states)
        {
            States.AddRange(states);
        }

        public void AddState(State state)
        {
            States.Add(state);
        }
    }
}