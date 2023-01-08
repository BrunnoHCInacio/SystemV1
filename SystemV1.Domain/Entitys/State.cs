using System;
using System.Collections.Generic;

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
            CountryId = country.Id;
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
    }
}