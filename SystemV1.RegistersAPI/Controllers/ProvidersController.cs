using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;
using SystemV1.RegistersAPI.Controllers;

namespace SystemV1.API.Controllers
{
    [Route("api/providers")]
    public class ProvidersController : MainController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetAll(int page, int pageSize)
        {
            return OkResult();
        }

        [HttpGet("GetById/{guid:id}")]
        public async Task<ActionResult<ProviderViewModel>> GetById(Guid id)
        {
            return OkResult();
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetByName(string name)
        {
            return OkResult();
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(ProviderViewModel providerViewModel)
        {
            return OkResult();
        }

        [HttpPut("Update/{guid:id}")]
        public async Task<ActionResult> Update(Guid id, ProviderViewModel providerViewModel)
        {
            return OkResult();
        }
    }
}