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
    [Route("api/State")]
    public class StateController : MainController
    {
        private readonly IApplicationServiceState _applicationServiceState;

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<StateViewModel>>> GetAllAsync(int page, int pageSize)
        {
            var states = await _applicationServiceState.GetAllAsync(page, pageSize);
            return OkResult(states);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<StateViewModel>> GetByIdAsync(Guid id)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);
            return OkResult(state);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult> GetByNameAsync(string name)
        {
            var states = await _applicationServiceState.GetByNameAsync(name);
            return OkResult(states);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(StateViewModel stateViewModel)
        {
            await _applicationServiceState.AddAsync(stateViewModel);
            return OkResult();
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> UpdateAsync(Guid id, StateViewModel stateViewModel)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);

            if (state == null)
            {
                return BadRequest();
            }

            if (id != state.Id)
            {
                return BadRequest();
            }

            await _applicationServiceState.UpdateAsync(stateViewModel);
            return OkResult();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);
            await _applicationServiceState.RemoveAsync(state);
            return OkResult();
        }
    }
}