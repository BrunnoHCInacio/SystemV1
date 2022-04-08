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

        public Task<IEnumerable<City>> GetAllAsync(int page, int pageSize)
        {
            return await _repositoryCity.GetAllCitiesAsync(page, pageSize);
        }

        public Task<City> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<City>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
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
