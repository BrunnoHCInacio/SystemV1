using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;

namespace SystemV1.Application
{
    public class ApplicationServiceCountry : IApplicationServiceCountry
    {
        public Task Add(CountryViewModel countryViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CountryViewModel>> GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<CountryViewModel> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CountryViewModel>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, CountryViewModel countryViewModel)
        {
            throw new NotImplementedException();
        }
    }
}