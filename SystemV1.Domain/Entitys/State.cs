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

        public State(Guid id,
                     string name,
                     Country country)
        {
            Id = id;
            Name = name;
            Country = country;
        }

        public State(Guid id,
                     string name,
                     DateTime dateRegister,
                     DateTime dateChange,
                     Guid idUserRegister,
                     Guid idUserChange,
                     Country country,
                     bool isActive)
        {
            Id = id;
            Name = name;
            DateRegister = dateRegister;
            DateChange = dateChange;
            IdUserRegister = idUserRegister;
            IdUserChange = idUserChange;
            Country = country;
            IsActive = isActive;
        }

        public string Name { get; private set; }
        public Guid CountryId { get; private set; }
        public Country Country { get; private set; }

        public void SetCountry(Guid countryId)
        {
            CountryId = countryId;
        }

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