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
            catch (Exception ex)
            {
                var t = ex;
                Notify("Falha ao adicionar novo estado");
            }
        }

        public async Task<IEnumerable<State>> GetAllAsync(int page, int pageSize)
        {
            try 
            {
                return await _repositoryState.GetAllStatesAsync(page, pageSize);
            } 
            catch(Exception) 
            {
                Notify("Falha ao consultar todos os estados.");
            }
            return new List<State>();
        }

        public async Task<State> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryState.GetStateByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao consultar o estado por id.");
            }
            return null;
        }

        public async Task<State> GetStateCountryByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryState.GetStateCountryByIdAsync(id);
            }
            catch (Exception)
            {                
                Notify("Falha ao consultar o estado por id");
            }
            return null;
        }

        public async Task<IEnumerable<State>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }
            try
            {
                return await _repositoryState.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o state por nome.");
            }
            return null;
        }

        public void Remove(State state)
        {
            state.DisableRegister();
            Update(state);
        }

        public async Task RemoveAsyncUow(State state)
        {
            try
            {
                Remove(state);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover estado.");
            }
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

            try
            {
                Update(state);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao alterar o estado.");
            }
        }
    }
}