using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.RegistersAPI.Controllers;

namespace SystemV1.API.Controllers
{
    [Route("api/Country")]
    public class CountryController : MainController
    {
        private readonly IApplicationServiceCountry _applicationServiceCountry;

        public CountryController(IApplicationServiceCountry applicationServiceCountry)
        {
            _applicationServiceCountry = applicationServiceCountry;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetAll(int page, int pageSize)
        {
            var countries = await _applicationServiceCountry.GetAllAsync(page, pageSize);
            return Ok(countries);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<CountryViewModel>> GetById(Guid id)
        {
            var country = await _applicationServiceCountry.GetByIdAsync(id);
            return Ok(country);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetByName(string name)
        {
            var countries = await _applicationServiceCountry.GetByNameAsync(name);
            return Ok(countries);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(CountryViewModel countryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Ok();
            }
            await _applicationServiceCountry.AddAsync(countryViewModel);
            return Ok();
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CountryViewModel countryViewModel)
        {
            var country = await _applicationServiceCountry.GetByIdAsync(id);

            if (country.Id != id)
            {
                Notify("O Id informado é diferente do Id de país.");
                return Ok();
            }
            await _applicationServiceCountry.UpdateAsync(countryViewModel);
            return Ok();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            var country = _applicationServiceCountry.GetByIdAsync(id);
            if (country == null)
            {
                Notify("Não foi encontrato o país com o id informado.");
                return Ok();
            }

            await _applicationServiceCountry.RemoveAsync(id);
            return Ok();
        }
    }
}