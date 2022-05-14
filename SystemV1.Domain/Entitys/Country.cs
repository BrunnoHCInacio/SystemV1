using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Country : Entity
    {
        public Country()
        {
            States = new List<State>();
        }
        public Country(Guid id,
                       string name)
        {
            States = new List<State>();
            Id = id;
            Name = name;
        }

        public Country(Guid id,
                       string name,    
                       List<State> states)
        {
            Id = id;
            Name = name;
            States = states;
        }

        public Country(Guid id,
                       string name,
                       DateTime dateRegister,
                       DateTime dateChange,
                       Guid idUserRegister,
                       Guid idUserChange,
                       bool isActive,
                       List<State> states)
        {
            Id = id;
            Name = name;
            DateRegister = dateRegister;
            DateChange = dateChange;
            IdUserRegister = idUserRegister;
            IdUserChange = idUserChange;
            IsActive = isActive;
            States = states;
        }

        public string Name { get; set; }

        public List<State> States { get; private set; }

        public ValidationResult ValidadeCountry()
        {
            return new CountryValidation().Validate(this);
        }

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