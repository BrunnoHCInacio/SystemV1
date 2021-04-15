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

        public StateController(IApplicationServiceState applicationServiceState)
        {
            _applicationServiceState = applicationServiceState;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<StateViewModel>>> GetAllAsync(int page, int pageSize)
        {
            var states = await _applicationServiceState.GetAllAsync(page, pageSize);
            return Ok(states);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult<StateViewModel>> GetByIdAsync(Guid id)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);
            return Ok(state);
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult> GetByNameAsync(string name)
        {
            var states = await _applicationServiceState.GetByNameAsync(name);
            return Ok(states);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(StateViewModel stateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Ok();
            }
            await _applicationServiceState.AddAsync(stateViewModel);
            return Ok();
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
            return Ok();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);
            await _applicationServiceState.RemoveAsync(state);
            return Ok();
        }
    }
}