using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceState : Service<State>, IServiceState
    {
        private readonly IRepositoryState _repositoryState;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceState(IRepositoryState repositoryState, IUnitOfWork unitOfWork) : base(repositoryState, unitOfWork)
        {
            _repositoryState = repositoryState;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<State>> GetByNameAsync(string name)
        {
            return await _repositoryState.GetByNameAsync(name);
        }

        public void Remove(State state)
        {
            state.IsActive = false;
            _repositoryState.Update(state);
        }

        public async Task RemoveAsyncUow(State state)
        {
            Remove(state);
            await _unitOfWork.CommitAsync();
        }
    }
}