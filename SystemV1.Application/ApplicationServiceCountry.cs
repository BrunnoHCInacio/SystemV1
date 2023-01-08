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
    public class ApplicationServiceCountry : ApplicationService<IServiceCountry,
                                                                CountryViewModel,
                                                                IMapperCountry,
                                                                Country>, IApplicationServiceCountry
    {
        private readonly IServiceCountry _serviceCountry;
        private readonly IMapperCountry _mapperCountry;

        public ApplicationServiceCountry(IServiceCountry serviceCountry,
                                         IMapperCountry mapperCountry) : base(serviceCountry, mapperCountry)
        {
            _serviceCountry = serviceCountry;
            _mapperCountry = mapperCountry;
        }

        public async Task<List<CountryViewModel>> GetByNameAsync(string name)
        {
            var countries = await _serviceCountry.SearchAsync(c => c.Name.ToUpper() == name.ToUpper());
            return _mapperCountry.ListEntityToViewModel(countries);
        }
    }
}