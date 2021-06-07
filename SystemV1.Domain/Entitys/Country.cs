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

        public string Name { get; private set; }

        public IEnumerable<State> States { get; set; }

        public ValidationResult ValidadeCountry()
        {
            return new CountryValidation().Validate(this);
        }
    }
}