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
    public class ServiceCity : Service, IServiceCity
    {
        private readonly IRepositoryCity _repositoryCity;
        private readonly IUnitOfWork _unitOfWork;
        public ServiceCity(INotifier notifier,
                           IRepositoryCity repositoryCity, 
                           IUnitOfWork unitOfWork) : base(notifier)
        {
            _repositoryCity = repositoryCity;
            _unitOfWork = unitOfWork;
        }

        public void Add(City city)
        {
            _repositoryCity.Add(city);
        }

        public async Task AddAsyncUow(City city)
        {
            if (!IsValidEntity(city.ValidateCity()))
            {
                return;
            }

            try
            {
                Add(city);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar a cidade");
            }
        }

        public async Task<IEnumerable<City>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryCity.GetAllCitiesAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter as cidades.");
            }
            return null;
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            try 
            { 
                return await _repositoryCity.GetByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter a cidade.");
            }
            return null;
        }

        public async Task<IEnumerable<City>> GetByNameAsync(string name)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Notify("Informe o nome a consultar.");
                return null;
            }
            try
            {
                return await _repositoryCity.GetByNameAsync(name);
            }
            catch (Exception)
            {
                Notify("Falha ao obter a cidade.");
            }
            return null;
        }

        public void Remove(City city)
        {
            city.DisableRegister();
            _repositoryCity.Update(city);
        }

        public async Task RemoveAsyncUow(City city)
        {
            try
            {
                Remove(city);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover a cidade.");
            }
        }

        public void Update(City city)
        {
            _repositoryCity.Update(city);
        }

        public async Task UpdateAsyncUow(City city)
        {
            if (!IsValidEntity(city.ValidateCity()))
            {
                return;
            }

            try
            {
                Update(city);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao alterar a cidade.");
            }
        }
    }
}
