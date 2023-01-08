using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application
{
    public class ApplicationService<TService, TViewModel, TMapper, TEntity> : IApplicationService<TViewModel>
        where TMapper : IGenericMapper<TEntity, TViewModel>
        where TService : IService<TEntity>
        where TEntity : Entity
    {
        private readonly TService _service;
        private readonly TMapper _mapper;

        public ApplicationService(TService service,
                                  TMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task AddAsync(TViewModel viewModel)
        {
            var entity = _mapper.ViewModelToEntity(viewModel);
            await _service.AddAsyncUow(entity);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _service.ExistsAsync(e => e.Id == id);
        }

        public async Task<List<TViewModel>> GetAllAsync(int page, int pageSize)
        {
            var cities = await _service.SearchAsync(null, page, pageSize);
            return _mapper.ListEntityToViewModel(cities);
        }

        public async Task<TViewModel> GetByIdAsync(Guid id)
        {
            var city = await _service.GetEntityAsync(c => c.Id == id, null);
            return _mapper.EntityToViewModel(city);
        }

        public async Task RemoveAsync(Guid id)
        {
            var city = await _service.GetEntityAsync(e => e.Id == id);
            await _service.RemoveAsyncUow(city);
        }

        public async Task UpdateAsync(TViewModel viewModel)
        {
            var city = _mapper.ViewModelToEntity(viewModel);
            await _service.UpdateAsyncUow(city);
        }
    }
}