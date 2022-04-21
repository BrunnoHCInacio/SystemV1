using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.Application
{
    public class ApplicationServiceCity : IApplicationServiceCity
    {
        private readonly IServiceCity _serviceCity;
        private readonly IMapperCity _mapperCity;

        public ApplicationServiceCity(IServiceCity serviceCity, 
                                      IMapperCity mapperCity)
        {
            _serviceCity = serviceCity;
            _mapperCity = mapperCity;
        }

        public async Task AddAsync(CityViewModel cityViewModel)
        {
            var city = _mapperCity.ViewModelToEntity(cityViewModel);
            await _serviceCity.AddAsyncUow(city);
        }

        public async Task UpdateAsync(CityViewModel cityViewModel)
        {
            var city = _mapperCity.ViewModelToEntity(cityViewModel);
            await _serviceCity.UpdateAsyncUow(city);
        }

        public async Task<IEnumerable<CityViewModel>> GetAllAsync(int page, int pageSize)
        {
            var cities = await _serviceCity.GetAllAsync(page, pageSize);
            return _mapperCity.ListEntityToViewModel(cities);
        }

        public async Task<CityViewModel> GetByIdAsync(Guid id)
        {
            var city = await _serviceCity.GetByIdAsync(id);
            if (city != null)
            {
                return _mapperCity.EntityToViewModel(city);
            }

            return null;
        }

        public async Task DeleteAsync(CityViewModel cityViewModel)
        {
            var city = _mapperCity.ViewModelToEntity(cityViewModel);
            await _serviceCity.RemoveAsyncUow(city);
        }
    }
}
