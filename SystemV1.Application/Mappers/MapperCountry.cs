using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperCountry : IMapperCountry
    {
        private readonly IMapperState _mapperState;

        public MapperCountry(IMapperState mapperState)
        {
            _mapperState = mapperState;
        }

        public CountryViewModel EntityToViewModel(Country country)
        {
            if (country is null) return null;
            return new CountryViewModel
            {
                Id = country.Id,
                Name = country.Name
            };
        }

        public List<CountryViewModel> ListEntityToViewModel(List<Country> countries)
        {
            return countries.Select(c => EntityToViewModel(c)).ToList();
        }

        public List<Country> ListViewModelToEntity(List<CountryViewModel> viewModels)
        {
            return viewModels.Select(c => ViewModelToEntity(c)).ToList();
        }

        public Country ViewModelToEntity(CountryViewModel countryViewModel, Country countryPersisted)
        {
            countryPersisted.Id = countryViewModel.Id.GetValueOrDefault();
            countryPersisted.Name = countryViewModel.Name;

            return countryPersisted;
        }

        public Country ViewModelToEntity(CountryViewModel countryViewModel)
        {
            if (countryViewModel.Id.GetValueOrDefault() == null
                   || countryViewModel.Id.GetValueOrDefault() == Guid.Empty)
            {
                countryViewModel.Id = Guid.NewGuid();
            }

            return new Country(countryViewModel.Id.GetValueOrDefault(),
                               countryViewModel.Name,
                               null);
        }
    }
}