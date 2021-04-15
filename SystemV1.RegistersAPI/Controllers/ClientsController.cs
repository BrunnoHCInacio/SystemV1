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
    [Route("api/clients")]
    public class ClientsController : MainController
    {
        private readonly IApplicationServiceClient _applicationServiceClient;

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ClientViewModel>>> GetAllAsync(int page, int pageSize)
        {
            var clients = await _applicationServiceClient.GetAllAsync(page, pageSize);
            return OkResult(clients);
        }

        [HttpGet("GetById/{guid:id}")]
        public async Task<ActionResult<ClientViewModel>> GetByIdAsync(Guid id)
        {
            var client = await _applicationServiceClient.GetByIdAsync(id);
            return OkResult(client);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<ClientViewModel>>> GetByNameAsync(string name)
        {
            var clients = await _applicationServiceClient.GetByNameAsync(name);
            return OkResult(clients);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(ClientViewModel clientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult();
            }

            await _applicationServiceClient.AddAsync(clientViewModel);

            return OkResult();
        }

        [HttpPut("Update/{guid:id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, ClientViewModel clientViewModel)
        {
            var client = await _applicationServiceClient.GetByIdAsync(id);

            if (client == null)
            {
                return BadRequest();
            }

            if (client.Id != clientViewModel.Id)
            {
                return BadRequest();
            }

            await _applicationServiceClient.UpdateAsync(clientViewModel);
            return OkResult();
        }

        [HttpDelete("Delete/{guid:id}")]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _applicationServiceClient.RemoveAsync(id);
            return OkResult();
        }
    }
}