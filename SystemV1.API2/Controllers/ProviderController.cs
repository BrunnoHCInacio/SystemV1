using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : MainController
    {
        private readonly IApplicationServiceProvider _applicationServideProvider;

        public ProviderController(INotifier notifier,
                                  IApplicationServiceProvider applicationServideProvider) : base(notifier)
        {
            _applicationServideProvider = applicationServideProvider;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            await _applicationServideProvider.AddAsync(providerViewModel);

            return OkResult();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProviderViewModel>>> GetAll(int page = 1, int pageSize = 10)
        {
            var providers = await _applicationServideProvider.GetAllAsync(page, pageSize);
            return OkResult(providers);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<ProviderViewModel>> GetById(Guid id)
        {
            var provider = await _applicationServideProvider.GetByIdAsync(id);
            return OkResult(provider);
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            if (!await _applicationServideProvider.ExistsProvider(id))
            {
                Notify("Fornecedor não encontrato");
                return OkResult();
            }

            await _applicationServideProvider.UpdateAsync(providerViewModel);
            return OkResult();
        }

        [HttpDelete("Remove/{id:guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            if (!await _applicationServideProvider.ExistsProvider(id))
            {
                Notify("Fornecedor não encontrato");
                return OkResult();
            }

            await _applicationServideProvider.RemoveAsync(id);
            return OkResult();
        }
    }
}