using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : MainController
    {
        private readonly IApplicationServiceCountry _applicationServiceCountry;

        public CountryController(INotifier notifier,
                                 IApplicationServiceCountry applicationServiceCountry) : base(notifier)
        {
            _applicationServiceCountry = applicationServiceCountry;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetAll(int page, int pageSize)
        {
            if (page == 0 && pageSize == 0)
            {
                Notify("É necessário informar a página e a quantidade de itens por página");
                OkResult();
            }

            var countries = await _applicationServiceCountry.GetAllAsync(page, pageSize);
            return OkResult(countries);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<CountryViewModel>> GetById(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var country = await _applicationServiceCountry.GetByIdAsync(id);
            return OkResult(country);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetByName(string name)
        {
            var countries = await _applicationServiceCountry.GetByNameAsync(name);
            return OkResult(countries);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(CountryViewModel countryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }
            await _applicationServiceCountry.AddAsync(countryViewModel);
            return OkResult();
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CountryViewModel countryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            var country = await _applicationServiceCountry.GetByIdAsync(id);

            if (country.Id != id)
            {
                Notify("O Id informado é diferente do Id de país.");
                return OkResult();
            }
            await _applicationServiceCountry.UpdateAsync(countryViewModel);
            return OkResult();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            var country = _applicationServiceCountry.GetByIdAsync(id);
            if (country == null)
            {
                Notify("Não foi encontrato o país com o id informado.");
                return OkResult();
            }

            await _applicationServiceCountry.RemoveAsync(id);
            return OkResult();
        }
    }
}