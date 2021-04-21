using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

namespace SystemV1.Domain.Services
{
    public class ServiceCountry : Service, IServiceCountry
    {
        private readonly IRepositoryCountry _repositoryCountry;
        private readonly IRepositoryState _repositoryState;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCountry(IRepositoryCountry repositoryCountry,
                              IUnitOfWork unitOfWork,
                              INotifier notifier) : base(notifier)
        {
            _repositoryCountry = repositoryCountry;
            _unitOfWork = unitOfWork;
        }

        public void Add(Country country)
        {
            _repositoryCountry.Add(country);
        }

        public async Task AddAsyncUow(Country country)
        {
            if (!RunValidation(new CountryValidation(), country))
            {
                return;
            }

            if (country.States.Any())
            {
                if (country.States.Any(s => !RunValidation(new StateValidation(), s)))
                {
                    return;
                }

                foreach (var state in country.States)
                {
                    _repositoryState.Add(state);
                }
            }

            Add(country);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Country>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryCountry.GetAllAsync(page, pageSize);
        }

        public async Task<Country> GetByIdAsync(Guid id)
        {
            return await _repositoryCountry.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Country>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar");
                return null;
            }

            return await _repositoryCountry.GetByNameAsync(name);
        }

        public void Remove(Country country)
        {
            country.IsActive = false;
            Update(country);
        }

        public async Task RemoveAsyncUow(Country country)
        {
            Remove(country);
            await _unitOfWork.CommitAsync();
        }

        public void Update(Country country)
        {
            _repositoryCountry.Update(country);
        }

        public async Task UpdateAsyncUow(Country country)
        {
            if (!RunValidation(new CountryValidation(), country))
            {
                return;
            }

            if (country.States.Any())
            {
                if (country.States.Any(s => !RunValidation(new StateValidation(), s)))
                {
                    return;
                }
            }

            Update(country);
            await _unitOfWork.CommitAsync();
        }
    }
}