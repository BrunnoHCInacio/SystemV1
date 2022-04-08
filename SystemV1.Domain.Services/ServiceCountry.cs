using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceCountry : Service, IServiceCountry
    {
        private readonly IRepositoryCountry _repositoryCountry;
        private readonly IRepositoryState _repositoryState;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCountry(IRepositoryCountry repositoryCountry,
                              IRepositoryState repositoryState,
                              IUnitOfWork unitOfWork,
                              INotifier notifier) : base(notifier)
        {
            _repositoryCountry = repositoryCountry;
            _unitOfWork = unitOfWork;
            _repositoryState = repositoryState;
        }

        public void Add(Country country)
        {
            _repositoryCountry.Add(country);
        }

        public async Task AddAsyncUow(Country country)
        {
            if (!CountryIsValid(country)) return;
            
            try
            {
                Add(country);
                AddStates(country.States);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o país");
            }
        }

        public async Task<IEnumerable<Country>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryCountry.GetAllCountriesAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                var t = ex;
                Notify("Falha ao obter todos os países");
            }
            return null;
        }

        public async Task<Country> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryCountry.GetCountryByIdAsync(id);
            }
            catch (Exception ex)
            {
                var t = ex;
                Notify("Falha ao obter país por id");
            }
            return null;
        }

        public async Task<IEnumerable<Country>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }
            try
            {
                return await _repositoryCountry.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter os países por nome.");
            }
            return null;
        }

        public void Remove(Country country)
        {
            country.States.ForEach(state => 
            {
                state.DisableRegister();
            });

            country.DisableRegister();
            Update(country);
        }

        public async Task RemoveAsyncUow(Country country)
        {
            try
            {
                Remove(country);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o país");
            }
        }

        public void Update(Country country)
        {
            _repositoryCountry.Update(country);
        }

        public async Task UpdateAsyncUow(Country country)
        {
            if (!IsValidEntity(country.ValidadeCountry()))
            {
                return;
            }

            if (country.States.Any())
            {
                if (country.States.Any(s => !IsValidEntity(s.ValidateState())))
                {
                    return;
                }
                try
                {
                    foreach (var state in country.States)
                    {
                        _repositoryState.Update(state);
                    }
                }
                catch (Exception)
                {
                    Notify("Falha ao alterar o estado.");
                    return;
                }
            }

            try
            {
                Update(country);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Notify("Falha ao alterar o país.");
            }
        }

        private bool CountryIsValid(Country country)
        {
            if (!IsValidEntity(country.ValidadeCountry()))
            {
                return false;
            }

            if (country.States.Any())
            {
                if (country.States.Any(s => !IsValidEntity(s.ValidateState())))
                {
                    return false;
                }
            }

            return true;
        }

        private void AddStates(List<State> states)
        {
            try
            {
                foreach (var state in states)
                {
                    _repositoryState.Add(state);
                }
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o estado.");
                throw;
            }
        }
    }
}