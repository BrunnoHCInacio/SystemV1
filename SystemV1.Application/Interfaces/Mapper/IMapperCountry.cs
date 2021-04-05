using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperCountry
    {
        CountryViewModel EntityToViewModel(Country country);

        Country ViewModelToEntity(CountryViewModel countryViewModel);

        IEnumerable<CountryViewModel> ListEntityToViewModel(IEnumerable<Country> countries);
    }
}