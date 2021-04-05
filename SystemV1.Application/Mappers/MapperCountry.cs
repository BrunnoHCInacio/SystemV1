using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public IEnumerable<CountryViewModel> ListEntityToViewModel(IEnumerable<Country> countries)
        {
            throw new NotImplementedException();
        }

        public Country ViewModelToEntity(CountryViewModel countryViewModel)
        {
            throw new NotImplementedException();
        }
    }
}