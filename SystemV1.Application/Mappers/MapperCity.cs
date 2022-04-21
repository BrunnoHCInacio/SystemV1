using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperCity : IMapperCity
    {
        public CityViewModel EntityToViewModel(City city)
        {
            return new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                StateId = city.State?.Id,
                StateName = city.State?.Name
            };
        }

        public IEnumerable<CityViewModel> ListEntityToViewModel(IEnumerable<City> cities)
        {
            if (cities == null)
            { 
                return null;
            }

            return cities.Select(c => EntityToViewModel(c));
        }

        public List<City> ListViewModelToEntity(IEnumerable<CityViewModel> cityViewModels)
        {
            return cityViewModels.Select(c => ViewModelToEntity(c)).ToList();
        }

        public City ViewModelToEntity(CityViewModel cityViewModel)
        {
            var city = new City(cityViewModel.Id, cityViewModel.Name);
            if (cityViewModel.StateId.HasValue 
                && cityViewModel.StateId != Guid.Empty)
            {
                city.SetState(cityViewModel.StateId.GetValueOrDefault());
            }
            return city;
        }
    }
}
