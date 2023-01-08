using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application
{
    public class ApplicationServiceCity : ApplicationService<IServiceCity,
                                                             CityViewModel,
                                                             IMapperCity,
                                                             City>, IApplicationServiceCity
    {
        private readonly IServiceCity _serviceCity;
        private readonly IMapperCity _mapperCity;

        public ApplicationServiceCity(IServiceCity serviceCity,
                                      IMapperCity mapperCity) : base(serviceCity, mapperCity)
        {
            _serviceCity = serviceCity;
            _mapperCity = mapperCity;
        }

        public async Task<List<CityViewModel>> GetByNameAsync(string name)
        {
            var city = await _serviceCity.SearchAsync(c => c.Name.ToUpper() == name.ToUpper());
            return _mapperCity.ListEntityToViewModel(city);
        }

        public async Task<CityViewModel> GetCityStateByIdAsync(Guid id)
        {
            var city = await _serviceCity.GetEntityAsync(c => c.Id == id, "State");
            return _mapperCity.EntityToViewModel(city);
        }
    }
}