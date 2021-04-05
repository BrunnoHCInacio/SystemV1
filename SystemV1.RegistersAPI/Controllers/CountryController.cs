using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;
using SystemV1.RegistersAPI.Controllers;

namespace SystemV1.API.Controllers
{
    [Route("api/Country")]
    public class CountryController : MainController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetAll(int page, int pageSize)
        {
            return OkResult();
        }

        [HttpGet("GetById/{guid:id}")]
        public async Task<ActionResult<CountryViewModel>> GetById(Guid id)
        {
            return OkResult();
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<CountryViewModel>>> GetByName(string name)
        {
            return OkResult();
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(CountryViewModel countryViewModel)
        {
            return OkResult();
        }

        [HttpPut("Update/[guid:id}")]
        public async Task<ActionResult> Update(Guid id, CountryViewModel countryViewModel)
        {
            return OkResult();
        }

        [HttpDelete("Delete/{guid:id}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            return OkResult();
        }
    }
}