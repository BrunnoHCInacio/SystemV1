﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Authorize]
    [Route("api/state")]
    public class StateController : MainController
    {
        private readonly IApplicationServiceState _applicationServiceState;

        public StateController(IApplicationServiceState applicationServiceState,
                               INotifier notifier) : base(notifier)
        {
            _applicationServiceState = applicationServiceState;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll(int page, int pageSize)
        {
            if (page == 0 && pageSize == 0)
            {
                Notify("É necessário informar a página e a quantidade de itens por página");
                OkResult();
            }

            var states = await _applicationServiceState.GetAllAsync(page, pageSize);
            return OkResult(states);
        }

        [HttpGet("GetById/{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var state = await _applicationServiceState.GetByIdAsync(id);
            return OkResult(state);
        }

        [HttpGet("GetStateCountryById/{id:guid}")]
        public async Task<ActionResult> GetStateCountryById(Guid id)
        {
            var state = await _applicationServiceState.GetStateCountryByIdAsync(id);
            return OkResult(state);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(StateViewModel stateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }
            await _applicationServiceState.AddAsync(stateViewModel);

            return OkResult();
        }

        [HttpPut("Update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, StateViewModel stateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            if (id != stateViewModel.Id)
            {
                Notify("O id informado é diferente do objeto.");
                return OkResult();
            }

            await _applicationServiceState.UpdateAsync(stateViewModel);
            return OkResult();
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            if (!await _applicationServiceState.ExistsAsync(id))
            {
                Notify("Estado não encontrado.");
                return OkResult();
            }

            await _applicationServiceState.RemoveAsync(id);
            return OkResult();
        }
    }
}