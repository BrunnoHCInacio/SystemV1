using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

namespace SystemV1.Domain.Services
{
    public class ServiceState : Service, IServiceState
    {
        private readonly IRepositoryState _repositoryState;
        private readonly IRepositoryCountry _repositoryCountry;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceState(IRepositoryState repositoryState,
                            IUnitOfWork unitOfWork,
                            INotifier notifier, 
                            IRepositoryCountry repositoryCountry) : base(notifier)
        {
            _repositoryState = repositoryState;
            _unitOfWork = unitOfWork;
            _repositoryCountry = repositoryCountry;
        }

        public void Add(State state)
        {
            _repositoryState.Add(state);
        }

        public async Task AddAsyncUow(State state)
        {
            if (!RunValidation(state.ValidateState()))
            {
                return;
            }

            try
            {
                Add(state);
                await _unitOfWork.CommitAsync();
            }
            catch(Exception ex)
            {
                var t = ex;
                Notify("Falha ao adicionar novo estado");
            }
        }

        public async Task<IEnumerable<State>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryState.GetAllStatesAsync(page, pageSize);
        }

        public async Task<State> GetByIdAsync(Guid id)
        {
            return await _repositoryState.GetByIdAsync(id);
        }

        public async Task<IEnumerable<State>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }
            return await _repositoryState.GetByNameAsync(name);
        }

        public void Remove(State state)
        {
            state.IsActive = false;
            Update(state);
        }

        public async Task RemoveAsyncUow(State state)
        {
            Remove(state);
            await _unitOfWork.CommitAsync();
        }

        public void Update(State state)
        {
            _repositoryState.Update(state);
        }

        public async Task UpdateAsyncUow(State state)
        {
            if (!RunValidation(state.ValidateState()))
            {
                return;
            }

            Update(state);
            await _unitOfWork.CommitAsync();
        }
    }
}