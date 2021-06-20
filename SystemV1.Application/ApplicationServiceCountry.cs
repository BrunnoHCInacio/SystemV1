using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Application
{
    public class ApplicationServiceCountry : IApplicationServiceCountry
    {
        private readonly IServiceCountry _serviceCountry;
        private readonly IMapperCountry _mapperCountry;

        public ApplicationServiceCountry(IServiceCountry serviceCountry,
                                         IMapperCountry mapperCountry)
        {
            _serviceCountry = serviceCountry;
            _mapperCountry = mapperCountry;
        }

        public async Task AddAsync(CountryViewModel countryViewModel)
        {
            var country = _mapperCountry.ViewModelToEntity(countryViewModel);

            await _serviceCountry.AddAsyncUow(country);
        }

        public async Task<IEnumerable<CountryViewModel>> GetAllAsync(int page, int pageSize)
        {
            var countries = await _serviceCountry.GetAllAsync(page, pageSize);
            return _mapperCountry.ListEntityToViewModel(countries);
        }

        public async Task<CountryViewModel> GetByIdAsync(Guid id)
        {
            var country = await _serviceCountry.GetByIdAsync(id);
            return _mapperCountry.EntityToViewModel(country);
        }

        public async Task<IEnumerable<CountryViewModel>> GetByNameAsync(string name)
        {
            var countries = await _serviceCountry.GetByNameAsync(name);
            return _mapperCountry.ListEntityToViewModel(countries);
        }

        public async Task RemoveAsync(Guid id)
        {
            var country = await _serviceCountry.GetByIdAsync(id);
            if (country == null)
            {
            }

            await _serviceCountry.RemoveAsyncUow(country);
        }

        public async Task UpdateAsync(CountryViewModel countryViewModel)
        {
            var country = _mapperCountry.ViewModelToEntity(countryViewModel);
            await _serviceCountry.UpdateAsyncUow(country);
        }
    }
}