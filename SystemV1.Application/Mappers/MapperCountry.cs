using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperCountry : IMapperCountry
    {
        public CountryViewModel EntityToViewModel(Country country)
        {
            if (country is null) return null;
            return new CountryViewModel { Id = country?.Id, Name = country?.Name };
        }

        public IEnumerable<CountryViewModel> ListEntityToViewModel(IEnumerable<Country> countries)
        {
            return countries.Select(c => EntityToViewModel(c));
        }

        public Country ViewModelToEntity(CountryViewModel countryViewModel)
        {
            return new Country(countryViewModel.Id.GetValueOrDefault(), 
                               countryViewModel.Name);
        }
    }
}