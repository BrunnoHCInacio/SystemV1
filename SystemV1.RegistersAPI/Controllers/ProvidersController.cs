using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.RegistersAPI.Controllers;

namespace SystemV1.API.Controllers
{
    [Route("api/providers")]
    public class ProvidersController : MainController
    {
        private readonly IApplicationServiceProvider _applicationServiceProvider;

        public ProvidersController(IApplicationServiceProvider applicationServiceProvider)
        {
            _applicationServiceProvider = applicationServiceProvider;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetAllAsync(int page, int pageSize)
        {
            var prodivers = await _applicationServiceProvider.GetAllAsync(page, pageSize);
            return OkResult(prodivers);
        }

        [HttpGet("GetById/{guid:id}")]
        public async Task<ActionResult<ProviderViewModel>> GetByIdAsync(Guid id)
        {
            var provider = await _applicationServiceProvider.GetByIdAsync(id);
            return OkResult(provider);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetByNameAsync(string name)
        {
            var providers = await _applicationServiceProvider.GetByNameAsync(name);
            return OkResult(providers);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid)
            {
                OkResult();
            }
            await _applicationServiceProvider.AddAsync(providerViewModel);
            return OkResult();
        }

        [HttpPut("Update/{guid:id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, ProviderViewModel providerViewModel)
        {
            var provider = await _applicationServiceProvider.GetByIdAsync(id);
            if (provider == null)
            {
                return BadRequest();
            }

            if (provider.Id != providerViewModel.Id)
            {
                return BadRequest();
            }

            await _applicationServiceProvider.UpdateAsync(providerViewModel);
            return OkResult();
        }
    }
}