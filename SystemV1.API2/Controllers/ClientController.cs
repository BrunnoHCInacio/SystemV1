using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Resources;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Route("api/client")]
    public class ClientController : MainController
    {
        private readonly IApplicationServiceClient _applicationServiceClient;

        public ClientController(INotifier notifier,
                                IApplicationServiceClient applicationServiceClient) : base(notifier)
        {
            _applicationServiceClient = applicationServiceClient;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAllAsync(int page, int pageSize)
        {
            var clients = await _applicationServiceClient.GetAllAsync(page, pageSize);
            return OkResult(clients);
            
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            var client = await _applicationServiceClient.GetByIdAsync(id);
            return OkResult(client);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult> GetByNameAsync(string name)
        {
            var clients = await _applicationServiceClient.GetByNameAsync(name);
            return OkResult(clients);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(ClientViewModel clientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            await _applicationServiceClient.AddAsync(clientViewModel);

            return OkResult();
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> UpdateAsync(Guid id, ClientViewModel clientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            if(!await _applicationServiceClient.ExistsClient(id))
            {
                Notify(ConstantMessages.ClientNotFound);
                return OkResult();
            }

            await _applicationServiceClient.UpdateAsync(clientViewModel);
            return OkResult();
        }

        [HttpDelete("Remove/{id:guid}")]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            if (!await _applicationServiceClient.ExistsClient(id))
            {
                Notify(ConstantMessages.ClientNotFound);
                return OkResult();
            }

            await _applicationServiceClient.RemoveAsync(id);
            return OkResult();
        }
    }
}