using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Route("api/city")]
    [ApiController]
    public class CityController : MainController
    {
        private readonly IApplicationServiceCity _applicationServiceCity;
        public CityController(INotifier notifier, 
                              IApplicationServiceCity applicationServiceCity) : base(notifier)
        {
            _applicationServiceCity = applicationServiceCity;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(CityViewModel cityViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            await _applicationServiceCity.AddAsync(cityViewModel);
            return OkResult();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CityViewModel>>> GetAll(int page, int pageSize)
        {
            if(page == 0 || pageSize == 0)
            {
                Notify("É necessário informar a página e a quantidade de itens por página");
                return OkResult();
            }

            var cities = await _applicationServiceCity.GetAllAsync(page, pageSize);
            return OkResult(cities);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<CityViewModel>> GetById(Guid id)
        {
            var city = await _applicationServiceCity.GetByIdAsync(id);
            return OkResult(city);
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CityViewModel cityViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            await _applicationServiceCity.UpdateAsync(cityViewModel);
            return OkResult();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var city = await _applicationServiceCity.GetByIdAsync(id);

            if(city == null)
            {
                Notify("Cidade não encontrada.");
                return OkResult();
            }

            await _applicationServiceCity.DeleteAsync(city);
            return OkResult();
        }
    }

}
