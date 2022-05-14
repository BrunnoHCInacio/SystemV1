using FluentValidation.Results;
using System;
using System.Collections.Generic;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class State : Entity
    {
        public State()
        {
            Cities = new List<City>();
        }

        public State(Guid id,
                     string name)
        {
            Cities = new List<City>();
            Id = id;
            Name = name;
        }

        public State(Guid id,
                     string name,
                     Country country)
        {
            Cities = new List<City>();
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
            Cities = new List<City>();
            Id = id;
            Name = name;
            DateRegister = dateRegister;
            DateChange = dateChange;
            IdUserRegister = idUserRegister;
            IdUserChange = idUserChange;
            Country = country;
            IsActive = isActive;
        }

        public string Name { get; set; }
        public Guid CountryId { get; private set; }
        public Country Country { get; set; }

        public List<City> Cities { get; private set; }

        public void SetCountry(Guid countryId)
        {
            CountryId = countryId;
        }

        public void SetCountry(Country country)
        {
            Country = country;
            CountryId = country != null ? country.Id : Guid.Empty;
        }

        public ValidationResult ValidateState()
        {
            return new StateValidation().Validate(this);
        }
    }
}